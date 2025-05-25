using FCG.API.DTOs.Users;
using FCG.Application.Interfaces.Services.Users;
using FCG.Application.Settings;
using FCG.Domain.Entities.Users;
using FCG.Domain.Interfaces.Repositories.Users;
using Microsoft.Extensions.Options;

namespace FCG.Application.Services.Users
{
    public class AuthService(IUserRepository userRepository,
         ISessionRepository sessionRepository,
         IOptions<JwtSettings> jwtSettings,
         IPasswordService passwordService,
         IRoleRepository roleRepository,
         IJwtService jwtService) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISessionRepository _sessionRepository = sessionRepository;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<TokenDTO?> Authenticate(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null || !_passwordService.VerifyHash(user, password))
                return null;

            var role = await _roleRepository.GetByIdAsync(user.RoleId) ?? throw new Exception("Invalid role.");

            var token = _jwtService.GenerateToken(role.Name, user.Id);
            var refreshToken = _jwtService.GenerateRefreshToken();
            DateTime createDate = DateTime.UtcNow;
            DateTime tokenExpiration = createDate.AddHours(_jwtSettings.ExpireHours);
            DateTime refreshExpiration = createDate.AddHours(_jwtSettings.ExpireHours * 2);

            var session = new Session(refreshToken, refreshExpiration, user);

            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();

            return new TokenDTO()
            {
                Authenticated = true,
                CreatedAt = createDate,
                TokenExpirationAt = tokenExpiration,
                RefreshTokenExpirationAt = refreshExpiration,
                Token = token,
                RefreshToken = refreshToken,
            };
        }

        public async Task<TokenDTO?> RefreshToken(string accessToken, string refreshToken)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);

            if (principal is null) return null;

            var userId = Guid.Parse(principal.FindFirst("user_id")?.Value ?? throw new InvalidOperationException("Claim 'user_id' not found."));
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null) return null;

            var session = await _sessionRepository.GetActiveByRefreshTokenAsync(userId, refreshToken);

            if (session is null) return null;

            accessToken = _jwtService.GenerateToken(principal.Claims);
            refreshToken = _jwtService.GenerateRefreshToken();

            DateTime createDate = DateTime.UtcNow;
            DateTime tokenExpiration = createDate.AddHours(_jwtSettings.ExpireHours);
            DateTime refreshExpiration = createDate.AddHours(_jwtSettings.ExpireHours * 2);

            session.Refresh(refreshToken, refreshExpiration);
            await _sessionRepository.SaveChangesAsync();

            return new TokenDTO()
            {
                Authenticated = true,
                CreatedAt = createDate,
                TokenExpirationAt = tokenExpiration,
                RefreshTokenExpirationAt = refreshExpiration,
                Token = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<bool> RevokeToken(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null) return false;
            var sessions = await _sessionRepository.GetActiveByUserIdAsync(id);
            if (!sessions.Any()) return false;

            foreach (var session in sessions)
            {
                session.Deactivate();
            }

            await _sessionRepository.SaveChangesAsync();

            return true;
        }
    }
}
