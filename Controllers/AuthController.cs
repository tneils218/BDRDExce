using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BDRDExce.Commons.Utils;
using BDRDExce.Exceptions;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BDRDExce.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(SignInManager<AppUser> signInManager, IAuthService authService, UserManager<AppUser> userManager, ILogger<AuthController> logger, IConfiguration configuration)
    {
        _userManager = userManager;
        _authService = authService;
        _logger = logger;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task Login(LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        if(!result.Succeeded)
        {
            var response = new ResponseDto("Login failure");
            await Response.WriteAsJsonAsync(response);
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
        if (result.Succeeded)
        {
            return Ok("Verify Email Successfully");
        }
        return BadRequest("Verify Email Fail!");
    }
    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        var result = await _authService.RefreshTokenAsync(tokenModel);
        return Ok(result);
    }
}