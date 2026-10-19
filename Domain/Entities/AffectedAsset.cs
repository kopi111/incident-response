namespace IncidentResponse.Domain.Entities;

public class AffectedAsset
{
    public Guid Id { get; set; }
    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;
    public string Hostname { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string AssetStatus { get; set; } = "Active"; // Active, Isolated, Reimaged
    public int RiskScore { get; set; }
    public string? AssetType { get; set; } // Server, Workstation, Network
}
