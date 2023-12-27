using Survey.API.DTOs.QuestionDtos;

namespace Survey.xIntegrationTests.Tests.QuestionTests
{
    public class QuestionUpdateTest : QuestionFixture
    {
        public QuestionUpdateTest(WebApplicationFactory<Program> factory) : base(factory) { }

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

                var getQuestionResponse = await GetAsync(getQuestionEndpoint.GetDeletePut, _client);
                var getQuestion = JsonConvert.DeserializeObject<QuestionBasicInfoDto>(await getQuestionResponse.Content.ReadAsStringAsync());

                question.QuestionText = "Updated question";

                var updateQuestionResponse = await UpdateAsync(getQuestionEndpoint.GetDeletePut, question, _client);
                var updatedQuestion = JsonConvert.DeserializeObject<QuestionBasicInfoDto>(await updateQuestionResponse.Content.ReadAsStringAsync());

                var getUpdatedQuestionResponse = await GetAsync(getQuestionEndpoint.GetDeletePut, _client);
                var getUpdatedQuestion = JsonConvert.DeserializeObject<QuestionBasicInfoDto>(await getUpdatedQuestionResponse.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(getQuestion.QuestionType, createdQuestion.QuestionType);
                    Assert.NotEqual(getQuestion.QuestionText, updatedQuestion.QuestionText);
                    Assert.Equal(getUpdatedQuestion.QuestionText, updatedQuestion.QuestionText);
                });
            }
        }
    }
}
