using BDRDExce.Infrastructures.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RoleController(IRoleService roleService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetRoles(string roleName)
    {
        var roles = await roleService.GetRolesAsync(roleName);
        if (!roles.Any())
        {
            return NotFound();
        }
        return Ok(roles);
    }


    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var result = await roleService.CreateRoleAsync(roleName);
        if (result.Succeeded)
        {
            return Ok(new { Message = "Role created successfully", RoleName = roleName });
        }
        return BadRequest(result.Errors);
    }

    [Authorize]
    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        try
        {
            var result = await roleService.DeleteRoleAsync(roleName);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Role deleted successfully" });
            }
            return BadRequest(result.Errors);
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{roleName}")]
    public async Task<IActionResult> UpdateRole(string roleName, string updatedRoleName)
    {
        try
        {
            var result = await roleService.UpdateRoleAsync(roleName, updatedRoleName);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Role updated successfully", UpdatedRoleName = updatedRoleName });
            }
            return BadRequest(result.Errors);
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
