using Survey.API.DTOs.AnwserBlockDtos;

namespace Survey.xIntegrationTests.Tests.AnswerBlockTests
{
    public class AnswerBlockUpdateTest : AnswerBlockFixture
    {
        public AnswerBlockUpdateTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task UpdateAnswerBlock_ShouldReturnOk()
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

                var answerBlockEndpointsGet = new AnswerBlockClients(company.CompanyId, survey.SurveyID, answerBlockCreated.AnwserBlockID);

                answerBlock.AnwserBlockName = "UpdatedTest";

                var updateAnswerBlockResponse = await UpdateAsync(answerBlockEndpointsGet.GetPutDeleteAnswerBlock, answerBlock, _client);
                var updateAnswerBlockCreated = JsonConvert.DeserializeObject<AnwserBlockBasicInfoDto>(await updateAnswerBlockResponse.Content.ReadAsStringAsync());

                var getAnswerBlockResponse = await GetAsync(answerBlockEndpointsGet.GetPutDeleteAnswerBlock, _client);
                var getAnswerBlockCreated = JsonConvert.DeserializeObject<AnwserBlockBasicInfoDto>(await getAnswerBlockResponse.Content.ReadAsStringAsync());


                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, updateAnswerBlockResponse.StatusCode);
                    Assert.Equal(updateAnswerBlockCreated.AnwserBlockName, getAnswerBlockCreated.AnwserBlockName);
                    Assert.NotEqual(answerBlockCreated.AnwserBlockName, getAnswerBlockCreated.AnwserBlockName);
                });
            }
        }
    }
}
