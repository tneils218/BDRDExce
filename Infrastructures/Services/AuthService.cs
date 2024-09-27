using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BDRDExce.Commons.Utils;
using BDRDExce.Exceptions;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BDRDExce.Infrastructures.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSender _emailSender;
    private readonly string _key;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _emailSender = emailSender;
        _key = _configuration["KeyAes"];
    }

    public async Task<SignInResult> LoginAsync(LoginDto loginDto)
    {
        var r = _signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
        return result;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto userDto, HttpRequest request)
    {
        Media media;
        using (var ms = new MemoryStream())
            {
                await userDto.File.CopyToAsync(ms);
                var fileBytes = ms.ToArray(); // Chuyển thành mảng byte
                var id = Guid.NewGuid().ToString();
                media = new Media
                {
                    Id = id,
                    ContentType = userDto.File.ContentType,
                    ContentName = userDto.File.FileName,
                    Content = fileBytes,
                    FileUrl = $"{request.Scheme}://{request.Host}/api/v1/Media/{id}"
                };
                
            }
        var role = await _roleManager.FindByNameAsync("Students");
        var user = new AppUser
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            FullName = userDto.FullName,
            DOB = DateTime.Now.ToString("dd/MM/yyyy"),
            AvatarUrl = media.FileUrl,
            Role = role.Name,
            Media = media
        };
        var result = await _signInManager.UserManager.CreateAsync(user, userDto.Password);
        if (result.Succeeded)
        {
            var resultAddRole = await _userManager.AddToRoleAsync(user, role.Name);
            if (!resultAddRole.Succeeded)
                return resultAddRole;
            _emailSender.SendEmail(userDto.Email, "Reset Password",
                    "Please reset your password by clicking here: <a href=''>link</a>");
        }
        return result;
    }

    public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(changePasswordDto.Email);
        if (user == null)
        {
            throw new CustomException("User not found");
        }
        var result = await _signInManager.UserManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
        return result;
    }

    public async Task<string> ForgotPasswordAsync(BaseLoginDto userDto)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(userDto.Email);
        if (user == null)
        {
            throw new CustomException("User not found");
        }
        var token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
        _emailSender.SendEmail(userDto.Email, "Reset Password",
                "Please reset your password by clicking here: <a href='http://google.com'>link</a>");

        return token;
    }

    private (string Token, DateTime Expires) GenerateJwtToken(IdentityUser user)
    {
        var claims = new[]
        {
           new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (Token: tokenString, Expires: expires);
    }

    public async Task<IdentityResult> AddRoleToUser(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        // Check if the role exists, if not, create the role
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
        }
        // Add the role to the user
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            var removeResult = await _userManager.RemoveFromRoleAsync(user, user.Role);
            if (!removeResult.Succeeded)
            {
                return removeResult; // Trả về kết quả nếu loại bỏ vai trò cũ thất bại
            }
            user.Role = roleName;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return updateResult;
            }
        }
        return result;
    }

    public async Task<IdentityResult> VerifyEmailAsync(string emailHashCode)
    {
        // Giải mã emailHashCode -> Email
        var decryptedEmail = Utils.Decrypt(emailHashCode, _key);
        // Tìm người dùng có hash của email trùng với hashCodeEmail
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => decryptedEmail == u.Email);

        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Verification link is invalid or expired." });
        }
        if (user.EmailConfirmed)
        {
            // Email is already verified, no need to update the user
            return IdentityResult.Success;
        }

        // Xác nhận email đã được xác thực
        user.EmailConfirmed = true;

        // Cập nhật thông tin người dùng trong cơ sở dữ liệu
        var result = await _userManager.UpdateAsync(user);
        return result;
    }
}

