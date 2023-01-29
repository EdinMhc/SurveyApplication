using Survey.API.DTOs.Company;

namespace Survey.xIntegrationTests.Tests.CompanyTest
{
    public class CompanyTestsCollection : ICollectionFixture<WebApplicationFactory<Program>> { }

    [Collection("CompanyTestsCollection")]
    public class CreateCompanyTest : FixtureImp
    {
        public CreateCompanyTest(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Post_WhenCalledWithValidData_ReturnsCreatedCompany()
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

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(companyInfo.Email, company.Email);
                    Assert.Equal(company.CompanyName, company.CompanyName);
                });
            }
        }

        [Fact]
        public async Task Post_WhenCalledWithInvalidData_ReturnsBadRequest()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);
                var endpoint = new CompanyClients();

                var companyInfo = new CompanyEditDto()
                {
                    CompanyName = "EdinsCompany",
                    Email = "test",
                    Address = "testAddress No.1",
                };

                var response = await PostAsync(endpoint.GetAllOrCreateCompany, companyInfo, _client);
                var error = await response.Content.ReadAsStringAsync();

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                    Assert.Contains("'Email' must be between 5 and 255 characters. You entered 4 characters", error);
                    Assert.Contains("Email is not a valid email address.", error);
                });
            }
        }
    }
}
