using FCG.Application.Interfaces.Services.Users;
using FCG.Domain.Entities.Users; 
using Microsoft.AspNetCore.Identity;

namespace FCG.Application.Services.Users
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string GetHash(User user, string providedPassword)
        {
            return _passwordHasher.HashPassword(user, providedPassword);
        }

        public bool VerifyHash(User user, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);

            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
