using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(string id);
        Task<IdentityResult> UpdateUserAsync(string id, UserDto updatedUser);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<IdentityResult> CreateUserAsync(UserDto userDto);
    }
}