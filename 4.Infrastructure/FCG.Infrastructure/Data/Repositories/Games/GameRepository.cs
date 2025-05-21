using FCG.Domain.Entities.Games;
using FCG.Domain.Interfaces.Repositories.Games;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Data.Repositories.Games
{

    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {

        public GameRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Game>> GetActiveGamesAsync()
        {
            return await _dbSet
                .Where(g => g.IsActive)
                .Include(g => g.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _dbSet
                .Where(g => g.CategoryId == categoryId && g.IsActive)
                .ToListAsync();
        }

        public async Task<Game> GetByIdWithCategoryAsync(Guid id)
        {
            return await _dbSet
                .Include(g => g.Category)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game> GetByIdWithSalesAsync(Guid id)
        {
            return await _dbSet
                .Include(g => g.Sales.Where(s => s.IsActive && s.StartDate <= DateTime.UtcNow && s.EndDate >= DateTime.UtcNow))
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    
    }

}