using FCG.Domain.Entities.Users;
using FCG.Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Data.Repositories.Users
{

    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {

        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
        }

    }

}