using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace IncidentResponse.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
        return Task.FromResult(new AuthenticationState(user));
    }
}
