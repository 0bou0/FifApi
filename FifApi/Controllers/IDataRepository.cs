using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FifApi.Models
{
    public interface IDataRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(int id, T entity);
        Task<T> DeleteAsync(int id);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);
        IEnumerable<T> GetAllAsEnumerable();
    }
}
