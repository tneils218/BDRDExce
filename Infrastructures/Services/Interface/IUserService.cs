using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(string id);
        Task<IdentityResult> UpdateUserAsync(string id, UserDto updatedUser);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<IdentityResult> CreateUserAsync(CreateUserDto userDto);
        Task<IdentityResult> AddRoleToUser(string userId, string roleName);
        Task<IdentityResult> VerifyEmailAsync(string emailHashCode);
    }
}