using EFCORE.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Identity.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class RoleController(
    RoleManager<AppRole> roleManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(string name,CancellationToken cancellationToken)
    {
        AppRole appRole = new()
        {
            Name = name
        };

        var result = await roleManager.CreateAsync(appRole);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(s => s.Description));
        }

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles.ToListAsync();

        return Ok(roles);
    }
}
