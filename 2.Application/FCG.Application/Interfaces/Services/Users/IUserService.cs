using FCG.Application.DTOs.Users;

namespace FCG.Application.Interfaces.Services.Users
{

    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
        Task<UserDto> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
        Task<UserDto> ChangeUserRoleAsync(Guid userId, ChangeUserRoleDto changeUserRoleDto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> ActivateUserAsync(Guid id);
        Task<bool> DeactivateUserAsync(Guid id);
    }

}