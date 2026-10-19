namespace IncidentResponse.Domain.Entities;

public class AppUser
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Analyst, Manager, Executive, Admin
    public string DisplayName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
