namespace Survey.xIntegrationTests.Clients
{
    public class CompanyClients
    {
        public string CreateCompany { get; set; }
        public string CrudOperations { get; }
        public string GetAllCompanies { get; }
        public string UpdateCompany { get; }
        public string Delete { get; }

        public CompanyClients(int companyId)
        {
            CrudOperations = $"api/companies/{companyId}/";
        }

        public CompanyClients()
        {
            CreateCompany = "api/companies/";
            GetAllCompanies = "api/companies/";
        }
    }
}
