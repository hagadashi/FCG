using FCG.Domain.Entities.Games;
using FCG.Domain.Interfaces.Repositories.Games;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Data.Repositories.Games
{

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {

        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category> GetByIdWithGamesAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.Games.Where(g => g.IsActive))
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Name == name);
        }
    
    }

}