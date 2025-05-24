using FCG.API.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces.Services.Users
{
    public interface IAuthService
    {
        Task<TokenDTO?> Authenticate(string email, string password);
        Task<TokenDTO?> RefreshToken(string token, string refreshToken);
        Task<bool> RevokeToken(Guid id);
    }
}
