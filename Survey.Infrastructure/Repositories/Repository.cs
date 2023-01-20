namespace Survey.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Survey.Infrastructure.ContextClass1;
    using System.Linq.Expressions;

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ContextClass _context;
        private readonly DbSet<T> entities;

        public Repository(ContextClass context)
        {
            _context = context;
            entities = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return entities.AsQueryable();
        }

        public T GetByID(object id)
        {
            return entities.Find(id);
        }

        public IEnumerable<TType> Get<TType>(Expression<Func<T, bool>> where, Expression<Func<T, TType>> select) where TType : class
        {
            return entities.Where(where).Select(select).ToList();
        }

        public void Add(T entity)
        {
            entities.Add(entity);
        }

        public void Update(T entity)
        {
            entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            entities.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int companyId)
        {
            return await _context.Company.AnyAsync(c => c.CompanyID == companyId);
        }
    }
}
