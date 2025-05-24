namespace FCG.API.DTOs.Users
{
    public class TokenDTO
    {
        public bool Authenticated { get; set; }
        public DateTime CreatedAt { get; init; }

        public DateTime TokenExpirationAt { get; init; }
        public DateTime RefreshTokenExpirationAt { get; init; }

        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
