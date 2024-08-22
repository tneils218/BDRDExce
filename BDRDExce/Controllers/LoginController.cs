using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers;

[Route("api/v1")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInMangaer;

    public LoginController(SignInManager<AppUser> signInManager)
    {
        _signInMangaer = signInManager;
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _signInMangaer.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result);
    }

    [HttpPost("/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInMangaer.SignOutAsync();
        return Ok();
    }

    [HttpPost("/register")]
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
        var result = await _signInMangaer.UserManager.CreateAsync(user, userDto.Password);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("/change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = await _signInMangaer.UserManager.FindByEmailAsync(changePasswordDto.Email);
        if (user == null)
        {
            return NotFound();
        }
        var result = await _signInMangaer.UserManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("/forgot-password")]
    public async Task<IActionResult> ForgotPassword(BaseLoginDto userDto)
    {
        var user = await _signInMangaer.UserManager.FindByEmailAsync(userDto.Email);
        if (user == null)
        {
            return NotFound();
        }
        var token = await _signInMangaer.UserManager.GeneratePasswordResetTokenAsync(user);
        return Ok(token);
    }
}