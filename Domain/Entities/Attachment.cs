namespace IncidentResponse.Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; }
    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public string UploadedBy { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
