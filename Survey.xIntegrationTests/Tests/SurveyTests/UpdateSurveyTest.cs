using Survey.API.DTOs.SurveyDtos;
using Survey.xIntegrationTests.Clients;

namespace Survey.xIntegrationTests.Tests.SurveyTests
{
    public class UpdateSurveyTest : SurveyFixture
    {

        public UpdateSurveyTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task PutSurvey_ShouldReturnUpdatedSurvey()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);
                var companyId = await GetCompanyId();

                var createOrGetEndpoint = new SurveyClients(companyId);

                var createdSurvey = new SurveyForCreationDto()
                {
                    SurveyName = "TestSurveyName",
                    IsActive = true,
                };

                var surveyResponse = await PostAsync(createOrGetEndpoint.GetAllOrCreateSurvey, createdSurvey, _client);
                var survey = JsonConvert.DeserializeObject<SurveyBasicInfoDto>(await surveyResponse.Content.ReadAsStringAsync());

                var updateEndpoint = new SurveyClients(companyId, survey.SurveyID);

                var surveyGetResponse = await GetAsync(updateEndpoint.GetPutDeletePostSurvey, _client);
                var dbSurvey = JsonConvert.DeserializeObject<SurveyBasicInfoDto>(await surveyGetResponse.Content.ReadAsStringAsync());

                var surveyToUpdate = new SurveyUpdateDto()
                {
                    IsActive = false,
                    SurveyName = "OtherSurveyName",
                };

                var surveyUpdateResponse = await UpdateAsync(updateEndpoint.GetPutDeletePostSurvey, surveyToUpdate, _client);
                var updatedSurvey = JsonConvert.DeserializeObject<SurveyBasicInfoDto>(await surveyUpdateResponse.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, surveyResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, surveyGetResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, surveyUpdateResponse.StatusCode);
                    Assert.Equal(createdSurvey.IsActive, survey.IsActive);
                    Assert.Equal(createdSurvey.SurveyName, survey.SurveyName);
                    Assert.Equal(dbSurvey.SurveyName, survey.SurveyName);
                    Assert.Equal(dbSurvey.IsActive, survey.IsActive);
                    Assert.Equal(surveyToUpdate.SurveyName, updatedSurvey.SurveyName);
                    Assert.Equal(surveyToUpdate.IsActive, updatedSurvey.IsActive);
                });
            }
        }
    }
}
