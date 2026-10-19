using IncidentResponse.Data;
using IncidentResponse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponse.Services;

public class CaseService : ICaseService
{
    private readonly AppDbContext _db;

    public CaseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Case>> GetAllAsync()
    {
        return await _db.Cases
            .Include(c => c.CaseIncidents)
                .ThenInclude(ci => ci.Incident)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Case?> GetByIdAsync(Guid id)
    {
        return await _db.Cases
            .Include(c => c.CaseIncidents)
                .ThenInclude(ci => ci.Incident)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Case> CreateAsync(string title, string description, string assignedTo, string createdBy)
    {
        var count = await _db.Cases.CountAsync();
        var year = DateTime.UtcNow.Year;
        var caseNumber = $"CASE-{year}-{(count + 1):D3}";

        var newCase = new Case
        {
            Id = Guid.NewGuid(),
            CaseNumber = caseNumber,
            Title = title,
            Description = description,
            Status = "Open",
            AssignedTo = assignedTo,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Cases.Add(newCase);
        await _db.SaveChangesAsync();
        return newCase;
    }

    public async Task LinkIncidentAsync(Guid caseId, Guid incidentId)
    {
        var existing = await _db.CaseIncidents
            .FirstOrDefaultAsync(ci => ci.CaseId == caseId && ci.IncidentId == incidentId);

        if (existing != null)
            return;

        var link = new CaseIncident
        {
            CaseId = caseId,
            IncidentId = incidentId,
            LinkedAt = DateTime.UtcNow
        };

        _db.CaseIncidents.Add(link);

        var c = await _db.Cases.FindAsync(caseId);
        if (c != null)
            c.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task<bool> CloseAsync(Guid id)
    {
        var c = await _db.Cases.FindAsync(id);
        if (c == null)
            return false;

        c.Status = "Closed";
        c.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }
}
