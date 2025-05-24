using FCG.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces.Services.Users
{
    public interface IPasswordService
    {
        string GetHash(User user, string providedPassword);
        bool VerifyHash(User user, string providedPassword);
    }
}
