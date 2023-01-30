using Survey.API.DTOs.AnwserBlockDtos;

namespace Survey.xIntegrationTests.Tests.AnswerBlockTests
{
    public class AnswerBlockTest : AnswerBlockFixture
    {
        public AnswerBlockTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task PostAnswerBlock_ShouldReturnOk()
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

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, answerBlockResponse.StatusCode);
                    Assert.Equal(answerBlock.AnwserBlockName, answerBlockCreated.AnwserBlockName);
                    Assert.Equal(answerBlock.CodeOfAnwserBlock, answerBlockCreated.CodeOfAnwserBlock);
                    Assert.Equal(answerBlock.BlockType, answerBlockCreated.BlockType);
                });
            }
        }
    }
}
