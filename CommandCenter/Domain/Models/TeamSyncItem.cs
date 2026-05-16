using CommandCenter.Domain.Enums;

namespace CommandCenter.Domain.Models;

/// <summary>
/// Tracks cross-team dependencies and code-review items for up to two teammates.
/// </summary>
public class TeamSyncItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Pending;

    /// <summary>Name of the teammate this item is coordinated with.</summary>
    public string TeammateName { get; set; } = string.Empty;

    /// <summary>Current blocker state for this dependency.</summary>
    public BlockerStatus BlockerStatus { get; set; } = BlockerStatus.None;

    /// <summary>URL to the associated pull request, if any.</summary>
    public string? PullRequestUrl { get; set; }
}
