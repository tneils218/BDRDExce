using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BDRDExce.Exceptions;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BDRDExce.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginController> _logger;

    public LoginController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration configuration,
        ILogger<LoginController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
                if (result.Succeeded)
                {
                    var tokenDetails = GenerateJwtToken(user);
                    var userDto = new UserDto(user, tokenDetails.Token, new DateTimeOffset(tokenDetails.Expires).ToUnixTimeMilliseconds());
                    return Ok(new ResponseDto("Ok", userDto));
                }
            }
            return Ok(new ResponseDto("Tài khoản hoặc mật khẩu không đúng!!!"));
        }
        catch (CustomException ex)
        {
            var response = ex.ToResponseDto();
            return Ok(response);
        }

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

        // Calculate the expiration time
        var expires = DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Return both the token and the expiration time
        return (Token: tokenString, Expires: expires);
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        var user = new AppUser
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            FullName = userDto.FullName,
            DOB = userDto.DOB,
            AvatarUrl = userDto.AvatarUrl,
        };
        var result = await _signInManager.UserManager.CreateAsync(user, userDto.Password);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(changePasswordDto.Email);
        if (user == null)
        {
            return NotFound();
        }
        var result = await _signInManager.UserManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(BaseLoginDto userDto)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(userDto.Email);
        if (user == null)
        {
            return NotFound();
        }
        var token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
        return Ok(token);
    }

}