using CommandCenter.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandCenter.Data;

/// <summary>
/// EF Core DbContext for the CommandCenter application.
/// Provides typed DbSets for all four quadrant entities.
/// </summary>
public sealed class CommandCenterDbContext : DbContext
{
    public CommandCenterDbContext(DbContextOptions<CommandCenterDbContext> options)
        : base(options) { }

    public DbSet<DeepWorkTask> DeepWorkTasks => Set<DeepWorkTask>();
    public DbSet<TeamSyncItem> TeamSyncItems => Set<TeamSyncItem>();
    public DbSet<MentorshipTask> MentorshipTasks => Set<MentorshipTask>();
    public DbSet<AdHocRequest> AdHocRequests => Set<AdHocRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // DeepWorkTask
        modelBuilder.Entity<DeepWorkTask>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.Status).HasConversion<string>();
        });

        // TeamSyncItem
        modelBuilder.Entity<TeamSyncItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.TeammateName).IsRequired().HasMaxLength(100);
            e.Property(x => x.PullRequestUrl).HasMaxLength(500);
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.BlockerStatus).HasConversion<string>();
        });

        // MentorshipTask
        modelBuilder.Entity<MentorshipTask>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.AssignedJuniorName).IsRequired().HasMaxLength(100);
            e.Property(x => x.ExpectedSopStandard).IsRequired().HasMaxLength(500);
            e.Property(x => x.Status).HasConversion<string>();
        });

        // AdHocRequest
        modelBuilder.Entity<AdHocRequest>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.RequestorName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Status).HasConversion<string>();
        });

        base.OnModelCreating(modelBuilder);
    }
}
