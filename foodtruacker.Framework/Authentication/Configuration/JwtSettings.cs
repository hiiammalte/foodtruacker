namespace foodtruacker.Authentication.Configuration
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int ExpirationInMinutes { get; set; }
    }
}
