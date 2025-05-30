using FCG.API.DTOs.Users;
using FCG.Application.Interfaces.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Controllers.Users
{
    public class AuthController : BaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> GenerateToken([FromBody] LoginRequestDTO data,
            [FromServices] IAuthService service)
        {
            var result = await service.Authenticate(data.Email, data.Password);

            if (result is null) return Unauthorized("Invalid username or password.");

            return Ok(AuthResponseDTO.From(result));
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO data,
            [FromServices] IAuthService service)
        {
            var result = await service.RefreshToken(data.Token, data.RefreshToken);

            if (result is null) return BadRequest("Expired token");

            return Ok(AuthResponseDTO.From(result));
        }

        [HttpPost("revoke/{id}")]
        [Authorize]
        public async Task<ActionResult> Revoke(Guid id, [FromServices] IAuthService service)
        {
            var result = await service.RevokeToken(id);

            if (!result) return BadRequest("Invalid user request");

            return NoContent();
        }
    }
}