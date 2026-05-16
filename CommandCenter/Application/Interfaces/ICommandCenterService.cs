using CommandCenter.Domain.Enums;
using CommandCenter.Domain.Models;

namespace CommandCenter.Application.Interfaces;

/// <summary>
/// Defines the contract for all dashboard data operations.
/// Follows ISP and DIP principles.
/// </summary>
public interface ICommandCenterService
{
    // ── DeepWorkTask ─────────────────────────────────────────────────────────
    Task<IReadOnlyList<DeepWorkTask>> GetDeepWorkTasksAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<DeepWorkTask> AddDeepWorkTaskAsync(DeepWorkTask task, CancellationToken ct = default);
    Task UpdateDeepWorkTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteDeepWorkTaskAsync(int id, CancellationToken ct = default);

    // ── TeamSyncItem ─────────────────────────────────────────────────────────
    Task<IReadOnlyList<TeamSyncItem>> GetTeamSyncItemsAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<TeamSyncItem> AddTeamSyncItemAsync(TeamSyncItem item, CancellationToken ct = default);
    Task UpdateTeamSyncItemStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteTeamSyncItemAsync(int id, CancellationToken ct = default);

    // ── MentorshipTask ───────────────────────────────────────────────────────
    Task<IReadOnlyList<MentorshipTask>> GetMentorshipTasksAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<MentorshipTask> AddMentorshipTaskAsync(MentorshipTask task, CancellationToken ct = default);
    Task UpdateMentorshipTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteMentorshipTaskAsync(int id, CancellationToken ct = default);

    // ── AdHocRequest ─────────────────────────────────────────────────────────
    Task<IReadOnlyList<AdHocRequest>> GetAdHocRequestsAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<AdHocRequest> AddAdHocRequestAsync(AdHocRequest request, CancellationToken ct = default);
    Task UpdateAdHocRequestStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteAdHocRequestAsync(int id, CancellationToken ct = default);

    // ── Rollover ─────────────────────────────────────────────────────────────
    Task RolloverIncompleteTasksAsync(CancellationToken ct = default);

    // ── Project tracking ─────────────────────────────────────────────────────
    Task<IReadOnlyList<Project>> GetProjectsAsync(CancellationToken ct = default);
    Task<Project> AddProjectAsync(Project project, CancellationToken ct = default);
    Task UpdateProjectStatusAsync(int id, ProjectStatus status, CancellationToken ct = default);
    Task DeleteProjectAsync(int id, CancellationToken ct = default);

    // ── Project Work Items ────────────────────────────────────────────────────
    Task<IReadOnlyList<ProjectWorkItem>> GetProjectWorkItemsAsync(int projectId, ProjectItemType? typeFilter = null, CancellationToken ct = default);
    Task<ProjectWorkItem> AddProjectWorkItemAsync(ProjectWorkItem item, CancellationToken ct = default);
    Task UpdateProjectWorkItemStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteProjectWorkItemAsync(int id, CancellationToken ct = default);
}
