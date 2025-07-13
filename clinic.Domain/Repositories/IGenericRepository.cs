using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace clinic.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T GetBy(Expression<Func<T,bool>> ? predicate = null , string ? inludeWords =null );
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? includeWords = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        bool EntityExists(int id);
    }
}
