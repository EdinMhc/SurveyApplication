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
            this._context = context;
            this.entities = this._context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return this.entities.AsQueryable();
        }

        public T GetByID(object id)
        {
            return this.entities.Find(id);
        }

        public IEnumerable<TType> Get<TType>(Expression<Func<T, bool>> where, Expression<Func<T, TType>> select) where TType : class
        {
            return this.entities.Where(where).Select(select).ToList();
        }

        public void Add(T entity)
        {
            this.entities.Add(entity);
        }

        public void Update(T entity)
        {
            this.entities.Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            this.entities.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int cityId)
        {
            return await this._context.Company.AnyAsync(c => c.CompanyID == cityId);
        }
    }
}
