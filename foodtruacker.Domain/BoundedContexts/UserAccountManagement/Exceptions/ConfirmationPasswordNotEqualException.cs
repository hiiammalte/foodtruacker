using foodtruacker.Domain.Exceptions;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.Exceptions
{
    public class ConfirmationPasswordNotEqualException : DomainException
    {
        public ConfirmationPasswordNotEqualException() : base("The Accounts existing password does not match the provided confirmation password.") { }
    }
}
