using Alsiso.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Alsiso.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null ||
            !await userManager.IsInRoleAsync(user, "Admin"))
        {
            return Unauthorized(new
            {
                message = "Correo o contraseña incorrectos."
            });
        }

        var result = await signInManager.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: false,
            lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Unauthorized(new
            {
                message = "Correo o contraseña incorrectos."
            });
        }

        return Ok(new
        {
            message = "Sesión iniciada.",
            email = user.Email
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            email = User.Identity?.Name
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();
    }
}