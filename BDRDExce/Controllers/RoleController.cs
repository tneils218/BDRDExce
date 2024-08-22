using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult GetRoles(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }
        var role = _roleManager.FindByNameAsync(roleName).Result;
        if (role == null)
        {
            return NotFound();
        }
        return Ok(role);

    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var role = new IdentityRole(roleName);
        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            return Ok(role);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return NotFound();
        }
        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPut("{roleName}")]
    public async Task<IActionResult> UpdateRole(string roleName, string updatedRoleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return NotFound();
        }
        role.Name = updatedRoleName;
        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            return Ok(role);
        }
        return BadRequest(result.Errors);
    }
}