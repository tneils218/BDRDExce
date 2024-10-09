using BDRDExce.Commons;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services;
public class UserService(UserManager<AppUser> userManager) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = userManager.Users.Include(u => u.Media).ToList();
        var userRolesList = new List<UserDto>();

        foreach (var user in users)
        {
            // Lấy vai trò của người dùng từ UserManager
            var roles = await userManager.GetRolesAsync(user);
            var file = new FileDto(user.Media.ContentName, user.Media.FileUrl);
            // Tạo đối tượng ViewModel để chứa thông tin người dùng và vai trò
            var userWithRoles = new UserDto(user, roles.FirstOrDefault(), file);

            userRolesList.Add(userWithRoles);
        }

        return userRolesList;
    }

    public async Task<AppUser> GetUserByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task<IdentityResult> UpdateUserAsync(string id, UpdateUserDto userDto, HttpRequest request)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        Media media = null;
        if(userDto.File != null)
        {
            media = await Utils.ProcessUploadedFile(userDto.File, request);
        }
        user.FullName = userDto.FullName ?? user.FullName;
        user.PhoneNumber = userDto.PhoneNumber ?? user.PhoneNumber;
        user.DOB = userDto.DOB ?? user.DOB;
        user.AvatarUrl = media.FileUrl!;
        user.Media = media;

        if (!string.IsNullOrWhiteSpace(userDto.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var passwordChangeResult = await userManager.ResetPasswordAsync(user, token, userDto.Password);
            if (!passwordChangeResult.Succeeded)
            {
                return passwordChangeResult;
            }
        }

        return await userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteUserAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return await userManager.DeleteAsync(user);
    }
}
