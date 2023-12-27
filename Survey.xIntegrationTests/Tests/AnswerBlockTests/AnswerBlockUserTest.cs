using Survey.API.DTOs.AnwserBlockDtos;

namespace Survey.xIntegrationTests.Tests.AnswerBlockTests
{
    public class AnswerBlockUserTest : AnswerBlockFixture
    {
        public AnswerBlockUserTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task DifferentUsers_UseAnswerBlock_ShouldReturnOk()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);

                var company = await CreateCompany(_client);

                var survey = await CreateSurvey(_client, company.CompanyId);

                var answerBlock = new AnwserBlockForCreationDto()
                {
                    AnwserBlockName = "WebStorage",
                    CodeOfAnwserBlock = 1,
                    BlockType = "Texts"
                };

                var answerBlockEndpoints = new AnswerBlockClients(company.CompanyId, survey.SurveyID);

                var answerBlockResponse = await PostAsync(answerBlockEndpoints.GetAllOrPostAnswerBlock, answerBlock, _client);
                var answerBlockCreated = JsonConvert.DeserializeObject<AnwserBlockBasicInfoDto>(await answerBlockResponse.Content.ReadAsStringAsync());

                // Second user can not access second users answer block
                HttpClient secondClient = await CreateAndAuthorizeSecondUser();

                var getAllResponseSecondUser = await GetAllAsync(answerBlockEndpoints.GetAllOrPostAnswerBlock, secondClient);
                var errorMessage = await getAllResponseSecondUser.Content.ReadAsStringAsync();

                await DeleteUserAsync("test@test.com");
                string expected = "No results for search. User is a mismatch or Company does not exist";

                Assert.Multiple(() =>
                {
                    Assert.Contains(expected, errorMessage);
                });
            }
        }
    }
}
