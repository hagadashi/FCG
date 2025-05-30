using AutoMapper;
using FCG.Application.DTOs.Games;
using FCG.Application.Interfaces.Services.Games;
using FCG.Domain.Entities.Games;
using FCG.Domain.Interfaces.Repositories.Games;

namespace FCG.Application.Services.Games
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GameService(
            IGameRepository gameRepository,
            ICategoryRepository categoryRepository,
            ISaleRepository saleRepository,
            IMapper mapper)
        {
            _gameRepository = gameRepository;
            _categoryRepository = categoryRepository;
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
        {
            var games = await _gameRepository.GetActiveGamesAsync();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);

            await ApplySaleInfoAsync(gamesDto);
            return gamesDto;
        }

        public async Task<IEnumerable<GameDto>> GetActiveGamesAsync()
        {
            var games = await _gameRepository.GetActiveGamesAsync();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);

            await ApplySaleInfoAsync(gamesDto);
            return gamesDto;
        }

        public async Task<IEnumerable<GameDto>> GetGamesByCategoryAsync(Guid categoryId)
        {
            var games = await _gameRepository.GetByCategoryIdAsync(categoryId);
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);

            await ApplySaleInfoAsync(gamesDto);
            return gamesDto;
        }

        public async Task<GameDto?> GetGameByIdAsync(Guid id)
        {
            var game = await _gameRepository.GetByIdWithCategoryAsync(id);
            if (game == null)
                return null;

            var gameDto = _mapper.Map<GameDto>(game);
            await ApplySaleInfoAsync(new[] { gameDto });
            return gameDto;
        }

        public async Task<GameDto> CreateGameAsync(CreateGameDto createGameDto)
        {
            // Verificar se a categoria existe
            var category = await _categoryRepository.GetByIdAsync(createGameDto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            // Verificar se já existe um jogo com o mesmo nome
            var existingGame = await _gameRepository.GetByNameAsync(createGameDto.Name);
            if (existingGame != null)
                throw new InvalidOperationException("Game with this name already exists");

            var game = new Game(createGameDto.Name, createGameDto.Description, createGameDto.Price, createGameDto.ImageUrl, category);

            var createdGame = await _gameRepository.AddAsync(game);
            await _gameRepository.SaveChangesAsync();
            return _mapper.Map<GameDto>(createdGame);
        }

        public async Task<GameDto> UpdateGameAsync(Guid id, UpdateGameDto updateGameDto)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
                throw new KeyNotFoundException("Game not found");

            // Verificar se o novo nome já existe (se alterado)
            if (game.Title != updateGameDto.Name)
            {
                var existingGame = await _gameRepository.GetByNameAsync(updateGameDto.Name);
                if (existingGame != null && existingGame.Id != id)
                    throw new InvalidOperationException("Game with this name already exists");
            }

            var _category = game.Category;
            if (updateGameDto.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(updateGameDto.CategoryId.Value);
                if (category == null)
                    throw new KeyNotFoundException("Category not found");
                _category = category;
            }

            game.Update(updateGameDto.Name, updateGameDto.Description, updateGameDto.Price, updateGameDto.ImageUrl, _category);

            if (updateGameDto.IsActive.HasValue)
            {
                if (updateGameDto.IsActive.Value)
                    game.Activate();
                else
                    game.Deactivate();
            }

            var updatedGame = await _gameRepository.UpdateAsync(game);
            await _gameRepository.SaveChangesAsync();
            return _mapper.Map<GameDto>(updatedGame);
        }

        public async Task<bool> DeleteGameAsync(Guid id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
                return false;

            await _gameRepository.DeleteAsync(game);
            await _gameRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<GameDto>> GetGamesOnSaleAsync()
        {
            var gamesOnSale = await _gameRepository.GetActiveGamesOnSaleAsync();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(gamesOnSale);

            await ApplySaleInfoAsync(gamesDto);
            return gamesDto;
        }

        private async Task ApplySaleInfoAsync(IEnumerable<GameDto> games)
        {
            var activeSales = await _saleRepository.GetActiveAsync();

            foreach (var game in games)
            {
                var sale = activeSales.FirstOrDefault(s => s.GameId == game.Id);
                if (sale != null)
                {
                    game.IsOnSale = true;
                    game.SalePrice = game.Price * (1 - sale.DiscountPercentage / 100);
                }
                else
                {
                    game.IsOnSale = false;
                    game.SalePrice = null;
                }
            }
        }
    }
}