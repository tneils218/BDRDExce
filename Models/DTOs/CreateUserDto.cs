namespace BDRDExce.Models.DTOs
{
    public class CreateUserDto : BaseLoginDto
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public string AvatarUrl { get; set; }
        public string Token { get; set; }
        public long? Expires { get; set; }
    }
}