namespace BDRDExce.Models.DTOs
{
    public class UserDto : BaseLoginDto
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public string AvatarUrl { get; set; }
        public string Token { get; set; }
        public long? Expires { get; set; }
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

        public UserDto(AppUser user, string token, long expires)
        {
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            DOB = user.DOB;
            AvatarUrl = user.AvatarUrl;
            Token = token;
            Expires = expires;
        }
    }

}