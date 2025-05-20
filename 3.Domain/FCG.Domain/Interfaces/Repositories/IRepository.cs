using FCG.Domain.Entities;
using System.Linq.Expressions;

namespace FCG.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}