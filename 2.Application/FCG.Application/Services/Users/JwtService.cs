using FCG.Application.Interfaces.Services.Users;
using FCG.Application.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FCG.Application.Services.Users
{
    public class JwtService (IOptions<JwtSettings> jwtSettings) : IJwtService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public SymmetricSecurityKey GetSecurityKey()
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var result = new SymmetricSecurityKey(key);

            return result;
        }

        public string GenerateToken(string roleName, Guid userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, roleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("user_id", userId.ToString())
            };

            return GenerateToken(claims);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            if (_jwtSettings.ExpireHours < 1) _jwtSettings.ExpireHours = 2;

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpireHours),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(
                    GetSecurityKey(),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _jwtSettings.Issuer,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }
}
