using Survey.API.DTOs.AnwserDtos;

namespace Survey.xIntegrationTests.Tests.AnswerTests
{
    public class AnswerUpdateTest : AnswerBlockFixture
    {
        public AnswerUpdateTest(WebApplicationFactory<Program> factory) : base(factory) { }

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

                var answer = await CreateAnswer(_client, company.CompanyId, answerBlock.AnwserBlockID);

                var answerEndpointsGet = new AnswerClients(company.CompanyId, answerBlock.AnwserBlockID, answer.AnwserID);

                var request = new AnswerUpdateDto()
                {
                    AnwserText = "Updated Answer"
                };

                var answerResponse = await UpdateAsync(answerEndpointsGet.UpdateGetDelete, request, _client);
                var answerUpdated = JsonConvert.DeserializeObject<AnswerBasicInfoDto>(await answerResponse.Content.ReadAsStringAsync());


                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, answerResponse.StatusCode);
                    Assert.Equal(request.AnwserText, answerUpdated.AnwserText);
                    Assert.NotEqual(answer.AnwserText, answerUpdated.AnwserText);
                });
            }
        }
    }
}
