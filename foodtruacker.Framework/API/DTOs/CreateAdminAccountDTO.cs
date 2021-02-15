namespace foodtruacker.API.DTOs
{
    public class CreateAdminAccountDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string SecretProductKey { get; set; }
    }
}
