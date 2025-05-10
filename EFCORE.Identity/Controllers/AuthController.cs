using EFCORE.Identity.Dtos;
using EFCORE.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Identity.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController(
    UserManager<AppUser> userManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
    {
        AppUser appUser = new()
        {
            Email = request.Email,
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        IdentityResult result = await userManager.CreateAsync(appUser, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(s => s.Description));
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByIdAsync(request.Id.ToString());

        if (appUser is null)
        {
            return BadRequest(new { Message = "Kullanıcı Bulunamadı" });
        }

        IdentityResult result = await userManager.ChangePasswordAsync(appUser, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(s => s.Description));
        }

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByEmailAsync(email);

        if (appUser is null)
        {
            return BadRequest(new { Message = "Kullanıcı Bulunamadı" });
        }

        string token = await userManager.GeneratePasswordResetTokenAsync(appUser);

        return Ok(new { Token = token });
    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordUsingToken(ChangePasswordUsingTokenDto request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByEmailAsync(request.Email);

        if (appUser is null)
        {
            return BadRequest(new { Message = "Kullanıcı Bulunamadı" });
        }

        IdentityResult result = await userManager.ResetPasswordAsync(appUser, request.Token, request.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(s => s.Description));
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Login (LoginDto request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(p => p.Email == request.UserNameOrEmail || p.UserName == request.UserNameOrEmail);

        if (appUser is null)
        {
            return BadRequest(new { Message = "Kullanıcı Bulunamadı" });
        }

        bool result = await userManager.CheckPasswordAsync(appUser, request.Password);

        if (!result)
        {
            return BadRequest(new { Message = "Şifre Yanlış" });
        }

        return Ok(new { Token = "Token" });
    }
}
