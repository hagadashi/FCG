using FCG.Application.DTOs.Games;

namespace FCG.Application.Interfaces.Services.Games
{

    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetAllGamesAsync();
        Task<IEnumerable<GameDto>> GetActiveGamesAsync();
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<GameDto>> GetGamesByCategoryAsync(Guid categoryId);
        Task<GameDto?> GetGameByIdAsync(Guid id);
        Task<GameDto> CreateGameAsync(CreateGameDto createGameDto);
        Task<GameDto> UpdateGameAsync(Guid id, UpdateGameDto updateGameDto);
        Task<bool> DeleteGameAsync(Guid id);
        Task<IEnumerable<GameDto>> GetGamesOnSaleAsync();
    }

}