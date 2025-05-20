using FCG.Domain.Entities.Users;

namespace FCG.Domain.Interfaces.Repositories.Users
{

    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);
    }

}