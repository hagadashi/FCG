using FCG.Domain.Entities.Users;

namespace FCG.Domain.Interfaces.Repositories.Users
{

    public interface ISessionRepository : IBaseRepository<Session>
    {
        Task<IEnumerable<Session>> GetActiveByUserIdAsync(Guid userId);
        Task<Session?> GetActiveByRefreshTokenAsync(Guid userId, string token);
    }

}