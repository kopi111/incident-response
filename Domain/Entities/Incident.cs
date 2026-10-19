using IncidentResponse.Domain.Enums;

namespace IncidentResponse.Domain.Entities;

public class Incident
{
    public Guid Id { get; set; }
    public string IncidentNumber { get; set; } = string.Empty; // INC-2024-001
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Severity Severity { get; set; }
    public IncidentStatus Status { get; set; }
    public string? AssignedTo { get; set; } // username
    public DateTime DetectedAt { get; set; }
    public DateTime? ContainedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? MitreTactics { get; set; } // JSON array stored as string
    public string? Tags { get; set; } // JSON array
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? SlaDeadline { get; set; }
    public bool SlaBreached { get; set; }
    public ICollection<IncidentNote> Notes { get; set; } = new List<IncidentNote>();
    public ICollection<AffectedAsset> AffectedAssets { get; set; } = new List<AffectedAsset>();
    public ICollection<IndicatorOfCompromise> IOCs { get; set; } = new List<IndicatorOfCompromise>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
