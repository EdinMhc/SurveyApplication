using Survey.API.DTOs.SurveyDtos;
using Survey.xIntegrationTests.Clients;

namespace Survey.xIntegrationTests.Tests.SurveyTests
{
    public class CreateSurveyTest : SurveyFixture
    {
        public CreateSurveyTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task PostSurvey_ShouldReturnCreatedSurvey()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);
                var companyId = await GetCompanyId();

                var surveyEndpoint = new SurveyClients(companyId);

                var createdSurvey = new SurveyForCreationDto()
                {
                    SurveyName = "TestSurveyName",
                    IsActive = true,
                };

                var surveyResponse = await PostAsync(surveyEndpoint.GetAllOrCreateSurvey, createdSurvey, _client);
                var survey = JsonConvert.DeserializeObject<SurveyBasicInfoDto>(await surveyResponse.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, surveyResponse.StatusCode);
                    Assert.Equal(createdSurvey.IsActive, survey.IsActive);
                    Assert.Equal(createdSurvey.SurveyName, survey.SurveyName);
                });
            }
        }

        [Fact]
        public async Task PostSurvey_ShouldBadRequest()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);
                var companyId = await GetCompanyId();

                var surveyEndpoint = new SurveyClients(companyId);

                var createdSurvey = new SurveyForCreationDto()
                {
                    SurveyName = "a",
                    IsActive = true,
                };

                var surveyResponse = await PostAsync(surveyEndpoint.GetAllOrCreateSurvey, createdSurvey, _client);
                var survey = JsonConvert.DeserializeObject<SurveyBasicInfoDto>(await surveyResponse.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.BadRequest, surveyResponse.StatusCode);
                });
            }
        }
    }
}
