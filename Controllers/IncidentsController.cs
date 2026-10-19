using IncidentResponse.Domain.Enums;
using IncidentResponse.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IncidentResponse.Controllers;

[ApiController]
[Route("api/incidents")]
[Authorize]
public class IncidentsController : ControllerBase
{
    private readonly IIncidentService _incidentService;

    public IncidentsController(IIncidentService incidentService)
    {
        _incidentService = incidentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? severity,
        [FromQuery] string? assignee)
    {
        IncidentStatus? statusEnum = null;
        Severity? severityEnum = null;

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<IncidentStatus>(status, true, out var s))
            statusEnum = s;

        if (!string.IsNullOrEmpty(severity) && Enum.TryParse<Severity>(severity, true, out var sev))
            severityEnum = sev;

        var incidents = await _incidentService.GetAllAsync(statusEnum, severityEnum, assignee);
        return Ok(incidents.Select(i => new
        {
            i.Id, i.IncidentNumber, i.Title, i.Severity, i.Status,
            i.AssignedTo, i.DetectedAt, i.UpdatedAt, i.CreatedBy,
            noteCount = i.Notes.Count,
            assetCount = i.AffectedAssets.Count,
            iocCount = i.IOCs.Count,
            mitreTactics = i.MitreTactics != null ? JsonSerializer.Deserialize<List<string>>(i.MitreTactics) : null
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var incident = await _incidentService.GetByIdAsync(id);
        if (incident == null) return NotFound();
        return Ok(incident);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIncidentDto dto)
    {
        var username = User.Identity?.Name ?? "system";
        var incident = await _incidentService.CreateAsync(dto, username);
        return CreatedAtAction(nameof(GetById), new { id = incident.Id }, incident);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIncidentDto dto)
    {
        var username = User.Identity?.Name ?? "system";
        try
        {
            var incident = await _incidentService.UpdateStatusAsync(id, dto.Status, username);
            return Ok(incident);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] UpdateStatusDto dto)
    {
        var username = User.Identity?.Name ?? "system";
        if (!Enum.TryParse<IncidentStatus>(dto.Status, true, out var newStatus))
            return BadRequest(new { message = "Invalid status value." });

        try
        {
            var incident = await _incidentService.UpdateStatusAsync(id, newStatus, username);
            return Ok(new { incident.Id, incident.Status, incident.UpdatedAt });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id:guid}/assign")]
    public async Task<IActionResult> Assign(Guid id, [FromBody] AssignDto dto)
    {
        var username = User.Identity?.Name ?? "system";
        try
        {
            var incident = await _incidentService.AssignAsync(id, dto.AssignTo, username);
            return Ok(new { incident.Id, incident.AssignedTo, incident.UpdatedAt });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:guid}/notes")]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] AddNoteDto dto)
    {
        var username = User.Identity?.Name ?? "system";
        try
        {
            var note = await _incidentService.AddNoteAsync(id, dto.Content, username);
            return Ok(note);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:guid}/assets/{assetId:guid}/isolate")]
    public async Task<IActionResult> IsolateAsset(Guid id, Guid assetId)
    {
        try
        {
            var asset = await _incidentService.IsolateAssetAsync(id, assetId);
            return Ok(asset);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("export-csv")]
    [AllowAnonymous]
    public async Task<IActionResult> ExportCsv()
    {
        var incidents = await _incidentService.GetAllAsync();
        var csv = new System.Text.StringBuilder();
        csv.AppendLine("Incident Number,Title,Severity,Status,Assigned To,Detected At,Resolved At");
        foreach (var i in incidents)
            csv.AppendLine($"{i.IncidentNumber},{i.Title},{i.Severity},{i.Status},{i.AssignedTo},{i.DetectedAt:u},{i.ResolvedAt?.ToString("u") ?? ""}");
        return File(System.Text.Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "incidents.csv");
    }

    [HttpGet("{id:guid}/export")]
    public async Task<IActionResult> Export(Guid id)
    {
        var incident = await _incidentService.GetByIdAsync(id);
        if (incident == null) return NotFound();

        var export = new
        {
            incident.IncidentNumber,
            incident.Title,
            incident.Description,
            Severity = incident.Severity.ToString(),
            Status = incident.Status.ToString(),
            incident.AssignedTo,
            incident.DetectedAt,
            incident.ContainedAt,
            incident.ResolvedAt,
            MitreTactics = incident.MitreTactics != null
                ? JsonSerializer.Deserialize<List<string>>(incident.MitreTactics)
                : null,
            Notes = incident.Notes.Select(n => new { n.Author, n.CreatedAt, n.Content }),
            AffectedAssets = incident.AffectedAssets.Select(a => new
                { a.Hostname, a.IpAddress, a.AssetType, a.AssetStatus, a.RiskScore }),
            IOCs = incident.IOCs.Select(i => new { i.IocType, i.Value, i.Description, i.AddedAt })
        };

        return Ok(export);
    }
}

public record UpdateIncidentDto(IncidentStatus Status);
public record UpdateStatusDto(string Status);
public record AssignDto(string AssignTo);
public record AddNoteDto(string Content);
