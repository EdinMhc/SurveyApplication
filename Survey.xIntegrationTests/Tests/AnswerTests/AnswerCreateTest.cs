using Survey.API.DTOs.AnwserDtos;

namespace Survey.xIntegrationTests.Tests.AnswerTests
{
    public class AnswerCreateTest : AnswerBlockFixture
    {
        public AnswerCreateTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task PostAnswer_ShouldReturnOk()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);

                var company = await CreateCompany(_client);

                var survey = await CreateSurvey(_client, company.CompanyId);

                var answerBlock = await CreateAnswerBlock(_client, company.CompanyId, survey.SurveyID);

                var question = await CreateQuestionAsync(_client, company.CompanyId, survey.SurveyID, answerBlock.AnwserBlockID);

                var answerEndpoints = new AnswerClients(company.CompanyId, answerBlock.AnwserBlockID);

                var answer = new AnswerForCreationDto()
                {
                    AnwserText = "CreativeAnswerToQuestion",
                };

                var answerResponse = await PostAsync(answerEndpoints.PostGetAll, answer, _client);
                var answerCreated = JsonConvert.DeserializeObject<AnswerBasicInfoDto>(await answerResponse.Content.ReadAsStringAsync());

                var answerEndpointsGet = new AnswerClients(company.CompanyId, answerBlock.AnwserBlockID, answerCreated.AnwserID);

                var answerResponseGet = await GetAsync(answerEndpointsGet.UpdateGetDelete, _client);
                var answerGet = JsonConvert.DeserializeObject<AnswerBasicInfoDto>(await answerResponseGet.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, answerResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, answerResponseGet.StatusCode);
                    Assert.Equal(answerGet.AnwserText, answerCreated.AnwserText);
                });
            }
        }

        [Fact]
        public async Task PostAnswer_InvalidEntries_ShouldReturnBadRequestError()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);

                var company = await CreateCompany(_client);

                var survey = await CreateSurvey(_client, company.CompanyId);

                var answerBlock = await CreateAnswerBlock(_client, company.CompanyId, survey.SurveyID);

                var question = await CreateQuestionAsync(_client, company.CompanyId, survey.SurveyID, answerBlock.AnwserBlockID);

                var answerEndpoints = new AnswerClients(company.CompanyId, answerBlock.AnwserBlockID);

                var answer = new AnswerForCreationDto()
                {
                    AnwserText = "",
                };

                var answerResponse = await PostAsync(answerEndpoints.PostGetAll, answer, _client);
                var error = await answerResponse.Content.ReadAsStringAsync();


                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.BadRequest, answerResponse.StatusCode);
                    Assert.Contains("Can not create Answer Text with empty property", error);
                });
            }
        }
    }
}
