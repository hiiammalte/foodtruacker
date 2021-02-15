using System;

namespace foodtruacker.Authentication.Exceptions
{
    class RoleCannotBeAssignedException : AuthenticationException
    {
        public RoleCannotBeAssignedException(string email) : base($"Idenity Role cannot be assigned to: {email}") { }
        public RoleCannotBeAssignedException(Guid userId) : base($"Idenity Role cannot be assigned to: {userId}") { }
    }
}
