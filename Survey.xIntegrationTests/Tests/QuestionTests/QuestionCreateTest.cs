using Survey.API.DTOs.QuestionDtos;

namespace Survey.xIntegrationTests.Tests.QuestionTests
{
    public class QuestionCreateTest : QuestionFixture
    {
        public QuestionCreateTest(WebApplicationFactory<Program> factory) : base(factory) { }



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
                    QuestionText = "Extraordinary question?",
                    QuestionType = "Text",
                };

                var questionResponse = await PostAsync(questionEndpoint.GetAllAndPost, question, _client);
                var createdQuestion = JsonConvert.DeserializeObject<QuestionBasicInfoDto>(await questionResponse.Content.ReadAsStringAsync());

                var getQuestionEndpoint = new QuestionClients(company.CompanyId, survey.SurveyID, createdQuestion.QuestionID);

                var getQuestionResponse = await GetAsync(getQuestionEndpoint.GetDeletePut, _client);
                var getQuestion = JsonConvert.DeserializeObject<QuestionBasicInfoDto>(await getQuestionResponse.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, questionResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, getQuestionResponse.StatusCode);
                    Assert.NotNull(createdQuestion);
                    Assert.Equal(getQuestion.QuestionType, createdQuestion.QuestionType);
                    Assert.Equal(getQuestion.QuestionText, createdQuestion.QuestionText);
                    Assert.Equal(getQuestion.Code, createdQuestion.Code);
                });
            }
        }

        [Fact]
        public async Task PostQuestion_InvalidEntries_ShouldReturnBadRequestError()
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
                    AnwserBlockID = 0,
                    Code = Guid.NewGuid().ToString(),
                    QuestionText = "Some creative question?",
                    QuestionType = "Text",
                };

                var firstQuestionResponse = await PostAsync(questionEndpoint.GetAllAndPost, question, _client);
                var firstErrorMessage = await firstQuestionResponse.Content.ReadAsStringAsync();

                await NormalizeQuestion(ref question, answerBlock.AnwserBlockID);
                question.Code = "x";

                var secondQuestionResponse = await PostAsync(questionEndpoint.GetAllAndPost, question, _client);
                var secondErrorMessage = await secondQuestionResponse.Content.ReadAsStringAsync();

                await NormalizeQuestion(ref question, answerBlock.AnwserBlockID);
                question.QuestionText = "x";

                var thirdQuestionResponse = await PostAsync(questionEndpoint.GetAllAndPost, question, _client);
                var thirdErrorMessage = await thirdQuestionResponse.Content.ReadAsStringAsync();

                await NormalizeQuestion(ref question, answerBlock.AnwserBlockID);
                question.QuestionType = "x";

                var fourthQuestionResponse = await PostAsync(questionEndpoint.GetAllAndPost, question, _client);
                var fourthErrorMessage = await fourthQuestionResponse.Content.ReadAsStringAsync();

                Assert.Multiple(() =>
                {
                    Assert.Contains(AnswerBlockNotValid, firstErrorMessage);
                    Assert.Contains(CodeNotValid, secondErrorMessage);
                    Assert.Contains(QuestionTextNotValid, thirdErrorMessage);
                    Assert.Contains(QuestionTypeNotValid, fourthErrorMessage);
                });
            }
        }

        private Task NormalizeQuestion(ref QuestionForCreationDto question, int answerBlockId)
        {
            question.AnwserBlockID = answerBlockId;
            question.Code = Guid.NewGuid().ToString();
            question.QuestionText = "What is your question again?";
            question.QuestionType = "Text";
            return Task.CompletedTask;
        }
    }
}
