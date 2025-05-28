using AutoMapper;
using FCG.Application.DTOs.Users;
using FCG.Domain.Entities.Users;
using FCG.Application.Interfaces.Services.Users;
using FCG.Domain.Interfaces.Repositories.Users;

namespace FCG.Application.Services.Users
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public UserService(IUserRepository userRepository,
                           IRoleRepository roleRepository,
                           IMapper mapper,
                           IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdWithRoleAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Verificar se o email já existe
            var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            // Obtem a role default
            var role = await _roleRepository.GetDefaultAsync();
            if (role == null)
                throw new KeyNotFoundException("Default role not found. Cannot create user.");

            var passwordHash = _passwordService.GetHash(null, createUserDto.Password);

            var user = new User(createUserDto.Username,
                                createUserDto.Email,
                                passwordHash,
                                createUserDto.Name,
                                createUserDto.LastName,
                                role);

            var createdUser = await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            // Verificar se o novo email já existe (se alterado)
            if (user.Email != updateUserDto.Email)
            {
                var existingUser = await _userRepository.GetByEmailAsync(updateUserDto.Email);
                if (existingUser != null)
                    throw new InvalidOperationException("User with this email already exists");
            }

            user.Update(updateUserDto.Name, updateUserDto.LastName, updateUserDto.Email);

            var updatedUser = await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<UserDto> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (!_passwordService.VerifyHash(user, changePasswordDto.OldPassword))
                throw new InvalidOperationException("Incorrect old password.");

            var newPasswordHash = _passwordService.GetHash(user, changePasswordDto.NewPassword);
            user.ChangePassword(newPasswordHash);

            var updatedUser = await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<UserDto> ChangeUserRoleAsync(Guid userId, ChangeUserRoleDto changeUserRoleDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var newRole = await _roleRepository.GetByIdAsync(changeUserRoleDto.NewRoleId);
            if (newRole == null)
                throw new KeyNotFoundException("New role not found.");

            user.ChangeRole(newRole);
            var updatedUser = await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<bool> ActivateUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            user.Activate();
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeactivateUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            user.Deactivate();
            await _userRepository.UpdateAsync(user);
            return true;
        }

    }

}