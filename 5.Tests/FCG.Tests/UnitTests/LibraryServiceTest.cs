using Moq;
using AutoMapper;
using FCG.Application.Services.Games;
using FCG.Application.DTOs.Games;
using FCG.Domain.Interfaces.Repositories.Games;
using FCG.Domain.Entities.Games;
using FCG.Domain.Entities.Users;

namespace FCG.Tests.UnitTests
{
    public class LibraryServiceTest
    {
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LibraryService _libraryService;

        public LibraryServiceTest()
        {
            _libraryRepositoryMock = new Mock<ILibraryRepository>();
            _mapperMock = new Mock<IMapper>();
            _libraryService = new LibraryService(_libraryRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetLibrariesByUserAsync_ReturnsMappedDtos()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Create mock entities
            var role = new Role("User", "Standard User", false);
            var user = new User("testuser", "test@example.com", "hashedpassword", "John", "Doe", role);
            var category = new Category("Action", "Action games");
            var game1 = new Game("Game 1", "Description 1", 59.99m, "image1.jpg", category);
            var game2 = new Game("Game 2", "Description 2", 39.99m, "image2.jpg", category);

            var libraries = new List<Library>
                {
                    new Library(user, game1),
                    new Library(user, game2)
                };

            var librariesDto = new List<LibraryDto>
                {
                    new LibraryDto { Id = libraries[0].Id, PurchasedAt = libraries[0].PurchasedAt },
                    new LibraryDto { Id = libraries[1].Id, PurchasedAt = libraries[1].PurchasedAt }
                };

            _libraryRepositoryMock
                .Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(libraries);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<LibraryDto>>(libraries))
                .Returns(librariesDto);

            // Act
            var result = await _libraryService.GetLibrariesByUserAsync(userId);

            // Assert
            Assert.Equal(librariesDto, result);
            _libraryRepositoryMock.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<LibraryDto>>(libraries), Times.Once);
        }

        [Fact]
        public async Task GetLibrariesByUserAsync_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Setup para retornar uma lista vazia ao invés de null
            var emptyLibraries = new List<Library>();

            _libraryRepositoryMock
                .Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(emptyLibraries);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<LibraryDto>>(emptyLibraries))
                .Returns(new List<LibraryDto>());

            // Act
            var result = await _libraryService.GetLibrariesByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _libraryRepositoryMock.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        }
    }
}