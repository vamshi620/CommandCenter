using CommandCenter.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandCenter.Data;

public sealed class CommandCenterDbContext : DbContext
{
    public CommandCenterDbContext(DbContextOptions<CommandCenterDbContext> options) : base(options) { }

    // Daily quadrant
    public DbSet<DeepWorkTask>   DeepWorkTasks   => Set<DeepWorkTask>();
    public DbSet<TeamSyncItem>   TeamSyncItems   => Set<TeamSyncItem>();
    public DbSet<MentorshipTask> MentorshipTasks => Set<MentorshipTask>();
    public DbSet<AdHocRequest>   AdHocRequests   => Set<AdHocRequest>();

    // Project management
    public DbSet<Project>         Projects         => Set<Project>();
    public DbSet<ProjectWorkItem> ProjectWorkItems => Set<ProjectWorkItem>();
    public DbSet<ProjectMember>   ProjectMembers   => Set<ProjectMember>();
    public DbSet<TeamMember>      TeamMembers      => Set<TeamMember>();
    public DbSet<WorkItemComment> WorkItemComments => Set<WorkItemComment>();

    // Settings
    public DbSet<AppSetting>      AppSettings      => Set<AppSetting>();
    public DbSet<Scratchpad>      Scratchpads      => Set<Scratchpad>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<AppSetting>(e => {
            e.HasKey(x => x.Key);
        });
        m.Entity<Scratchpad>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
        });
        m.Entity<DeepWorkTask>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Status).HasConversion<string>();
        });

        m.Entity<TeamSyncItem>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.TeammateName).IsRequired().HasMaxLength(100);
            e.Property(x => x.WorkItemId).HasMaxLength(100);
            e.Property(x => x.WorkItemUrl).HasMaxLength(500);
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.BlockerStatus).HasConversion<string>();
        });

        m.Entity<MentorshipTask>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.AssignedJuniorName).IsRequired().HasMaxLength(100);
            e.Property(x => x.ExpectedSopStandard).IsRequired().HasMaxLength(500);
            e.Property(x => x.Status).HasConversion<string>();
        });

        m.Entity<AdHocRequest>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.RequestorName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Status).HasConversion<string>();
        });

        m.Entity<TeamMember>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.Role).IsRequired().HasMaxLength(100);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.Department).HasMaxLength(100);
        });

        m.Entity<Project>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.Priority).HasConversion<string>();
            e.HasMany(x => x.WorkItems).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Members).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        });

        m.Entity<ProjectWorkItem>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.AssignedTo).HasMaxLength(100);
            e.Property(x => x.ItemType).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.Priority).HasConversion<string>();
            e.HasOne(x => x.AssignedTeamMember).WithMany(x => x.AssignedWorkItems)
             .HasForeignKey(x => x.AssignedTeamMemberId).OnDelete(DeleteBehavior.SetNull);
            e.HasMany(x => x.Comments).WithOne(x => x.WorkItem)
             .HasForeignKey(x => x.WorkItemId).OnDelete(DeleteBehavior.Cascade);
        });

        m.Entity<WorkItemComment>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Content).IsRequired();
            e.Property(x => x.Author).HasMaxLength(100);
        });

        m.Entity<ProjectMember>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.ProjectRole).HasMaxLength(50);
            e.HasOne(x => x.TeamMember).WithMany(x => x.ProjectAssignments)
             .HasForeignKey(x => x.TeamMemberId).OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(m);
    }
}
