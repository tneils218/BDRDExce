using Microsoft.AspNetCore.Identity;

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
        public bool EmailConfirmed { get; set; }
        public IList<string> Role { get; set; }
        public UserDto()
        {

        }

        public UserDto(AppUser user, IList<string> role)
        {
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            DOB = user.DOB;
            AvatarUrl = user.AvatarUrl;
            Role = role;
        }

        public UserDto(AppUser user, string token, long expires, IList<string> role)
        {
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            DOB = user.DOB;
            AvatarUrl = user.AvatarUrl;
            Token = token;
            Expires = expires;
            Role = role;
            EmailConfirmed = user.EmailConfirmed;
        }
    }

}