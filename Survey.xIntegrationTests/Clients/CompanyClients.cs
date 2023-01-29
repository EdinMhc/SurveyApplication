namespace Survey.xIntegrationTests.Clients
{
    public class CompanyClients
    {
        public string GetAllOrCreateCompany { get; set; }
        public string DeleteUpdateGetCompany { get; }

        /// <summary>
        /// Target specific company
        /// </summary>
        /// <param name="companyId"></param>
        public CompanyClients(int companyId)
        {
            DeleteUpdateGetCompany = $"api/companies/{companyId}/";
        }

        public CompanyClients()
        {
            GetAllOrCreateCompany = "api/companies/";
        }
    }
}
