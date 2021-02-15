using foodtruacker.Domain.Exceptions;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.Exceptions
{
    public class UserAccountNotFoundException : DomainException
    {
        public UserAccountNotFoundException() : base("User Account not found.") { }
    }
}
