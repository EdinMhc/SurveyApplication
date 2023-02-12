using Survey.API.DTOs.Company;

namespace Survey.xIntegrationTests.Tests.CompanyTest
{
    public class UpdateCompanyTest : FixtureImp
    {
        public UpdateCompanyTest(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task UpdateCompany_WhenCalledWithValidData_ReturnsOk()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                await AuthenticateUser(_client);
                var company = await CreateCompany(_client);
                var endpoint = new CompanyClients();

                var crudEndpoint = new CompanyClients(company.CompanyId);

                var getCompany = await GetAsync(crudEndpoint.DeleteUpdateGetCompany, _client);
                var dbCompany = JsonConvert.DeserializeObject<CompanyDto>(await getCompany.Content.ReadAsStringAsync());

                CompanyEditDto companyToUpdate = new()
                {
                    Address = "ChangedAddress",
                    CompanyName = "Test",
                    Email = "Test@Test",
                };

                var updateCompanyResponse = await UpdateAsync(crudEndpoint.DeleteUpdateGetCompany, companyToUpdate, _client);

                var getUpdatedCompany = await GetAsync(crudEndpoint.DeleteUpdateGetCompany, _client);
                var dbUpdatedCompany = JsonConvert.DeserializeObject<CompanyDto>(await getUpdatedCompany.Content.ReadAsStringAsync());

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, getCompany.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, updateCompanyResponse.StatusCode);
                    Assert.Equal(HttpStatusCode.OK, getUpdatedCompany.StatusCode);
                    Assert.Equal(company.CompanyName, dbCompany.CompanyName);
                    Assert.Equal(company.CompanyName, dbCompany.CompanyName);
                    Assert.Equal(dbUpdatedCompany.CompanyName, companyToUpdate.CompanyName);
                    Assert.Equal(dbUpdatedCompany.Address, companyToUpdate.Address);
                });
            }
        }
    }
}
