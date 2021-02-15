namespace foodtruacker.API.DTOs
{
    public class UpdatePasswordDTO
    {
        public long ExpectedVersion { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
