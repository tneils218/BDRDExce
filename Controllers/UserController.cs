using System.Security.Cryptography;
using System.Text;
using BDRDExce.Commons.Utils;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly string _key ;

    public UserController(IUserService userService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _userService = userService;
        _roleManager =roleManager;
        _userManager = userManager;
        _configuration = configuration;
        _key = _configuration["KeyAes"];
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
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
    public async Task<IActionResult> UpdateUser(string id, UserDto updatedUser)
    {
        try
        {
            var result = await _userService.UpdateUserAsync(id, updatedUser);
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

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(CreateUserDto userDto)
    {
        var result = await _userService.CreateUserAsync(userDto);
        if (result.Succeeded)
        {
            return Ok(userDto);
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("role")]
    public async Task<IActionResult> AddRoleToUser(string userId, string roleName)
    {
        // Inject UserManager and RoleManager (via constructor or service locator)
        var result = await _userService.AddRoleToUser(userId, roleName);
        
        if (result.Succeeded)
        {
            return Ok($"Role '{roleName}' added to user.");
        }
        
        return BadRequest("Failed to add role to user.");
    }

    [HttpGet("verify")]
    public async Task<ActionResult> VerifyEmailAsync(string hashCodeEmail)
    {
        var result = await _userService.VerifyEmailAsync(hashCodeEmail);
        if(result.Succeeded)
        {
            return Ok("Verify Email Successfully");
        }
        return BadRequest("Verify Email Fail!");
    } 
}
