using Microsoft.AspNetCore.Identity;

namespace BDRDExce.Models.DTOs
{
    public class UserDto : BaseLoginDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public FileDto File { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public long? Expires { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Role { get; set; }
        public UserDto()
        {

        }

        public UserDto(AppUser user, string role, FileDto file)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            DOB = user.DOB;
            File = file;
            Role = role;
        }

        public UserDto(AppUser user, string token, string refreshToken, long expires, string role, FileDto file)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            DOB = user.DOB;
            File = file;
            Token = token;
            RefreshToken = refreshToken;
            Expires = expires;
            Role = role;
            EmailConfirmed = user.EmailConfirmed;
        }
    }

}