using IncidentResponse.Data;
using IncidentResponse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IncidentResponse.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<(string Token, AppUser User)> LoginAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        var token = GenerateToken(user);
        return (token, user);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<AppUser> CreateUserAsync(string username, string displayName, string password, string role)
    {
        if (await _db.Users.AnyAsync(u => u.Username == username))
            throw new InvalidOperationException($"Username '{username}' is already taken.");

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Username = username,
            DisplayName = displayName,
            Role = role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<List<AppUser>> GetAllUsersAsync()
    {
        return await _db.Users.OrderBy(u => u.CreatedAt).ToListAsync();
    }

    private string GenerateToken(AppUser user)
    {
        var key = _configuration["Jwt:Key"] ?? "incident-response-super-secret-jwt-key-2024-secure";
        var expiryHours = int.TryParse(_configuration["Jwt:ExpiryHours"], out var h) ? h : 8;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("displayName", user.DisplayName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiryHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
