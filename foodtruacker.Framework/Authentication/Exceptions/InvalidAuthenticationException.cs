namespace foodtruacker.Authentication.Exceptions
{
    class InvalidAuthenticationException : AuthenticationException
    {
        public InvalidAuthenticationException() : base("Wrong Username or Password.") { }
    }
}
