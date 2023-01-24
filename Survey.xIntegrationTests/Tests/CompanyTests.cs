using Survey.API.DTOs.Company;
using Survey.xIntegrationTests.Clients;
using System.Net.Http.Headers;

namespace Survey.xIntegrationTests.Tests
{
    public class CompanyTests : CompanyFixture
    {
        public CompanyTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Post_WhenCalledWithValidData_ReturnsCreatedCompany()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                var setupResult = await TestSetup(_client);

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, setupResult.response.StatusCode);
                    Assert.Equal(setupResult.companyInfo.Email, setupResult.company.Email);
                    Assert.Equal(setupResult.company.CompanyName, setupResult.company.CompanyName);
                    Assert.NotNull(setupResult.responseString);
                });
            }
        }

        [Fact]
        public async Task CrudFunctionality_WhenCalledWithValidData()
        {
            await using (var scope = await CreateUserScope(Role.Admin))
            {
                var setupResult = await TestSetup(_client);

                var allCompaniesResponse = await GetAllAsync(setupResult.endpoint.GetAllCompanies, _client);
                var responseString = await allCompaniesResponse.Content.ReadAsStringAsync();
                var company = JsonConvert.DeserializeObject<CompanyBasicInfoDto[]>(responseString);

                var testCompany = company.FirstOrDefault(x => x.CompanyName == "EdinsCompany");

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, allCompaniesResponse.StatusCode);
                    Assert.Equal(testCompany.CompanyName, setupResult.company.CompanyName);
                    Assert.Equal(testCompany.CompanyId, setupResult.company.CompanyId);
                    Assert.Equal(testCompany.Address, setupResult.company.Address);
                });

                var endpoint = new CompanyClients(testCompany.CompanyId);

                CompanyCreationDto companyToUpdate = new()
                {
                    Address = "ChangedAddress",
                    CompanyName = "Test",
                    Email = "Test@Test",
                };

                var json = CreateJsonContent(companyToUpdate);

                var updateCompanyResponse = await UpdateAsync(endpoint.CrudOperations, json, _client);
                var responseStringUpdated = await updateCompanyResponse.Content.ReadAsStringAsync();
                var updatedCompany = JsonConvert.DeserializeObject<CompanyBasicInfoDto>(responseStringUpdated);

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, updateCompanyResponse.StatusCode);
                    Assert.Equal(updatedCompany.CompanyName, companyToUpdate.CompanyName);
                    Assert.Equal(updatedCompany.Address, companyToUpdate.Address);
                });

                var deleteCompanyResponse = await DeleteAsync(endpoint.CrudOperations, _client);

                Assert.Equal(HttpStatusCode.OK, deleteCompanyResponse.StatusCode);

                var tryGetCompanyResponse = await GetAsync(endpoint.CrudOperations, _client);
                var responseStringGet = await tryGetCompanyResponse.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.BadRequest, tryGetCompanyResponse.StatusCode);
            }
        }

        private async Task<(CompanyBasicInfoDto company, CompanyCreationDto companyInfo, HttpResponseMessage response, string responseString, CompanyClients endpoint)> TestSetup(HttpClient client)
        {
            var token = await GetToken(_client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = new CompanyClients();

            var companyInfo = new CompanyCreationDto()
            {
                CompanyName = "EdinsCompany",
                Email = "test@test",
                Address = "testAddress No.1",
            };

            var json = CreateJsonContent(companyInfo);

            var response = await PostAsync(endpoint.CreateCompany, json, _client);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var company = JsonConvert.DeserializeObject<CompanyBasicInfoDto>(responseString);

            return (company, companyInfo, response, responseString, endpoint);
        }
    }
}
