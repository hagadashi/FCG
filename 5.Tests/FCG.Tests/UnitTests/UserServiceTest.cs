using AutoMapper;
using FCG.Application.DTOs.Users;
using FCG.Application.Interfaces.Services.Users;
using FCG.Application.Services.Users;
using FCG.Domain.Entities.Users;
using FCG.Domain.Interfaces.Repositories.Users;
using Moq;

namespace FCG.Tests.UnitTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IRoleRepository> _roleRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _roleRepoMock = new Mock<IRoleRepository>();
            _mapperMock = new Mock<IMapper>();
            _passwordServiceMock = new Mock<IPasswordService>();

            _userService = new UserService(
                _userRepoMock.Object,
                _roleRepoMock.Object,
                _mapperMock.Object,
                _passwordServiceMock.Object
            );
        }


        [Fact]
        public async Task GetUserById_DeveRetornarUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var role = new Role("Usuário", "Permissão básica", true);

            var user = new User("userTest", "test@email.com", "hash_simulado", "João", "Silva", role);
            typeof(User).GetProperty("Id")?.SetValue(user, userId); // Força o ID se for readonly

            _userRepoMock.Setup(r => r.GetByIdWithRoleAsync(userId)).ReturnsAsync(user);

            var expectedUser = new UserDto
            {
                Name = "João",
                Email = "test@email.com",
                RoleName = "Usuário",
                IsActive = true
            };

            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Email, result.Email);
            Assert.Equal(expectedUser.RoleName, result.RoleName);
            Assert.True(result.IsActive);
        }


        [Fact]
        public async Task GetAllUsersAsync_DeveRetornarListaDeUserDto()
        {
            // Arrange
            var role = new Role("Usuário", "Permissão básica", true);

            var users = new List<User>
            {
                new User("user1", "user1@email.com", "hash1", "João", "Silva", role),
                new User("user2", "user2@email.com", "hash2", "Maria", "Oliveira", role),
                new User("user3", "user3@email.com", "hash3", "Carlos", "Souza", role)
            };

            // Simula retorno do repositório
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Prepara os usuários esperados após o mapeamento
            var userDtos = users.Select(user => new UserDto
            {
                Name = user.FirstName,
                Email = user.Email,
                RoleName = user.Role.Name,
                IsActive = true
            });

            // Configura o mock do AutoMapper para retornar o resultado esperado
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Collection(result,
                u => Assert.Equal("user1@email.com", u.Email),
                u => Assert.Equal("user2@email.com", u.Email),
                u => Assert.Equal("user3@email.com", u.Email)
            );
        }

        [Fact]
        public async Task GetUserByEmailAsync_DeveRetornarUserDtoOuNull()
        {
            var email = "teste@email.com";
            var role = new Role("Admin", "Administração", true);
            var user = new User("user1", email, "hash", "João", "Silva", role);

            _userRepoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Email = email });

            var result = await _userService.GetUserByEmailAsync(email);

            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Theory]
        [InlineData("user1", "João", "Silva", "joao@email.com", "Senha@123")]
        [InlineData("user2", "Maria", "Oliveira", "maria@email.com", "Teste@456")]
        [InlineData("user3", "Carlos", "Souza", "carlos@email.com", "Pass@word9")]
        [InlineData("user4", "Carlos", "Santos", "carlosemail.com", "password")]
        [InlineData("user5", "Carlos", "Santos", "carlos@email.com", "!passwor123d")]

        public async Task CreateUser_DeveRetornarUserDto(string username, string firstName, string lastName, string email, string password)
        {
            // Arrange
            var userDto = new CreateUserDto
            {
                Username = username,
                Name = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            
            var role = new Role("Usuário", "Permissão básica", true);
            _roleRepoMock.Setup(r => r.GetDefaultAsync()).ReturnsAsync(role);

           
            var hashedPassword = "hash_simulado";
            _passwordServiceMock.Setup(p => p.GetHash(null, password)).Returns(hashedPassword);

            
            var userEntity = new User(username, email, hashedPassword, firstName, lastName, role);
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(userEntity);

            
            var expectedUser = new UserDto
            {
                Name = firstName,
                Email = email,
                RoleName = "Usuário",
                IsActive = true
            };
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(expectedUser);

            // Act
            var result = await _userService.CreateUserAsync(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Email, result.Email);
            Assert.Equal(expectedUser.RoleName, result.RoleName);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task DeleteUserAsync_DeveRetornarTrue_QuandoUsuarioExiste()
        {
            var userId = Guid.NewGuid();
            var user = new User("user", "email@email.com", "hash", "Nome", "Sobrenome", new Role("Usuário", "Permissão básica", true));

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync());

            var result = await _userService.DeleteUserAsync(userId);

            Assert.True(result);
        }

        [Fact]
        public async Task ChangePasswordAsync_DeveAlterarSenha_QuandoSenhaAntigaEstiverCorreta()
        {
            var userId = Guid.NewGuid();
            var user = new User("user", "email@email.com", "hash", "Nome", "Sobrenome", new Role("Usuário", "Permissão básica", true));
            var dto = new ChangePasswordDto { OldPassword = "antiga", NewPassword = "nova" };

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _passwordServiceMock.Setup(p => p.VerifyHash(user, dto.OldPassword)).Returns(true);
            _passwordServiceMock.Setup(p => p.GetHash(user, dto.NewPassword)).Returns("novaHash");
            _userRepoMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.SaveChangesAsync());
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Email = user.Email });

            var result = await _userService.ChangePasswordAsync(userId, dto);

            Assert.Equal(user.Email, result.Email);
        }


        [Fact]
        public async Task UpdateUserAsync_DeveAtualizarUsuarioQuandoDadosValidos()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateDto = new UpdateUserDto
            {
                Name = "NovoNome",
                LastName = "NovoSobrenome",
                Email = "novo@email.com"
            };

            var role = new Role("User", "desc", false);
            var existingUser = new User("olduser", "old@email.com", "hash", "Old", "User", role);
            typeof(User).GetProperty("Id")?.SetValue(existingUser, userId);

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _userRepoMock.Setup(r => r.GetByEmailAsync(updateDto.Email)).ReturnsAsync((User)null);
            _userRepoMock.Setup(r => r.UpdateAsync(existingUser)).ReturnsAsync(existingUser);
            _mapperMock.Setup(m => m.Map<UserDto>(existingUser)).Returns(new UserDto { Email = updateDto.Email });

            // Act
            var result = await _userService.UpdateUserAsync(userId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Email, result.Email);
        }

        [Fact]
        public async Task ChangeUserRoleAsync_DeveAlterarPapelDoUsuario()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newRoleId = Guid.NewGuid();
            var oldRole = new Role("User", "desc", false);
            var newRole = new Role("Admin", "desc", false);

            var user = new User("user1", "email@email.com", "hash", "Joao", "Silva", oldRole);
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            typeof(Role).GetProperty("Id")?.SetValue(newRole, newRoleId);

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _roleRepoMock.Setup(r => r.GetByIdAsync(newRoleId)).ReturnsAsync(newRole);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { RoleName = newRole.Name });

            // Act
            var result = await _userService.ChangeUserRoleAsync(userId, new ChangeUserRoleDto { NewRoleId = newRoleId });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newRole.Name, result.RoleName);
        }

        [Fact]
        public async Task ActivateUserAsync_DeveAtivarUsuarioQuandoEncontrado()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var role = new Role("User", "desc", false);
            var user = new User("user1", "email@email.com", "hash", "Joao", "Silva", role);

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);

            // Act
            var result = await _userService.ActivateUserAsync(userId);

            // Assert
            Assert.True(result);
            Assert.True(user.IsActive);
        }

        [Fact]
        public async Task DeactivateUserAsync_DeveDesativarUsuarioQuandoEncontrado()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var role = new Role("User", "desc", false);
            var user = new User("user1", "email@email.com", "hash", "Joao", "Silva", role);

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);

            // Act
            var result = await _userService.DeactivateUserAsync(userId);

            // Assert
            Assert.True(result);
            Assert.False(user.IsActive);
        }




    }
}
