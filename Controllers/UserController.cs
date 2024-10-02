using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var userEmail = User.Identity.Name;
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDto updatedUser)
    {
        try
        {
            var result = await _userService.UpdateUserAsync(id, updatedUser, Request);
            if (result.Succeeded)
            {
                return Ok(updatedUser);
            }
            return BadRequest(result.Errors);
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User deleted successfully" });
            }
            return BadRequest(result.Errors);
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
