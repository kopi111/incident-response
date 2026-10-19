using IncidentResponse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponse.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<IncidentNote> IncidentNotes => Set<IncidentNote>();
    public DbSet<AffectedAsset> AffectedAssets => Set<AffectedAsset>();
    public DbSet<IndicatorOfCompromise> IOCs => Set<IndicatorOfCompromise>();
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<CaseIncident> CaseIncidents => Set<CaseIncident>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Username).IsUnique();
        });

        modelBuilder.Entity<Incident>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.IncidentNumber).IsUnique();
            e.HasIndex(x => x.Status);
            e.HasIndex(x => x.Severity);
            e.HasIndex(x => x.AssignedTo);
            e.HasMany(x => x.Notes).WithOne(x => x.Incident).HasForeignKey(x => x.IncidentId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.AffectedAssets).WithOne(x => x.Incident).HasForeignKey(x => x.IncidentId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.IOCs).WithOne(x => x.Incident).HasForeignKey(x => x.IncidentId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<IncidentNote>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.IncidentId);
        });

        modelBuilder.Entity<AffectedAsset>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.IncidentId);
        });

        modelBuilder.Entity<IndicatorOfCompromise>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.IncidentId);
        });

        modelBuilder.Entity<CaseIncident>().HasKey(ci => new { ci.CaseId, ci.IncidentId });

        modelBuilder.Entity<Case>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.CaseNumber).IsUnique();
            e.HasMany(x => x.CaseIncidents).WithOne(x => x.Case).HasForeignKey(x => x.CaseId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Attachment>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.IncidentId);
        });

        modelBuilder.Entity<Alert>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Status);
        });

        modelBuilder.Entity<Incident>(e =>
        {
            e.HasMany(x => x.Attachments).WithOne(x => x.Incident).HasForeignKey(x => x.IncidentId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
