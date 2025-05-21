using FCG.Domain.Entities.Games;
using FCG.Domain.Interfaces.Repositories.Games;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Data.Repositories.Games
{

    public class LibraryRepository : RepositoryBase<Library>, ILibraryRepository
    {

        public LibraryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Library> GetByUserIdAndGameIdAsync(Guid userId, Guid gameId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(l => l.UserId == userId && l.GameId == gameId);
        }

        public async Task<IEnumerable<Library>> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(l => l.UserId == userId)
                .Include(l => l.Game)
                    .ThenInclude(g => g.Category)
                .ToListAsync();
        }

    }

}