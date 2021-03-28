using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetAsync(int id);

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        public Task<IEnumerable<T>> GetAllAsync();

        ApplicationDBContext GetContext();

        public Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        public void Add(T entity);

        public void Delete(T entity);

        public void Update(T entity);

        public Task SaveChangesAsync();
    }
}
