using System;

namespace foodtruacker.Authentication.Exceptions
{
    class UserCannotBeFoundException : AuthenticationException
    {
        public UserCannotBeFoundException(string email) : base($"Idenity User cannot be found: {email}") { }
        public UserCannotBeFoundException(Guid userId) : base($"Idenity User cannot be found: {userId}") { }
    }
}
