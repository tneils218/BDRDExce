using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using BDRDExce.Commons;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BDRDExce.AuthenticationHandler;

public class JwtHandler : JwtBearerHandler, IAuthenticationSignInHandler
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    private readonly int _refreshTokenValidityIndays;

    public JwtHandler(UserManager<AppUser> userManager, IConfiguration configuration, IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
        _configuration = configuration;
        _userManager = userManager;
        _refreshTokenValidityIndays = int.Parse(_configuration["Jwt:RefreshTokenValidityInDays"]);
    }

    public async Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
    { 
        var email = user.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        var appUser = _userManager.Users.Include(x => x.Media).FirstOrDefault(e => e.Email == email);
        var tokenInfo = GenerateJwtToken(user);
        var role = await _userManager.GetRolesAsync(appUser);
        var refreshToken = Utils.GenerateRefreshToken();
        appUser.RefreshToken = refreshToken;
        appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenValidityIndays);
        await _userManager.UpdateAsync(appUser);
        var file = appUser.Media != null ? new FileDto(appUser.Media.ContentName, appUser.Media.FileUrl) : new FileDto();
        var dto = new UserDto(appUser, tokenInfo.Token, refreshToken, new DateTimeOffset(tokenInfo.Expires).ToUnixTimeMilliseconds(), role.FirstOrDefault(), file);
        var response = new ResponseDto("Login successful!", dto);
        await Context.Response.WriteAsJsonAsync(response);
    }

    public Task SignOutAsync(AuthenticationProperties properties)
    => Task.CompletedTask;


    private (string Token, DateTime Expires) GenerateJwtToken(ClaimsPrincipal user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var expires = DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = user.Claims;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = signingCredentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };
        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (Token: token, Expires: expires);
    }
}