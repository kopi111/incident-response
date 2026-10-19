namespace IncidentResponse.Domain.Entities;

public class CaseIncident
{
    public Guid CaseId { get; set; }
    public Case Case { get; set; } = null!;
    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;
    public DateTime LinkedAt { get; set; }
}
