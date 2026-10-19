namespace IncidentResponse.Domain.Entities;

public class IndicatorOfCompromise
{
    public Guid Id { get; set; }
    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;
    public string IocType { get; set; } = string.Empty; // Hash, Domain, IP, Email, URL
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime AddedAt { get; set; }
}
