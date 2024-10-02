using BDRDExce.Commons.Utils;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services;
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = _userManager.Users.ToList();
        var userRolesList = new List<UserDto>();

        foreach (var user in users)
        {
            // Lấy vai trò của người dùng từ UserManager
            var roles = await _userManager.GetRolesAsync(user);

            // Tạo đối tượng ViewModel để chứa thông tin người dùng và vai trò
            var userWithRoles = new UserDto(user, roles.FirstOrDefault());

            userRolesList.Add(userWithRoles);
        }

        return userRolesList;
    }

    public async Task<AppUser> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<IdentityResult> UpdateUserAsync(string id, UpdateUserDto userDto, HttpRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        Media media = null;
        if(userDto.File != null)
        {
            using (var ms = new MemoryStream())
            {
                await userDto.File.CopyToAsync(ms);
                var fileBytes = ms.ToArray(); // Chuyển thành mảng byte
                var idMedia = Guid.NewGuid().ToString();
                media = new Media
                {
                    Id = idMedia,
                    ContentType = userDto.File.ContentType,
                    ContentName = userDto.File.FileName,
                    Content = fileBytes,
                    FileUrl = $"{request.Scheme}://{request.Host}/api/v1/Media/{idMedia}"
                };
            }
        }
        user.FullName = userDto.FullName ?? user.FullName;
        user.PhoneNumber = userDto.PhoneNumber ?? user.PhoneNumber;
        user.DOB = userDto.DOB ?? user.DOB;
        user.AvatarUrl = media.FileUrl;
        user.Media = media;

        if (!string.IsNullOrWhiteSpace(userDto.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, userDto.Password);
            if (!passwordChangeResult.Succeeded)
            {
                return passwordChangeResult;
            }
        }

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return await _userManager.DeleteAsync(user);
    }
}
