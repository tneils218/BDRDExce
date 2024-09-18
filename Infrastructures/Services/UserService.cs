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
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly string _key;

    public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _key = _configuration["KeyAes"];
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
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        // Check if the role exists, if not, create the role
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
        }

        // Add the role to the user
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if(result.Succeeded)
        {
            var removeResult = await _userManager.RemoveFromRoleAsync(user, user.Role);
            if (!removeResult.Succeeded)
            {
                return removeResult; // Trả về kết quả nếu loại bỏ vai trò cũ thất bại
            }
            user.Role = roleName;
            var updateResult = await _userManager.UpdateAsync(user);
            if(!updateResult.Succeeded)
            {
                return updateResult;
            }
        }
        return result;
    }
    
    public async Task<IdentityResult> VerifyEmailAsync(string emailHashCode)
    {
        // Giải mã emailHashCode -> Email
        var decryptedEmail = Utils.Decrypt(emailHashCode, _key);
        // Tìm người dùng có hash của email trùng với hashCodeEmail
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => decryptedEmail == u.Email);

        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Verification link is invalid or expired." });
        }
        if (user.EmailConfirmed)
        {
            // Email is already verified, no need to update the user
            return IdentityResult.Success;
        }

        // Xác nhận email đã được xác thực
        user.EmailConfirmed = true;

        // Cập nhật thông tin người dùng trong cơ sở dữ liệu
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return result;
        }
        throw new Exception("Email verification failed.");
    }
}
