using Survey.API.DTOs.AnwserBlockDtos;

namespace Survey.xIntegrationTests.Tests.AnswerBlockTests
{
    public class AnswerBlockCreateTest : AnswerBlockFixture
    {
        public AnswerBlockCreateTest(WebApplicationFactory<Program> factory) : base(factory) { }

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

                var answerBlockEndpointsGet = new AnswerBlockClients(company.CompanyId, survey.SurveyID, answerBlockCreated.AnwserBlockID);

                var getAnswerBlockResponse = await GetAsync(answerBlockEndpointsGet.GetPutDeleteAnswerBlock, _client);
                var getAnswerBlockCreated = JsonConvert.DeserializeObject<AnwserBlockBasicInfoDto>(await getAnswerBlockResponse.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, answerBlockResponse.StatusCode);
                    Assert.Equal(getAnswerBlockCreated.AnwserBlockName, answerBlock.AnwserBlockName);
                    Assert.Equal(getAnswerBlockCreated.CodeOfAnwserBlock, answerBlock.CodeOfAnwserBlock);
                    Assert.Equal(getAnswerBlockCreated.BlockType, answerBlock.BlockType);
                });
            }
        }

        [Fact]
        public async Task PostAnswerBlock_InvalidEntries_ShouldReturnBadRequestError()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);

                var company = await CreateCompany(_client);

                var survey = await CreateSurvey(_client, company.CompanyId);

                var answerBlock = new AnwserBlockForCreationDto()
                {
                    AnwserBlockName = "WebStorage",
                    CodeOfAnwserBlock = 0,
                    BlockType = "Texts"
                };

                var answerBlockEndpoints = new AnswerBlockClients(company.CompanyId, survey.SurveyID);

                var answerBlockResponse = await PostAsync(answerBlockEndpoints.GetAllOrPostAnswerBlock, answerBlock, _client);
                var codeErrorMessage = await answerBlockResponse.Content.ReadAsStringAsync();

                answerBlock.CodeOfAnwserBlock = 1;
                answerBlock.AnwserBlockName = "x";

                var answerBlockNameResponse = await PostAsync(answerBlockEndpoints.GetAllOrPostAnswerBlock, answerBlock, _client);
                var nameErrorMessage = await answerBlockNameResponse.Content.ReadAsStringAsync();

                answerBlock.AnwserBlockName = "ValidName";
                answerBlock.BlockType = "x";

                var answerBlockBlockTypeResponse = await PostAsync(answerBlockEndpoints.GetAllOrPostAnswerBlock, answerBlock, _client);
                var blockTypeErrorMessage = await answerBlockBlockTypeResponse.Content.ReadAsStringAsync();

                Assert.Multiple(() =>
                {
                    Assert.Contains("'Code Of Anwser Block' must be greater than or equal to '1'.", codeErrorMessage);
                    Assert.Contains("'Anwser Block Name' must be between 2 and 255 characters. You entered 1 characters.", nameErrorMessage);
                    Assert.Contains("'Block Type' must be between 2 and 255 characters. You entered 1 characters.", blockTypeErrorMessage);
                });
            }
        }
    }
}
