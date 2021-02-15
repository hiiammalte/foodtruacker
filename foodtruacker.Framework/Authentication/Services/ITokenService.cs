using foodtruacker.Authentication.Entities;
using System.Collections.Generic;

namespace foodtruacker.Authentication.Services
{
    public interface ITokenService
    {
        string GenerateJwt(User user, IList<string> roles);
    }
}
