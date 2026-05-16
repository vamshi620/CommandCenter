namespace CommandCenter.Domain.Models;

/// <summary>
/// Represents a team member managed by the project manager.
/// </summary>
public class TeamMember
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;       // e.g. Developer, QA, Designer
    public string? Email { get; set; }
    public string? Department { get; set; }

    /// <summary>Index 0–7 into the avatar color palette (assigned on creation).</summary>
    public int AvatarColorIndex { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

    public ICollection<ProjectMember> ProjectAssignments { get; set; } = [];
    public ICollection<ProjectWorkItem> AssignedWorkItems { get; set; } = [];
}
