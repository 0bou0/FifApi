using System.Collections.Generic;
using System.Threading.Tasks;

namespace FifApi.Models
{
    public interface IDataRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(int id, T entity);
        Task<T> DeleteAsync(int id);
    }
}
