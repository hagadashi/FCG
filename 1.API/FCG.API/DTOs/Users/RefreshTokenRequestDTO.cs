using System.ComponentModel.DataAnnotations;

namespace FCG.API.DTOs.Users
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
