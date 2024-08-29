using Microsoft.AspNetCore.Identity;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetRolesAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<IdentityResult> UpdateRoleAsync(string roleName, string updatedRoleName);
    }
}