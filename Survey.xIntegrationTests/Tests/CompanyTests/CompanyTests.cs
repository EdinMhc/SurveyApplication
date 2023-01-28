using Survey.API.DTOs.Company;
using Survey.xIntegrationTests.Clients;

namespace Survey.xIntegrationTests.Tests.CompanyTest
{
    public class CompanyTestsCollection : ICollectionFixture<WebApplicationFactory<Program>> { }

    [Collection("CompanyTestsCollection")]
    public class CompanyTests : FixtureImp
    {
        public CompanyTests(WebApplicationFactory<Program> factory) : base(factory)
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

                var response = await PostAsync(endpoint.GetOrCreateCompany, companyInfo, _client);
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
        public async Task CrudFunctionality_WhenCalledWithValidData()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);
                var company = await CreateCompany(_client);
                var endpoint = new CompanyClients();

                var allCompaniesResponse = await GetAllAsync(endpoint.GetOrCreateCompany, _client);
                var responseString = await allCompaniesResponse.Content.ReadAsStringAsync();
                var companies = JsonConvert.DeserializeObject<CompanyDto[]>(responseString);

                var testCompany = companies.FirstOrDefault(x => x.CompanyName == "EdinsCompany");

                var crudEndpoint = new CompanyClients(company.CompanyId);

                CompanyEditDto companyToUpdate = new()
                {
                    Address = "ChangedAddress",
                    CompanyName = "Test",
                    Email = "Test@Test",
                };

                var updateCompanyResponse = await UpdateAsync(crudEndpoint.CrudOperations, companyToUpdate, _client);
                var deserializedCompany = JsonConvert.DeserializeObject<CompanyDto>(await updateCompanyResponse.Content.ReadAsStringAsync());

                var deleteCompanyResponse = await DeleteAsync(crudEndpoint.CrudOperations, _client);

                var tryGetCompanyResponse = await GetAsync(crudEndpoint.CrudOperations, _client);

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, allCompaniesResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, deleteCompanyResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, updateCompanyResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.BadRequest, tryGetCompanyResponse.StatusCode);
                    Assert.Equal(testCompany.CompanyName, company.CompanyName);
                    Assert.Equal(testCompany.CompanyId, company.CompanyId);
                    Assert.Equal(testCompany.Address, company.Address);
                    Assert.Equal(deserializedCompany.CompanyName, companyToUpdate.CompanyName);
                    Assert.Equal(deserializedCompany.Address, companyToUpdate.Address);
                });
            }
        }
    }
}
