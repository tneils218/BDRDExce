namespace BDRDExce.Infrastructures.Services;

using BDRDExce.Infrastructures.Services.Interface;
using Microsoft.AspNetCore.Identity;

public class RoleService(RoleManager<IdentityRole> roleManager) : IRoleService
{
    public async Task<IEnumerable<IdentityRole>> GetRolesAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return roleManager.Roles.ToList();
        }
        var role = await roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            return new List<IdentityRole> { role };
        }
        return Enumerable.Empty<IdentityRole>();
    }

    public async Task<IdentityResult> CreateRoleAsync(string roleName)
    {
        var role = new IdentityRole(roleName);
        var result = await roleManager.CreateAsync(role);
        return result;
    }

    public async Task<IdentityResult> DeleteRoleAsync(string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new Exception("Role not found");
        }
        var result = await roleManager.DeleteAsync(role);
        return result;
    }

    public async Task<IdentityResult> UpdateRoleAsync(string roleName, string updatedRoleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new Exception("Role not found");
        }
        role.Name = updatedRoleName;
        var result = await roleManager.UpdateAsync(role);
        return result;
    }
}
