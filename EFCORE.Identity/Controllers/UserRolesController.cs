using EFCORE.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EFCORE.Identity.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserRolesController(UserManager<AppUser> userManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Create (Guid userId,string roleName,CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByIdAsync(userId.ToString());

        if (appUser is null)
        {
            return BadRequest(new { Message = "Kullanıcı bulunamadı" });
        }

        IdentityResult result = await userManager.AddToRoleAsync(appUser, roleName);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(s => s.Description));
        }

        return NoContent();
    } 
    
}
