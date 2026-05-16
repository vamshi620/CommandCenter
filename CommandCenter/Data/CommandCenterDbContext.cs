using CommandCenter.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandCenter.Data;

/// <summary>
/// EF Core DbContext for the CommandCenter application.
/// </summary>
public sealed class CommandCenterDbContext : DbContext
{
    public CommandCenterDbContext(DbContextOptions<CommandCenterDbContext> options)
        : base(options) { }

    // ── Daily quadrant entities ────────────────────────────────────────────────
    public DbSet<DeepWorkTask> DeepWorkTasks => Set<DeepWorkTask>();
    public DbSet<TeamSyncItem> TeamSyncItems => Set<TeamSyncItem>();
    public DbSet<MentorshipTask> MentorshipTasks => Set<MentorshipTask>();
    public DbSet<AdHocRequest> AdHocRequests => Set<AdHocRequest>();

    // ── Project tracking entities ─────────────────────────────────────────────
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectWorkItem> ProjectWorkItems => Set<ProjectWorkItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeepWorkTask>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.Status).HasConversion<string>();
        });

        modelBuilder.Entity<TeamSyncItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.TeammateName).IsRequired().HasMaxLength(100);
            e.Property(x => x.WorkItemId).HasMaxLength(100);
            e.Property(x => x.WorkItemUrl).HasMaxLength(500);
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.BlockerStatus).HasConversion<string>();
        });

        modelBuilder.Entity<MentorshipTask>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.AssignedJuniorName).IsRequired().HasMaxLength(100);
            e.Property(x => x.ExpectedSopStandard).IsRequired().HasMaxLength(500);
            e.Property(x => x.Status).HasConversion<string>();
        });

        modelBuilder.Entity<AdHocRequest>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.RequestorName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Status).HasConversion<string>();
        });

        modelBuilder.Entity<Project>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.Priority).HasConversion<string>();
            e.HasMany(x => x.WorkItems)
             .WithOne(x => x.Project)
             .HasForeignKey(x => x.ProjectId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProjectWorkItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.AssignedTo).HasMaxLength(100);
            e.Property(x => x.ItemType).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.Priority).HasConversion<string>();
        });

        base.OnModelCreating(modelBuilder);
    }
}
