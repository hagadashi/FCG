using FCG.Domain.Entities.Users;
using FCG.Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Data.Repositories.Users
{

    public class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {

        public SessionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Session>> GetActiveByUserIdAsync(Guid userId)
        {
            DateTime now = DateTime.UtcNow;
            return await _dbSet
                .Where(s => s.UserId == userId && s.IsActive && s.ExpiresAt > now)
                .ToListAsync();
        }

        public async Task<Session?> GetActiveByRefreshTokenAsync(Guid userId, string token)
        {
            return await _dbSet.
                FirstOrDefaultAsync(s => s.UserId == userId && s.Token == token && s.IsActive && s.ExpiresAt > DateTime.UtcNow);
        }

    }

}