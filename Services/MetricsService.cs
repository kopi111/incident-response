using IncidentResponse.Data;
using IncidentResponse.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponse.Services;

public class MetricsService : IMetricsService
{
    private readonly AppDbContext _db;

    public MetricsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummary> GetSummaryAsync()
    {
        var now = DateTime.UtcNow;
        var activeIncidents = await _db.Incidents
            .Where(i => i.Status != IncidentStatus.Closed)
            .ToListAsync();

        var critical = activeIncidents.Count(i => i.Severity == Severity.SEV0);
        var high = activeIncidents.Count(i => i.Severity == Severity.SEV1);
        var medium = activeIncidents.Count(i => i.Severity == Severity.SEV2);
        var totalActive = activeIncidents.Count;

        // SLA: SEV0/SEV1 open > 4 hours
        var slaBreaches = activeIncidents.Count(i =>
            (i.Severity == Severity.SEV0 || i.Severity == Severity.SEV1) &&
            (now - i.DetectedAt).TotalHours > 4);

        // Avg MTTR for resolved incidents
        var resolvedToday = await _db.Incidents
            .Where(i => i.Status == IncidentStatus.Closed && i.ResolvedAt != null &&
                        i.ResolvedAt.Value.Date == now.Date)
            .ToListAsync();

        var mttrList = await _db.Incidents
            .Where(i => i.ResolvedAt != null)
            .Select(i => new { i.DetectedAt, i.ResolvedAt })
            .ToListAsync();

        int avgMttr = 0;
        if (mttrList.Count > 0)
        {
            var avgMinutes = mttrList
                .Select(i => (i.ResolvedAt!.Value - i.DetectedAt).TotalMinutes)
                .Average();
            avgMttr = (int)avgMinutes;
        }

        return new DashboardSummary(
            TotalActive: totalActive,
            Critical: critical,
            High: high,
            Medium: medium,
            AvgMttrMinutes: avgMttr,
            SlaBreaches: slaBreaches,
            Resolved: resolvedToday.Count
        );
    }

    public async Task<List<AnalystWorkload>> GetWorkloadAsync()
    {
        var workload = await _db.Incidents
            .Where(i => i.Status != IncidentStatus.Closed && i.AssignedTo != null)
            .GroupBy(i => i.AssignedTo!)
            .Select(g => new { Analyst = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

        return workload.Select(w => new AnalystWorkload(w.Analyst, w.Count)).ToList();
    }

    public async Task<List<DailyIncidentCount>> GetTimelineAsync(int days = 14)
    {
        var now = DateTime.UtcNow;
        var startDate = now.AddDays(-days).Date;

        var incidents = await _db.Incidents
            .Where(i => i.DetectedAt >= startDate)
            .Select(i => i.DetectedAt)
            .ToListAsync();

        var result = new List<DailyIncidentCount>();
        for (int i = days - 1; i >= 0; i--)
        {
            var date = now.AddDays(-i).Date;
            var count = incidents.Count(d => d.Date == date);
            result.Add(new DailyIncidentCount(date.ToString("MMM dd"), count));
        }

        return result;
    }
}
