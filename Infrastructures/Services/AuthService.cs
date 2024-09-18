using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BDRDExce.Exceptions;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BDRDExce.Infrastructures.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSender _emailSender;

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
    }

    public async Task<UserDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(user);
                var tokenDetails = GenerateJwtToken(user);
                var userDto = new UserDto(user, tokenDetails.Token, new DateTimeOffset(tokenDetails.Expires).ToUnixTimeMilliseconds(), role);
                return userDto;
            }
        }
        throw new CustomException("Invalid credentials");
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto userDto)
    {
        var role = await _roleManager.FindByNameAsync("Students");
        var user = new AppUser
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            FullName = userDto.FullName,
            DOB = DateTime.Now.ToString("dd/MM/YYYY"),
            AvatarUrl = String.Empty,
            Role = role.Name
        };
        var result = await _signInManager.UserManager.CreateAsync(user, userDto.Password);
        if(result.Succeeded)
        {
            var resultAddRole = await _userManager.AddToRoleAsync(user, role.Name);
            if(!resultAddRole.Succeeded)
                return resultAddRole;
        }
        _emailSender.SendEmail(userDto.Email, "Reset Password",
                "Please reset your password by clicking here: <a href=''>link</a>");
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
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
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
}
