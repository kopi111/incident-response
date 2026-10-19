using IncidentResponse.Domain.Entities;
using IncidentResponse.Domain.Enums;

namespace IncidentResponse.Services;

public record CreateIncidentDto(string Title, string Description, Severity Severity, string? AssignedTo);

public interface IIncidentService
{
    Task<List<Incident>> GetAllAsync(IncidentStatus? status = null, Severity? severity = null, string? assignee = null);
    Task<Incident?> GetByIdAsync(Guid id);
    Task<Incident> CreateAsync(CreateIncidentDto dto, string createdBy);
    Task<Incident> UpdateStatusAsync(Guid id, IncidentStatus newStatus, string updatedBy);
    Task<Incident> AssignAsync(Guid id, string assignTo, string updatedBy);
    Task<IncidentNote> AddNoteAsync(Guid id, string content, string author);
    Task<AffectedAsset> IsolateAssetAsync(Guid incidentId, Guid assetId);
    Task<List<Incident>> GetByAssigneeAsync(string username);
}
