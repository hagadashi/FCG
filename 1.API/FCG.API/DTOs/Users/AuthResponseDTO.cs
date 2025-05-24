namespace FCG.API.DTOs.Users
{
    public class AuthResponseDTO
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public bool Authenticated { get; init; }
        public string Created { get; init; } = string.Empty;
        public string Expiration { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;

        public static AuthResponseDTO From(TokenDTO token)
        {
            return new AuthResponseDTO
            {
                Authenticated = token.Authenticated,
                Token = token.Token,
                RefreshToken = token.RefreshToken,
                Created = token.CreatedAt.ToString(DATE_FORMAT),
                Expiration = token.TokenExpirationAt.ToString(DATE_FORMAT)
            };
        }
    }
}
