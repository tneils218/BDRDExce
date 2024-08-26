namespace BDRDExce.Models.DTOs
{
    public class UserDto : BaseLoginDto
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public string AvatarUrl { get; set; }
        public UserDto()
        {

        }

        public UserDto(AppUser user)
        {
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            DOB = user.DOB;
            AvatarUrl = user.AvatarUrl;
        }
    }

}