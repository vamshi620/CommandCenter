using CommandCenter.Domain.Enums;

namespace CommandCenter.Domain.Models;

/// <summary>
/// Represents an individual developer deliverable requiring focused (deep) work.
/// </summary>
public class DeepWorkTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Pending;

    /// <summary>Estimated hours needed to complete this deliverable.</summary>
    public double TimeEstimatedHours { get; set; }
}
