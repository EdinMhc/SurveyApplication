using Survey.API.DTOs.QuestionDtos;

namespace Survey.xIntegrationTests.Tests.QuestionTests
{
    public class QuestionUserTest : QuestionFixture
    {
        public QuestionUserTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task UpdateQuestion_ShouldReturnOk()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);

                var company = await CreateCompany(_client);

                var survey = await CreateSurvey(_client, company.CompanyId);

                var answerBlock = await CreateAnswerBlock(_client, company.CompanyId, survey.SurveyID);

                var questionEndpoint = new QuestionClients(company.CompanyId, survey.SurveyID);

                var question = new QuestionForCreationDto()
                {
                    AnwserBlockID = answerBlock.AnwserBlockID,
                    Code = Guid.NewGuid().ToString(),
                    QuestionText = "Extraordinary question?",
                    QuestionType = "Text",
                };

                var questionResponse = await PostAsync(questionEndpoint.GetAllOrPost, question, _client);
                var createdQuestion = JsonConvert.DeserializeObject<QuestionBasicInfoDto>(await questionResponse.Content.ReadAsStringAsync());

                var getQuestionEndpoint = new QuestionClients(company.CompanyId, survey.SurveyID, createdQuestion.QuestionID);

                HttpClient secondClient = await CreateAndAuthorizeSecondUser();

                var getAllResponseSecondUser = await GetAllAsync(getQuestionEndpoint.GetAllOrPost, secondClient);
                var errorMessage = await getAllResponseSecondUser.Content.ReadAsStringAsync();

                await DeleteUserAsync("test@test.com");

                Assert.Multiple(() =>
                {
                    Assert.Contains("No results for search. User is a mismatch or Company does not exist", errorMessage);
                    Assert.Equal(HttpStatusCode.BadRequest, getAllResponseSecondUser.StatusCode);
                });
            }
        }
    }
}
