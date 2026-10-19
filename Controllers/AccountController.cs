using IncidentResponse.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IncidentResponse.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password, [FromForm] string? returnUrl)
    {
        try
        {
            var (_, user) = await _authService.LoginAsync(username, password);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role),
                new("displayName", user.DisplayName),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            return Redirect(returnUrl ?? "/");
        }
        catch (UnauthorizedAccessException)
        {
            return Redirect($"/login?error=1");
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return Redirect("/login");
    }
}
