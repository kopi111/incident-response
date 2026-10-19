using IncidentResponse.Data;
using IncidentResponse.Domain.Entities;
using IncidentResponse.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponse.Services;

public class IncidentService : IIncidentService
{
    private readonly AppDbContext _db;

    public IncidentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Incident>> GetAllAsync(IncidentStatus? status = null, Severity? severity = null, string? assignee = null)
    {
        var query = _db.Incidents
            .Include(i => i.Notes)
            .Include(i => i.AffectedAssets)
            .Include(i => i.IOCs)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (severity.HasValue)
            query = query.Where(i => i.Severity == severity.Value);

        if (!string.IsNullOrWhiteSpace(assignee))
            query = query.Where(i => i.AssignedTo == assignee);

        return await query
            .OrderBy(i => i.Severity)
            .ThenByDescending(i => i.DetectedAt)
            .ToListAsync();
    }

    public async Task<Incident?> GetByIdAsync(Guid id)
    {
        return await _db.Incidents
            .Include(i => i.Notes.OrderByDescending(n => n.CreatedAt))
            .Include(i => i.AffectedAssets)
            .Include(i => i.IOCs)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Incident> CreateAsync(CreateIncidentDto dto, string createdBy)
    {
        var count = await _db.Incidents.CountAsync();
        var year = DateTime.UtcNow.Year;
        var incidentNumber = $"INC-{year}-{(count + 1):D3}";

        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = incidentNumber,
            Title = dto.Title,
            Description = dto.Description,
            Severity = dto.Severity,
            Status = IncidentStatus.Triage,
            AssignedTo = dto.AssignedTo,
            DetectedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        _db.Incidents.Add(incident);
        await _db.SaveChangesAsync();
        return incident;
    }

    public async Task<Incident> UpdateStatusAsync(Guid id, IncidentStatus newStatus, string updatedBy)
    {
        var incident = await _db.Incidents.FindAsync(id)
            ?? throw new KeyNotFoundException($"Incident {id} not found");

        incident.Status = newStatus;
        incident.UpdatedAt = DateTime.UtcNow;

        if (newStatus == IncidentStatus.Containment && incident.ContainedAt == null)
            incident.ContainedAt = DateTime.UtcNow;

        if (newStatus == IncidentStatus.Closed && incident.ResolvedAt == null)
            incident.ResolvedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return incident;
    }

    public async Task<Incident> AssignAsync(Guid id, string assignTo, string updatedBy)
    {
        var incident = await _db.Incidents.FindAsync(id)
            ?? throw new KeyNotFoundException($"Incident {id} not found");

        incident.AssignedTo = assignTo;
        incident.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return incident;
    }

    public async Task<IncidentNote> AddNoteAsync(Guid id, string content, string author)
    {
        var incident = await _db.Incidents.FindAsync(id)
            ?? throw new KeyNotFoundException($"Incident {id} not found");

        var note = new IncidentNote
        {
            Id = Guid.NewGuid(),
            IncidentId = id,
            Content = content,
            Author = author,
            CreatedAt = DateTime.UtcNow
        };

        incident.UpdatedAt = DateTime.UtcNow;
        _db.IncidentNotes.Add(note);
        await _db.SaveChangesAsync();
        return note;
    }

    public async Task<AffectedAsset> IsolateAssetAsync(Guid incidentId, Guid assetId)
    {
        var asset = await _db.AffectedAssets
            .FirstOrDefaultAsync(a => a.Id == assetId && a.IncidentId == incidentId)
            ?? throw new KeyNotFoundException($"Asset {assetId} not found");

        asset.AssetStatus = "Isolated";

        var incident = await _db.Incidents.FindAsync(incidentId);
        if (incident != null)
            incident.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return asset;
    }

    public async Task<List<Incident>> GetByAssigneeAsync(string username)
    {
        return await _db.Incidents
            .Include(i => i.Notes)
            .Include(i => i.AffectedAssets)
            .Include(i => i.IOCs)
            .Where(i => i.AssignedTo == username && i.Status != IncidentStatus.Closed)
            .OrderBy(i => i.Severity)
            .ThenByDescending(i => i.DetectedAt)
            .ToListAsync();
    }
}
