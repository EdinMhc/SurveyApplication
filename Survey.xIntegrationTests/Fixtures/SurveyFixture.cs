using Survey.API.DTOs.Company;
using Survey.xIntegrationTests.Clients;

namespace Survey.xIntegrationTests.Fixtures
{
    public class SurveyFixture : FixtureImp
    {
        public SurveyFixture(WebApplicationFactory<Program> factory) : base(factory) { }

        public async Task<int> GetCompanyId()
        {
            var endpoint = new CompanyClients();

            var companyInfo = new CompanyEditDto()
            {
                CompanyName = "EdinsCompany",
                Email = "test@test",
                Address = "testAddress No.1",
            };

            var response = await PostAsync(endpoint.GetOrCreateCompany, companyInfo, _client);
            var company = JsonConvert.DeserializeObject<CompanyDto>(await response.Content.ReadAsStringAsync());

            return company.CompanyId;
        }
    }
}
