using FCG.Domain.Entities.Games;

namespace FCG.Domain.Interfaces.Repositories.Games
{

    public interface IGameRepository : IBaseRepository<Game>
    {
        Task<Game> GetByIdWithCategoryAsync(Guid id);
        Task<Game> GetByIdWithSalesAsync(Guid id);
        Task<IEnumerable<Game>> GetActiveGamesOnSaleAsync();
        Task<Game> GetByNameAsync(string name);
        Task<IEnumerable<Game>> GetByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Game>> GetActiveGamesAsync();
    }

}