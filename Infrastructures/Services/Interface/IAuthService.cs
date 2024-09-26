using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IAuthService
    {
        Task<SignInResult> LoginAsync(LoginDto loginDto);
        Task LogoutAsync();
        Task<IdentityResult> RegisterAsync(RegisterDto userDto);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<string> ForgotPasswordAsync(BaseLoginDto userDto);
        Task<IdentityResult> AddRoleToUser(string userId, string roleName);
        Task<IdentityResult> VerifyEmailAsync(string emailHashCode);
    }
}