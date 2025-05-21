using FCG.Domain.Entities.Users;

namespace FCG.Domain.Interfaces.Repositories.Users
{

    public interface ISessionRepository : IRepository<Session>
    {
        Task<IEnumerable<Session>> GetActiveByUserIdAsync(Guid userId);
    }

}