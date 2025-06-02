using FCG.API.Controllers.Users;
using FCG.Application.DTOs.Users;
using FCG.Application.Interfaces.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FCG.Tests.UnitTests
{
    public class UserControllerTests
    {
        [Theory]
        [InlineData("user1", "João", "Silva", "joao@email.com", "Senha@123")]
        [InlineData("user2", "Maria", "Oliveira", "maria@email.com", "Teste@456")]
        [InlineData("user3", "Carlos", "Souza", "carlos@email.com", "Pass@word9")]
        public async Task CreateUser_TemQueRetornarUserDTO(string username, string name, string lastName, string email, string password)
        {
            // Arrange
            var mockService = new Mock<IUserService>();

            var userDto = new CreateUserDto
            {
                Username = username,
                Name = name,
                LastName = lastName,
                Email = email,
                Password = password
            };

            var expectedUser = new UserDto
            {
                Name = name,
                Email = email,
                RoleName = "Usuário",
                IsActive = true
            };

            mockService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserDto>()))
                .ReturnsAsync(expectedUser);

            var controller = new UserController(mockService.Object);

            // Act
            var result = await controller.CreateUser(userDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedUser = Assert.IsType<UserDto>(createdResult.Value);
            Assert.Equal(expectedUser.Email, returnedUser.Email);
            Assert.Equal(expectedUser.Name, returnedUser.Name);
        }
    }
}
