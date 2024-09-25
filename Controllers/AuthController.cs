using BDRDExce.Exceptions;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var userDto = await _authService.LoginAsync(loginDto);
            return Ok(new ResponseDto("Login successful!", userDto));
        }
        catch (CustomException ex)
        {
            var response = ex.ToResponseDto();
            return BadRequest(response);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto userDto)
    {
        var result = await _authService.RegisterAsync(userDto);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        try
        {
            var result = await _authService.ChangePasswordAsync(changePasswordDto);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }
        catch (CustomException ex)
        {
            var response = ex.ToResponseDto();
            return BadRequest(response);
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(BaseLoginDto userDto)
    {
        try
        {
            var token = await _authService.ForgotPasswordAsync(userDto);
            return Ok(token);
        }
        catch (CustomException ex)
        {
            var response = ex.ToResponseDto();
            return BadRequest(response);
        }
    }
    [HttpPost("role")]
    public async Task<IActionResult> AddRoleToUser(string userId, string roleName)
    {
        // Inject UserManager and RoleManager (via constructor or service locator)
        var result = await _authService.AddRoleToUser(userId, roleName);
        
        if (result.Succeeded)
        {
            return Ok($"Role '{roleName}' added to user.");
        }
        
        return BadRequest("Failed to add role to user.");
    }

    [HttpGet("verify")]
    public async Task<ActionResult> VerifyEmailAsync(string hashCodeEmail)
    {
        var result = await _authService.VerifyEmailAsync(hashCodeEmail);
        if(result.Succeeded)
        {
            return Ok("Verify Email Successfully");
        }
        return BadRequest("Verify Email Fail!");
    }
}