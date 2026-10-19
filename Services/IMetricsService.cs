namespace IncidentResponse.Services;

public record DashboardSummary(int TotalActive, int Critical, int High, int Medium, int AvgMttrMinutes, int SlaBreaches, int Resolved);
public record AnalystWorkload(string Analyst, int OpenCount);
public record DailyIncidentCount(string Date, int Count);

public interface IMetricsService
{
    Task<DashboardSummary> GetSummaryAsync();
    Task<List<AnalystWorkload>> GetWorkloadAsync();
    Task<List<DailyIncidentCount>> GetTimelineAsync(int days = 14);
}
