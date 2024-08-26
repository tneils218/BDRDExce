namespace BDRDExce.Controllers;

using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    public UserController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        try
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("{id}")]
    public IActionResult GetUser(string id)
    {
        var user = _userManager.FindByIdAsync(id).Result;
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, UserDto updatedUser)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.FullName = updatedUser.FullName ?? user.FullName;
        user.Email = updatedUser.Email ?? user.Email;
        user.PhoneNumber = updatedUser.PhoneNumber ?? user.PhoneNumber;
        user.DOB = updatedUser.DOB ?? user.DOB;
        user.AvatarUrl = updatedUser.AvatarUrl ?? user.AvatarUrl;
        if (!string.IsNullOrWhiteSpace(updatedUser.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, updatedUser.Password);
            if (!passwordChangeResult.Succeeded)
            {
                return BadRequest(passwordChangeResult.Errors);
            }
        }
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok(user);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(UserDto userDto)
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

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded)
        {
            return Ok(user);
        }

        return BadRequest(result.Errors);
    }
}