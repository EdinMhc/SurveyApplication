using Survey.API.DTOs.QuestionDtos;

namespace Survey.xIntegrationTests.Tests.QuestionTests
{
    public class CreateQuestionTest : QuestionFixture
    {
        public CreateQuestionTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task PostQuestion_ShouldReturnOk()
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
                    QuestionText = "What is your question again?",
                    QuestionType = "Text",
                };

                var questionResponse = await PostAsync(questionEndpoint.GetAllAndPost, question, _client);
                var createdQuestion = JsonConvert.DeserializeObject<QuestionBasicInfoDto>(await questionResponse.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, questionResponse.StatusCode);
                    Assert.Equal(question.QuestionType, createdQuestion.QuestionType);
                    Assert.Equal(question.QuestionText, createdQuestion.QuestionText);
                    Assert.Equal(question.Code, createdQuestion.Code);
                    Assert.NotNull(createdQuestion);
                });
            }
        }
    }
}
