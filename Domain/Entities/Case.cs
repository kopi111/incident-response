namespace IncidentResponse.Domain.Entities;

public class Case
{
    public Guid Id { get; set; }
    public string CaseNumber { get; set; } = string.Empty; // CASE-2024-001
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Open"; // Open, InProgress, Closed
    public string AssignedTo { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<CaseIncident> CaseIncidents { get; set; } = new List<CaseIncident>();
}
