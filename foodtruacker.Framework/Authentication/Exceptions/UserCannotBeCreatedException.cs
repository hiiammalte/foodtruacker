namespace foodtruacker.Authentication.Exceptions
{
    class UserCannotBeCreatedException : AuthenticationException
    {
        public UserCannotBeCreatedException(string email) : base($"Idenity User cannot be created: {email}") { }
    }
}
