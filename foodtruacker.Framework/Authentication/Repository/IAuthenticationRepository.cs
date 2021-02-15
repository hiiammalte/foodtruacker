using System;
using System.Threading.Tasks;

namespace foodtruacker.Authentication.Repository
{
    public interface IAuthenticationRepository
    {
        Task CreateAdmin(Guid userId, string email, string password);
        Task CreateCustomer(Guid userId, string email, string password);

        Task<string> GenerateEmailValidationToken(Guid userId);
        Task<string> GenerateChangeEmailValidationToken(Guid userId, string email);
        Task<string> GenerateHashedPassword(Guid userId, string password);
        Task<string> GenerateJWT(string email, string password);

        Task<bool> EmailAddressInUse(string email);
        Task<bool> UserHasRole(Guid userId, string role);
        Task<bool> UserExists(Guid userId);
        Task<bool> IsValidEmailVerificationToken(Guid userId, string verificationToken);
        Task<bool> IsCurrentPassword(Guid userId, string password);

        Task VerifyEmail(Guid userId, string verificationToken);
        Task ChangePassword(Guid userId, string hashedPassword);
    }
}
