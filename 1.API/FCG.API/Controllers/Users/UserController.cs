using FCG.Application.DTOs.Users;
using FCG.Application.Interfaces.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Controllers.Users;

[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Obtém todos os usuários
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return HandleResult(users);
    }

    /// <summary>
    /// Obtém um usuário por ID
    /// </summary>
    /// <param name="id">ID do usuário</param>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return HandleResult(user);
    }

    /// <summary>
    /// Obtém um usuário por email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        return HandleResult(user);
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <param name="createUserDto">Dados do usuário</param>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var user = await _userService.CreateUserAsync(createUserDto);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="updateUserDto">Dados atualizados do usuário</param>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        var user = await _userService.UpdateUserAsync(id, updateUserDto);
        return HandleResult(user, "Usuário atualizado com sucesso");
    }

    /// <summary>
    /// Remove um usuário
    /// </summary>
    /// <param name="id">ID do usuário</param>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var success = await _userService.DeleteUserAsync(id);

        if (!success)
            return NotFound("Usuário não encontrado");

        return NoContent();
    }
}