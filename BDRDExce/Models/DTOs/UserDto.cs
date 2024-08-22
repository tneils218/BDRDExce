namespace BDRDExce.Models.DTOs
{
    public class UserDto : BaseLoginDto
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public string AvatarUrl { get; set; }
    }
}