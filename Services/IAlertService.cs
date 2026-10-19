using IncidentResponse.Domain.Entities;

namespace IncidentResponse.Services;

public interface IAlertService
{
    Task<List<Alert>> GetAllAsync(string? status = null);
    Task<Alert> CreateAsync(string title, string description, string source, string severity);
    Task<bool> AcknowledgeAsync(Guid id, string acknowledgedBy);
    Task<bool> DismissAsync(Guid id);
    Task<Guid?> ConvertToIncidentAsync(Guid alertId, string createdBy);
    Task<int> GetNewCountAsync();
}
