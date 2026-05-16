namespace CommandCenter.Domain.Models;

/// <summary>
/// Join entity between a Project and a TeamMember — supports many-to-many with a project role.
/// </summary>
public class ProjectMember
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    public int TeamMemberId { get; set; }
    public TeamMember? TeamMember { get; set; }

    /// <summary>Role on this specific project: Lead, Member, Stakeholder, Observer.</summary>
    public string ProjectRole { get; set; } = "Member";
}
