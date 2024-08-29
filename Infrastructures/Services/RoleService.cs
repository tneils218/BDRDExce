namespace BDRDExce.Infrastructures.Services;

using BDRDExce.Infrastructures.Services.Interface;
using Microsoft.AspNetCore.Identity;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<IdentityRole>> GetRolesAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return _roleManager.Roles.ToList();
        }
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            return new List<IdentityRole> { role };
        }
        return Enumerable.Empty<IdentityRole>();
    }

    public async Task<IdentityResult> CreateRoleAsync(string roleName)
    {
        var role = new IdentityRole(roleName);
        var result = await _roleManager.CreateAsync(role);
        return result;
    }

    public async Task<IdentityResult> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new Exception("Role not found");
        }
        var result = await _roleManager.DeleteAsync(role);
        return result;
    }

    public async Task<IdentityResult> UpdateRoleAsync(string roleName, string updatedRoleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new Exception("Role not found");
        }
        role.Name = updatedRoleName;
        var result = await _roleManager.UpdateAsync(role);
        return result;
    }
}
