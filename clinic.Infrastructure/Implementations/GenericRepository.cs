using clinic.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace clinic.Infrastructure.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly applicationContext _context;
        private DbSet<T> _Dbset;

        public GenericRepository(applicationContext context)
        {
            _context = context;
            _Dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _Dbset.Add(entity);
        }

        public void Delete(T Entity)
        {
            _Dbset.Remove(Entity);
        }

        public bool EntityExists(int id)
        {
            return _Dbset.Find(id) != null;
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>>? predicate = null, string? inludeWords = null)
        {
            IQueryable<T> query = _Dbset;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (inludeWords != null)
            {
                foreach (var word in inludeWords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(word);
                }
            }
            return query.ToList();
        }

        public T GetBy(System.Linq.Expressions.Expression<Func<T, bool>>? predicate = null, string? inludeWords = null)
        {
            IQueryable<T> query = _Dbset;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (inludeWords != null)
            {
                foreach (var word in inludeWords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(word);
                }
            }
            return query.SingleOrDefault();
        }

        public void Update(T entity)
        {
            _Dbset.Update(entity);
        }
    }

}
