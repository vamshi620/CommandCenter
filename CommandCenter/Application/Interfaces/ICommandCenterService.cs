using CommandCenter.Domain.Enums;
using CommandCenter.Domain.Models;

namespace CommandCenter.Application.Interfaces;

public interface ICommandCenterService
{
    // ── Daily quadrant ────────────────────────────────────────────────────────
    Task<IReadOnlyList<DeepWorkTask>>   GetDeepWorkTasksAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<DeepWorkTask>  AddDeepWorkTaskAsync(DeepWorkTask task, CancellationToken ct = default);
    Task UpdateDeepWorkTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteDeepWorkTaskAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<TeamSyncItem>>   GetTeamSyncItemsAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<TeamSyncItem>  AddTeamSyncItemAsync(TeamSyncItem item, CancellationToken ct = default);
    Task UpdateTeamSyncItemStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteTeamSyncItemAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<MentorshipTask>> GetMentorshipTasksAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<MentorshipTask> AddMentorshipTaskAsync(MentorshipTask task, CancellationToken ct = default);
    Task UpdateMentorshipTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteMentorshipTaskAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<AdHocRequest>>   GetAdHocRequestsAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<AdHocRequest>  AddAdHocRequestAsync(AdHocRequest request, CancellationToken ct = default);
    Task UpdateAdHocRequestStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteAdHocRequestAsync(int id, CancellationToken ct = default);

    Task RolloverIncompleteTasksAsync(CancellationToken ct = default);

    // ── App Settings ──────────────────────────────────────────────────────────
    Task<string> GetAppSettingAsync(string key, string defaultValue = "", CancellationToken ct = default);
    Task SetAppSettingAsync(string key, string value, CancellationToken ct = default);

    // ── Team Members ──────────────────────────────────────────────────────────
    Task<IReadOnlyList<TeamMember>> GetTeamMembersAsync(bool activeOnly = true, CancellationToken ct = default);
    Task<TeamMember> AddTeamMemberAsync(TeamMember member, CancellationToken ct = default);
    Task UpdateTeamMemberAsync(TeamMember member, CancellationToken ct = default);
    Task DeleteTeamMemberAsync(int id, CancellationToken ct = default);

    // ── Projects ──────────────────────────────────────────────────────────────
    Task<IReadOnlyList<Project>> GetProjectsAsync(CancellationToken ct = default);
    Task<Project?> GetProjectDetailAsync(int id, CancellationToken ct = default);
    Task<Project> AddProjectAsync(Project project, CancellationToken ct = default);
    Task UpdateProjectStatusAsync(int id, ProjectStatus status, CancellationToken ct = default);
    Task UpdateProjectNotesAsync(int id, string? notes, CancellationToken ct = default);
    Task DeleteProjectAsync(int id, CancellationToken ct = default);

    // ── Project Members ───────────────────────────────────────────────────────
    Task AddProjectMemberAsync(int projectId, int teamMemberId, string role, CancellationToken ct = default);
    Task RemoveProjectMemberAsync(int projectMemberId, CancellationToken ct = default);

    // ── Project Work Items ────────────────────────────────────────────────────
    Task<IReadOnlyList<ProjectWorkItem>> GetProjectWorkItemsAsync(int projectId, ProjectItemType? typeFilter = null, CancellationToken ct = default);
    Task<ProjectWorkItem?> GetProjectWorkItemDetailAsync(int id, CancellationToken ct = default);
    Task<ProjectWorkItem> AddProjectWorkItemAsync(ProjectWorkItem item, CancellationToken ct = default);
    Task UpdateProjectWorkItemAsync(int id, WorkItemStatus status, int? assignedTeamMemberId, CancellationToken ct = default);
    Task UpdateProjectWorkItemDetailsAsync(ProjectWorkItem item, CancellationToken ct = default);
    Task DeleteProjectWorkItemAsync(int id, CancellationToken ct = default);
    Task<WorkItemComment> AddWorkItemCommentAsync(WorkItemComment comment, CancellationToken ct = default);
}
