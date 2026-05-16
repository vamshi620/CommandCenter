using CommandCenter.Domain.Enums;

namespace CommandCenter.Domain.Models;

/// <summary>
/// Represents a project being actively worked on — the top-level unit for project tracking.
/// </summary>
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? TargetDate { get; set; }

    /// <summary>Navigation property — all tracked items under this project.</summary>
    public ICollection<ProjectWorkItem> WorkItems { get; set; } = [];
}
