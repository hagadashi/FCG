using FCG.Domain.Entities.Users;
using FCG.Domain.Interfaces.Repositories;
using FCG.Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Data.Repositories.Users
{

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                               .Include(u => u.Role).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdWithRoleAsync(Guid id)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByIdWithLibraryAsync(Guid id)
        {
            return await _dbSet
                .Include(u => u.Libraries)
                    .ThenInclude(l => l.Game)
                        .ThenInclude(g => g.Category)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

    }

}