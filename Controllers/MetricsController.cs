using IncidentResponse.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncidentResponse.Controllers;

[ApiController]
[Route("api/metrics")]
[Authorize]
public class MetricsController : ControllerBase
{
    private readonly IMetricsService _metricsService;

    public MetricsController(IMetricsService metricsService)
    {
        _metricsService = metricsService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
    {
        var summary = await _metricsService.GetSummaryAsync();
        return Ok(new
        {
            totalActive = summary.TotalActive,
            critical = summary.Critical,
            high = summary.High,
            medium = summary.Medium,
            avgMttr = summary.AvgMttrMinutes,
            slaBreaches = summary.SlaBreaches,
            resolved = summary.Resolved
        });
    }

    [HttpGet("workload")]
    public async Task<IActionResult> Workload()
    {
        var workload = await _metricsService.GetWorkloadAsync();
        return Ok(workload.Select(w => new { analyst = w.Analyst, openCount = w.OpenCount }));
    }

    [HttpGet("timeline")]
    public async Task<IActionResult> Timeline([FromQuery] int days = 14)
    {
        var timeline = await _metricsService.GetTimelineAsync(days);
        return Ok(timeline.Select(t => new { date = t.Date, count = t.Count }));
    }
}
