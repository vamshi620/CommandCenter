using CommandCenter.Domain.Enums;

namespace CommandCenter.Domain.Models;

/// <summary>
/// Represents an assignment delegated to a junior developer with an expected SOP standard.
/// </summary>
public class MentorshipTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Pending;

    /// <summary>Full name of the junior developer assigned to this task.</summary>
    public string AssignedJuniorName { get; set; } = string.Empty;

    /// <summary>The Standard Operating Procedure quality level expected on delivery.</summary>
    public string ExpectedSopStandard { get; set; } = string.Empty;
}
