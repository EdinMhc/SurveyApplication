using Survey.Infrastructure.DapperRepository.StoredProcedure.DapperDto;

namespace Survey.Infrastructure.DapperRepository
{
    public interface IGenericRepository<T>
    {

        Task<IEnumerable<T>> GetAllAsync();

        Task DeleteRowAsync(Guid id);

        Task DeleteRowAsync(int id);

        Task<T> GetAsync(Guid id);

        Task<T> GetAsync(int id);

        Task<int> SaveRangeAsync(IEnumerable<T> list);

        Task StoredProcedureAsync(List<DapperCompanyCreationDto> company);

        Task UpdateAsync(T t);

        Task InsertAsync(T t);
    }
}
