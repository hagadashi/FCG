using FCG.Domain.Entities.Users;

namespace FCG.Domain.Interfaces.Repositories.Users
{

    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);
        Task<Role> GetDefaultAsync();
    }

}