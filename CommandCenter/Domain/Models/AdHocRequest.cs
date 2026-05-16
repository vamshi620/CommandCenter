using CommandCenter.Domain.Enums;

namespace CommandCenter.Domain.Models;

/// <summary>
/// Represents an unplanned estimation or guidance request that arrives ad hoc.
/// </summary>
public class AdHocRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Pending;

    /// <summary>Name of the person or team who raised this request.</summary>
    public string RequestorName { get; set; } = string.Empty;

    /// <summary>Maximum time in minutes allocated to handle this request.</summary>
    public int TimeboxMinutes { get; set; }

    /// <summary>Hard deadline by which this request must be addressed.</summary>
    public DateTime? Deadline { get; set; }
}
