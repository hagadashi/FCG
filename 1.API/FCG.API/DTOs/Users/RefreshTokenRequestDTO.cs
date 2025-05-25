using System.ComponentModel.DataAnnotations;

namespace FCG.API.DTOs.Users
{
    public class RefreshTokenRequestDTO
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
