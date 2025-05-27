using FCG.Domain.Entities.Users;


namespace FCG.Domain.Interfaces.Repositories.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdWithRoleAsync(Guid id);
        Task<User> GetByIdWithLibraryAsync(Guid id);
    }
}