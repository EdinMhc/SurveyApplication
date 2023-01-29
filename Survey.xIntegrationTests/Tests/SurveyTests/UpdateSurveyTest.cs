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

                var createdSurvey = new SurveyCreationDto()
                {
                    SurveyName = "TestSurveyName",
                    IsActive = true,
                };

                var surveyResponse = await PostAsync(createOrGetEndpoint.CreateOrGetAllSurvey, createdSurvey, _client);
                var survey = JsonConvert.DeserializeObject<SurveyDto>(await surveyResponse.Content.ReadAsStringAsync());

                var updateEndpoint = new SurveyClients(companyId, survey.SurveyID);

                var surveyGetResponse = await GetAsync(updateEndpoint.GetPutDeletePostSurvey, _client);
                var dbSurvey = JsonConvert.DeserializeObject<SurveyDto>(await surveyGetResponse.Content.ReadAsStringAsync());

                var surveyToUpdate = new SurveyUpdateDto()
                {
                    IsActive = false,
                    SurveyName = "OtherSurveyName",
                };

                var surveyUpdateResponse = await UpdateAsync(updateEndpoint.GetPutDeletePostSurvey, surveyToUpdate, _client);
                var updatedSurvey = JsonConvert.DeserializeObject<SurveyDto>(await surveyUpdateResponse.Content.ReadAsStringAsync());

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

        [Fact]
        public async Task PutSurvey_ShouldReturnBadRequest()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);
                var companyId = await GetCompanyId();

                var createOrGetEndpoint = new SurveyClients(companyId);

                var surveyToCreate = new SurveyCreationDto()
                {
                    SurveyName = "TestSurveyName",
                    IsActive = true,
                };

                var createSurveyResponse = await PostAsync(createOrGetEndpoint.CreateOrGetAllSurvey, surveyToCreate, _client);
                var survey = JsonConvert.DeserializeObject<SurveyDto>(await createSurveyResponse.Content.ReadAsStringAsync());

                var updateEndpoint = new SurveyClients(companyId, survey.SurveyID);

                var surveyToUpdate = new SurveyUpdateDto()
                {
                    IsActive = null,
                    SurveyName = "a",
                };

                var surveyUpdateResponse = await UpdateAsync(updateEndpoint.GetPutDeletePostSurvey, surveyToUpdate, _client);

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, createSurveyResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.BadRequest, surveyUpdateResponse.StatusCode);
                });
            }
        }
    }
}
