using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BDRDExce.Infrastructures.Services;
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
        var userWithRoles = new UserDto(user, roles);

        userRolesList.Add(userWithRoles);
    }

    return userRolesList;
    }

    public async Task<AppUser> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<IdentityResult> UpdateUserAsync(string id, UserDto updatedUser)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        user.FullName = updatedUser.FullName ?? user.FullName;
        user.Email = updatedUser.Email ?? user.Email;
        user.PhoneNumber = updatedUser.PhoneNumber ?? user.PhoneNumber;
        user.DOB = updatedUser.DOB ?? user.DOB;
        user.AvatarUrl = updatedUser.AvatarUrl ?? user.AvatarUrl;

        if (!string.IsNullOrWhiteSpace(updatedUser.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, updatedUser.Password);
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

    public async Task<IdentityResult> CreateUserAsync(CreateUserDto userDto)
    {
        var role = await _roleManager.FindByNameAsync("Students");
        var user = new AppUser
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            FullName = userDto.FullName,
            DOB = userDto.DOB,
            AvatarUrl = userDto.AvatarUrl,
            Role = role.Name,
        };

        var result = await _userManager.CreateAsync(user, userDto.Password);
        if(result.Succeeded)
        {
            result = await _userManager.AddToRoleAsync(user, role.Name);
        }
        return result;
    }

    public async Task<IdentityResult> AddRoleToUser(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Check if the role exists, if not, create the role
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            throw new Exception("Role not found");
        }

        // Add the role to the user
        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result;
    }
}
