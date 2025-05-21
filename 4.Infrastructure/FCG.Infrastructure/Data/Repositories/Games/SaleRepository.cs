using FCG.Domain.Entities.Games;
using FCG.Domain.Interfaces.Repositories.Games;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Data.Repositories.Games
{

    public class SaleRepository : RepositoryBase<Sale>, ISaleRepository
    {

        public SaleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Sale>> GetActiveAsync()
        {
            DateTime now = DateTime.UtcNow;
            return await _dbSet
                .Where(s => s.IsActive && s.StartDate <= now && s.EndDate >= now)
                .Include(s => s.Game)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetActiveByGameIdAsync(Guid gameId)
        {
            DateTime now = DateTime.UtcNow;
            return await _dbSet
                .Where(s => s.GameId == gameId && s.IsActive && s.StartDate <= now && s.EndDate >= now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByGameIdAsync(Guid gameId)
        {
            return await _dbSet
                .Where(s => s.GameId == gameId)
                .ToListAsync();
        }

    }

}