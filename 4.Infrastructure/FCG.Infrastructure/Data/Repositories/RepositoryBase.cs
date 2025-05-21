using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FCG.Infrastructure.Data.Repositories
{

    public abstract class RepositoryBase<T> : IRepository<T> where T : Entity
    {

        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected RepositoryBase(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
            => await _dbSet.AsNoTracking().ToListAsync();

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            int page = 1,
            int pageSize = 10)
        {
            return await _dbSet.Where(predicate)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .AsNoTracking()
                               .ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public virtual Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }

}