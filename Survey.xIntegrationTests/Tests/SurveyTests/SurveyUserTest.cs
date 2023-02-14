namespace Survey.xIntegrationTests.Tests.SurveyTests
{
    public class SurveyUserTest : SurveyFixture
    {
        public SurveyUserTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task GetAllSurveys_ShouldReturnTwoSurveys()
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

                var firstSurveyResponse = await PostAsync(createOrGetEndpoint.CreateOrGetAllSurvey, createdSurvey, _client);
                var survey = JsonConvert.DeserializeObject<SurveyDto>(await firstSurveyResponse.Content.ReadAsStringAsync());

                var secondSurveyResponse = await PostAsync(createOrGetEndpoint.CreateOrGetAllSurvey, createdSurvey, _client);
                var secondSurvey = JsonConvert.DeserializeObject<SurveyDto>(await secondSurveyResponse.Content.ReadAsStringAsync());

                var getAllResponse = await GetAllAsync(createOrGetEndpoint.CreateOrGetAllSurvey, _client);
                string responseString = await getAllResponse.Content.ReadAsStringAsync();
                var deserialize = JsonConvert.DeserializeObject<List<SurveyDto>>(responseString);

                int expected = 2;

                HttpClient secondClient = await CreateAndAuthorizeSecondUser();

                var getAllResponseSecondUser = await GetAllAsync(createOrGetEndpoint.CreateOrGetAllSurvey, secondClient);
                var errorMessage = await getAllResponseSecondUser.Content.ReadAsStringAsync();

                await DeleteUserAsync("test@test.com");

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);
                    Assert.Equal(deserialize.Count, expected);
                    Assert.Contains("No results for search. User is a mismatch or Company does not exist", errorMessage);
                    Assert.Equal(HttpStatusCode.BadRequest, getAllResponseSecondUser.StatusCode);
                });
            }
        }
    }
}
