namespace Survey.xIntegrationTests.Clients
{
    public class CompanyClients
    {
        public string GetOrCreateCompany { get; set; }
        public string CrudOperations { get; }

        public CompanyClients(int companyId)
        {
            CrudOperations = $"api/companies/{companyId}/";
        }

        public CompanyClients()
        {
            GetOrCreateCompany = "api/companies/";
        }
    }
}
