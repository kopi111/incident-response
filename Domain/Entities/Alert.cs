namespace IncidentResponse.Domain.Entities;

public class Alert
{
    public Guid Id { get; set; }
    public string AlertNumber { get; set; } = string.Empty; // ALT-2024-001
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty; // SIEM, Firewall, IDS, Manual
    public string Severity { get; set; } = "Medium"; // Critical, High, Medium, Low
    public string Status { get; set; } = "New"; // New, Acknowledged, Dismissed, Converted
    public Guid? ConvertedToIncidentId { get; set; }
    public DateTime ReceivedAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public string? AcknowledgedBy { get; set; }
}
