using AutoMapper;
using FCG.Application.DTOs.Games;
using FCG.Application.Interfaces.Services.Games;
using FCG.Application.Services.Games;
using FCG.Domain.Entities.Games;
using FCG.Domain.Interfaces.Repositories.Games;
using Moq;

namespace FCG.Tests.UnitTests
{
    public class GameServiceTest
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IGameService _gameService;

        public GameServiceTest()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _mapperMock = new Mock<IMapper>();

            _gameService = new GameService(
                _gameRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _saleRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task GetAllGamesAsync_DeveVoltarGamesDTO()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game("Game 1", "Description 1", 10.0m, "image1.jpg", new Category("Action", "Action Description")),
                new Game("Game 2", "Description 2", 20.0m, "image2.jpg", new Category("Adventure", "Adventure Description"))
            };

            _gameRepositoryMock.Setup(repo => repo.GetActiveGamesAsync())
                .ReturnsAsync(games);

            var gamesDto = new List<GameDto>
            {
                new GameDto { Title = "Game 1", Description = "Description 1", Price = 10.0m, CategoryName = "Action" },
                new GameDto { Title = "Game 2", Description = "Description 2", Price = 20.0m, CategoryName = "Adventure" }
            };

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<GameDto>>(games))
                .Returns(gamesDto);

            _saleRepositoryMock.Setup(repo => repo.GetActiveAsync())
                .ReturnsAsync(new List<Sale>());

            // Act
            var result = await _gameService.GetAllGamesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, g => g.Title == "Game 1");
            Assert.Contains(result, g => g.Title == "Game 2");
        }

        [Fact]
        public async Task GetGameByIdAsync_DeveRetornarGameDTO()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var category = new Category("Strategy", "Strategy Description");
            var game = new Game("Game 3", "Description 3", 30.0m, "image3.jpg", category);

            _gameRepositoryMock.Setup(repo => repo.GetByIdWithCategoryAsync(gameId))
                .ReturnsAsync(game);

            var gameDto = new GameDto
            {
                Title = "Game 3",
                Description = "Description 3",
                Price = 30.0m,
                CategoryName = "Strategy"
            };

            _mapperMock.Setup(mapper => mapper.Map<GameDto>(game))
                .Returns(gameDto);

            _saleRepositoryMock.Setup(repo => repo.GetActiveAsync())
                .ReturnsAsync(new List<Sale>());

            // Act
            var result = await _gameService.GetGameByIdAsync(gameId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Game 3", result.Title);
        }

        [Theory]
        [InlineData("New Game", "Um jogo divertido", 49.99, "image.jpg", "RPG", "Categoria de RPG")]
        [InlineData("Battle World", "Jogo de batalha", 59.90, "battle.jpg", "Ação", "Lutas intensas")]
        [InlineData("Puzzle Master", "Desafie seu cérebro", 29.90, "puzzle.jpg", "Puzzle", "Categoria lógica")]
        public async Task CreateGameAsync_TemQueVoltarUmGameDto(string nomeJogo, string descricaoJogo, decimal preco, string imagemUrl, string nomeCategoria, string descricaoCategoria)
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var createGameDto = new CreateGameDto
            {
                Name = nomeJogo,
                Description = descricaoJogo,
                Price = preco,
                ImageUrl = imagemUrl,
                CategoryId = categoryId
            };

            var category = new Category(nomeCategoria, descricaoCategoria);

            var game = new Game(
                createGameDto.Name,
                createGameDto.Description,
                createGameDto.Price,
                createGameDto.ImageUrl,
                category
            );

            var gameDto = new GameDto
            {
                Title = createGameDto.Name,
                Description = createGameDto.Description,
                Price = createGameDto.Price,
                CategoryName = category.Name
            };

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _gameRepositoryMock.Setup(repo => repo.GetByNameAsync(createGameDto.Name))
                .ReturnsAsync((Game)null);

            _gameRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Game>()))
                .ReturnsAsync(game);

            _gameRepositoryMock.Setup(repo => repo.SaveChangesAsync());

            _mapperMock.Setup(mapper => mapper.Map<GameDto>(It.IsAny<Game>()))
                .Returns(gameDto);

            // Act
            var result = await _gameService.CreateGameAsync(createGameDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nomeJogo, result.Title);
            Assert.Equal(descricaoJogo, result.Description);
            Assert.Equal(preco, result.Price);
            Assert.Equal(nomeCategoria, result.CategoryName);
        }


        [Theory]
        [InlineData("Old Game", "Novo Jogo", "Nova descrição", 59.99, "imagem_nova.jpg", "Ação", "Jogos de ação")]
        [InlineData("Velho RPG", "RPG Rebirth", "Reformulado e melhorado", 79.90, "rpg.jpg", "RPG", "Categoria RPG atualizada")]
        [InlineData("Puzzle Old", "Puzzle Novo", "Agora com mais desafios", 39.50, "puzzle_novo.jpg", "Puzzle", "Desafios mentais")]
        public async Task UpdateGameAsync_DeveAtualizarEVoltarGameDto(string nomeAntigo, string nomeNovo, string novaDescricao, decimal novoPreco, string novaImagem, string nomeCategoriaNova, string descricaoCategoriaNova)
        {
            // Arrange
            var gameId = Guid.NewGuid();

            var categoriaAntiga = new Category("Categoria Antiga", "Categoria antiga");
            var categoriaNova = new Category(nomeCategoriaNova, descricaoCategoriaNova);

            var jogoExistente = new Game(nomeAntigo, "Descrição Antiga", 10.0m, "antigo.jpg", categoriaAntiga);

            var updateGameDto = new UpdateGameDto
            {
                Name = nomeNovo,
                Description = novaDescricao,
                Price = novoPreco,
                ImageUrl = novaImagem,
                CategoryId = categoriaNova.Id,
                IsActive = true
            };

            var gameDto = new GameDto
            {
                Title = nomeNovo,
                Description = novaDescricao,
                Price = novoPreco,
                CategoryName = nomeCategoriaNova
            };

            _gameRepositoryMock.Setup(repo => repo.GetByIdAsync(gameId))
                .ReturnsAsync(jogoExistente);

            _gameRepositoryMock.Setup(repo => repo.GetByNameAsync(nomeNovo))
                .ReturnsAsync((Game)null);

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(updateGameDto.CategoryId.Value))
                .ReturnsAsync(categoriaNova);

            _gameRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Game>()))
                .ReturnsAsync(jogoExistente);

            _gameRepositoryMock.Setup(repo => repo.SaveChangesAsync());

            _mapperMock.Setup(mapper => mapper.Map<GameDto>(It.IsAny<Game>()))
                .Returns(gameDto);

            // Act
            var resultado = await _gameService.UpdateGameAsync(gameId, updateGameDto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(nomeNovo, resultado.Title);
            Assert.Equal(novaDescricao, resultado.Description);
            Assert.Equal(novoPreco, resultado.Price);
            Assert.Equal(nomeCategoriaNova, resultado.CategoryName);
        }

        [Fact]
        public async Task DeleteGameAsync_JogoExiste_DeveRetornarTrue()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var jogo = new Game("Jogo Teste", "Descrição", 49.99m, "img.jpg", new Category("Ação", "Jogos de ação"));

            _gameRepositoryMock.Setup(repo => repo.GetByIdAsync(gameId))
                .ReturnsAsync(jogo);

            _gameRepositoryMock.Setup(repo => repo.DeleteAsync(jogo))
                .Returns(Task.CompletedTask);

            _gameRepositoryMock.Setup(repo => repo.SaveChangesAsync());

            // Act
            var resultado = await _gameService.DeleteGameAsync(gameId);

            // Assert
            Assert.True(resultado);
            _gameRepositoryMock.Verify(repo => repo.DeleteAsync(jogo), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }



        [Fact]
        public async Task GetAllCategoriesAsync_DeveRetornarListaDeCategoryDto()
        {
            // Arrange
            var categories = new List<Category>
    {
        new Category("Ação", "Jogos de ação"),
        new Category("RPG", "Jogos de RPG")
    };

            var categoriesDto = new List<CategoryDto>
    {
        new CategoryDto { Name = "Ação", Description = "Jogos de ação" },
        new CategoryDto { Name = "RPG", Description = "Jogos de RPG" }
    };

            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<CategoryDto>>(categories))
                .Returns(categoriesDto);

            // Act
            var result = await _gameService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Ação");
            Assert.Contains(result, c => c.Name == "RPG");
        }

        [Fact]
        public async Task GetAllCategoriesAsync_SemCategorias_DeveRetornarListaVazia()
        {
            // Arrange
            var categories = new List<Category>();
            var categoriesDto = new List<CategoryDto>();

            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<CategoryDto>>(categories))
                .Returns(categoriesDto);

            // Act
            var result = await _gameService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetActiveGamesAsync_SemJogos_DeveRetornarListaVazia()
        {
            // Arrange
            var games = new List<Game>();
            var gamesDto = new List<GameDto>();

            _gameRepositoryMock.Setup(repo => repo.GetActiveGamesAsync())
                .ReturnsAsync(games);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<GameDto>>(games))
                .Returns(gamesDto);

            _saleRepositoryMock.Setup(repo => repo.GetActiveAsync())
                .ReturnsAsync(new List<Sale>());

            // Act
            var result = await _gameService.GetActiveGamesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetGamesOnSaleAsync_SemJogosEmPromocao_DeveRetornarListaVazia()
        {
            // Arrange
            var gamesOnSale = new List<Game>();
            var gamesDto = new List<GameDto>();

            _gameRepositoryMock.Setup(repo => repo.GetActiveGamesOnSaleAsync())
                .ReturnsAsync(gamesOnSale);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<GameDto>>(gamesOnSale))
                .Returns(gamesDto);

            _saleRepositoryMock.Setup(repo => repo.GetActiveAsync())
                .ReturnsAsync(new List<Sale>());

            // Act
            var result = await _gameService.GetGamesOnSaleAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetGamesOnSaleAsync_JogoSemSale_DeveRetornarIsOnSaleFalse()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var category = new Category("Puzzle", "Desc");
            var game = new Game("Game No Sale", "Desc", 150m, "img.jpg", category);

            // Use reflection to set the Id for testing purposes
            typeof(Game)
                .GetProperty("Id")
                .SetValue(game, gameId);

            var gamesOnSale = new List<Game> { game };

            var gamesDto = new List<GameDto>
                {
                    new GameDto { Id = gameId, Title = "Game No Sale", Price = 150m }
                };

            _gameRepositoryMock.Setup(repo => repo.GetActiveGamesOnSaleAsync())
                .ReturnsAsync(gamesOnSale);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<GameDto>>(gamesOnSale))
                .Returns(gamesDto);

            _saleRepositoryMock.Setup(repo => repo.GetActiveAsync())
                .ReturnsAsync(new List<Sale>());

            // Act
            var result = (await _gameService.GetGamesOnSaleAsync()).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.False(result[0].IsOnSale);
            Assert.Null(result[0].SalePrice);
        }
    }
}