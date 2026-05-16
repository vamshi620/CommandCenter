namespace CommandCenter.Domain.Models;

/// <summary>A comment thread entry on a project work item.</summary>
public class WorkItemComment
{
    public int Id { get; set; }

    /// <summary>FK to the parent work item.</summary>
    public int WorkItemId { get; set; }
    public ProjectWorkItem? WorkItem { get; set; }

    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = "PM";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
