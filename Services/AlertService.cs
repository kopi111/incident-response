using IncidentResponse.Data;
using IncidentResponse.Domain.Entities;
using IncidentResponse.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponse.Services;

public class AlertService : IAlertService
{
    private readonly AppDbContext _db;

    public AlertService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Alert>> GetAllAsync(string? status = null)
    {
        var query = _db.Alerts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(a => a.Status == status);

        return await query
            .OrderByDescending(a => a.ReceivedAt)
            .ToListAsync();
    }

    public async Task<Alert> CreateAsync(string title, string description, string source, string severity)
    {
        var count = await _db.Alerts.CountAsync();
        var year = DateTime.UtcNow.Year;
        var alertNumber = $"ALT-{year}-{(count + 1):D3}";

        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            AlertNumber = alertNumber,
            Title = title,
            Description = description,
            Source = source,
            Severity = severity,
            Status = "New",
            ReceivedAt = DateTime.UtcNow
        };

        _db.Alerts.Add(alert);
        await _db.SaveChangesAsync();
        return alert;
    }

    public async Task<bool> AcknowledgeAsync(Guid id, string acknowledgedBy)
    {
        var alert = await _db.Alerts.FindAsync(id);
        if (alert == null || alert.Status != "New")
            return false;

        alert.Status = "Acknowledged";
        alert.AcknowledgedAt = DateTime.UtcNow;
        alert.AcknowledgedBy = acknowledgedBy;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DismissAsync(Guid id)
    {
        var alert = await _db.Alerts.FindAsync(id);
        if (alert == null || alert.Status == "Converted" || alert.Status == "Dismissed")
            return false;

        alert.Status = "Dismissed";
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Guid?> ConvertToIncidentAsync(Guid alertId, string createdBy)
    {
        var alert = await _db.Alerts.FindAsync(alertId);
        if (alert == null || alert.Status == "Converted")
            return null;

        var count = await _db.Incidents.CountAsync();
        var year = DateTime.UtcNow.Year;
        var incidentNumber = $"INC-{year}-{(count + 1):D3}";

        var severity = alert.Severity switch
        {
            "Critical" => Severity.SEV0,
            "High" => Severity.SEV1,
            "Medium" => Severity.SEV2,
            "Low" => Severity.SEV3,
            _ => Severity.SEV2
        };

        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = incidentNumber,
            Title = alert.Title,
            Description = $"Converted from Alert {alert.AlertNumber}: {alert.Description}",
            Severity = severity,
            Status = IncidentStatus.Triage,
            DetectedAt = alert.ReceivedAt,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        _db.Incidents.Add(incident);

        alert.Status = "Converted";
        alert.ConvertedToIncidentId = incident.Id;

        await _db.SaveChangesAsync();
        return incident.Id;
    }

    public async Task<int> GetNewCountAsync()
    {
        return await _db.Alerts.CountAsync(a => a.Status == "New");
    }
}
