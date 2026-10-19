using IncidentResponse.Domain.Entities;

namespace IncidentResponse.Services;

public interface ICaseService
{
    Task<List<Case>> GetAllAsync();
    Task<Case?> GetByIdAsync(Guid id);
    Task<Case> CreateAsync(string title, string description, string assignedTo, string createdBy);
    Task LinkIncidentAsync(Guid caseId, Guid incidentId);
    Task<bool> CloseAsync(Guid id);
}
