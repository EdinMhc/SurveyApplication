using System.Linq.Expressions;

namespace Survey.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {

        IQueryable<T> GetAll();

        IEnumerable<TType> Get<TType>(Expression<Func<T, bool>> where, Expression<Func<T, TType>> select) where TType : class;

        T GetByID(object id);

        void Add(T entity);

        void Update(T dbEntity);

        void Delete(T entity);

        Task<bool> ExistsAsync(int id);
    }
}
