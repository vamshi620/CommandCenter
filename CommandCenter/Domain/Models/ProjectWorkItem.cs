using CommandCenter.Domain.Enums;

namespace CommandCenter.Domain.Models;

/// <summary>
/// A tracked item within a project: an issue, delay, pending task, risk, or milestone.
/// </summary>
public class ProjectWorkItem
{
    public int Id { get; set; }

    /// <summary>Foreign key to the parent project.</summary>
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    /// <summary>Categorises this item: Issue, Delay, PendingItem, Risk, or Milestone.</summary>
    public ProjectItemType ItemType { get; set; } = ProjectItemType.Issue;

    /// <summary>Lifecycle status of this item (reuses the shared WorkItemStatus enum).</summary>
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Pending;

    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;

    /// <summary>Person or team responsible for resolving this item.</summary>
    public string? AssignedTo { get; set; }

    /// <summary>FK to TeamMember — preferred over the free-text AssignedTo field.</summary>
    public int? AssignedTeamMemberId { get; set; }
    public TeamMember? AssignedTeamMember { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }

    /// <summary>Navigation property — all comments on this work item.</summary>
    public ICollection<WorkItemComment> Comments { get; set; } = [];
}
