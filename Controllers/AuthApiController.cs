using IncidentResponse.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncidentResponse.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthApiController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var (token, user) = await _authService.LoginAsync(dto.Username, dto.Password);
            return Ok(new
            {
                token,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    displayName = user.DisplayName,
                    role = user.Role
                }
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var displayName = User.FindFirst("displayName")?.Value;

        return Ok(new
        {
            username,
            displayName,
            role
        });
    }
}
