namespace foodtruacker.Authentication.Exceptions
{
    class RoleCannotBeFoundException : AuthenticationException
    {
        public RoleCannotBeFoundException(string role) : base($"Idenity Role cannot be found: {role}") { }
    }
}
