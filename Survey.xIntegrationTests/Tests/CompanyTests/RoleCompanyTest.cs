using Survey.API.DTOs.Company;

namespace Survey.xIntegrationTests.Tests.CompanyTest
{
    public class RoleCompanyTest : FixtureImp
    {
        public RoleCompanyTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task Get_WhenCalledWithOtherUser_ReturnsBadRequest()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);
                var endpoint = new CompanyClients();

                var companyInfo = new CompanyEditDto()
                {
                    CompanyName = "EdinsCompany",
                    Email = "test@test",
                    Address = "testAddress No.1",
                };

                var response = await PostAsync(endpoint.GetAllOrCreateCompany, companyInfo, _client);
                var company = JsonConvert.DeserializeObject<CompanyDto>(await response.Content.ReadAsStringAsync());

                var getEndpoint = new CompanyClients(company.CompanyId);

                HttpClient secondUser = await CreateAndAuthorizeSecondUser();

                var responseSecondUser = await GetAsync(getEndpoint.DeleteUpdateGetCompany, secondUser);
                string error = await responseSecondUser.Content.ReadAsStringAsync();

                await DeleteUserAsync(SecondUserEmail);

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.BadRequest, responseSecondUser.StatusCode);
                    Assert.Contains("", error);
                });
            }
        }
    }
}
