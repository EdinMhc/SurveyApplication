using Survey.Infrastructure.DapperRepository.StoredProcedure.DapperDto;
using Survey.Infrastructure.Entities;

namespace Survey.Domain.DapperServices.DapperCompanyServices
{
    public interface ICompanyServiceDapper
    {
        public Task<IEnumerable<Company>> GetAllAsync(string? role, string userId);

        public Task<Company> GetById(int CompanyId, string? role, string? userId);

        public Task<Company> CreateAsync(Company company, string? role, string? userId);

        public Task<Company> UpdateAsync(Company company, string? role, int companyId, string? userId);

        public Task<Company> UpdateDapper(List<DapperCompanyCreationDto> company, string? role, string userId);

        public Task<bool> DeleteAsync(int companyId, string? role, string? userId);

    }
}
