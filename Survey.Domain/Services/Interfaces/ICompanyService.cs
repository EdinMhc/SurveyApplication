namespace Survey.Domain.Services.Interfaces
{
    using Survey.Infrastructure.Entities;

    public interface ICompanyService
    {
        public Task<Company> CreateAsync(Company company, string? role, string? userId);

        Company GetById(int CompanyId, string? role, string? userId);

        public Task<Company> UpdateAsync(Company company, string? role, int companyId, string? userId);

        public Task<bool> DeleteAsync(int companyId, string? role, string? userId);

        public IEnumerable<Company> GetAll(string? role, string userId);
    }
}
