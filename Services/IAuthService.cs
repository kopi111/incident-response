using IncidentResponse.Domain.Entities;

namespace IncidentResponse.Services;

public record LoginDto(string Username, string Password);

public interface IAuthService
{
    Task<(string Token, AppUser User)> LoginAsync(string username, string password);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<AppUser> CreateUserAsync(string username, string displayName, string password, string role);
    Task<List<AppUser>> GetAllUsersAsync();
}
