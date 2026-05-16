using CommandCenter.Domain.Enums;
using CommandCenter.Domain.Models;

namespace CommandCenter.Application.Interfaces;

/// <summary>
/// Defines the contract for all dashboard data operations.
/// Follows the Interface Segregation and Dependency Inversion principles.
/// </summary>
public interface ICommandCenterService
{
    // --- DeepWorkTask ---
    Task<IReadOnlyList<DeepWorkTask>> GetDeepWorkTasksAsync(CancellationToken ct = default);
    Task<DeepWorkTask> AddDeepWorkTaskAsync(DeepWorkTask task, CancellationToken ct = default);
    Task UpdateDeepWorkTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteDeepWorkTaskAsync(int id, CancellationToken ct = default);

    // --- TeamSyncItem ---
    Task<IReadOnlyList<TeamSyncItem>> GetTeamSyncItemsAsync(CancellationToken ct = default);
    Task<TeamSyncItem> AddTeamSyncItemAsync(TeamSyncItem item, CancellationToken ct = default);
    Task UpdateTeamSyncItemStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteTeamSyncItemAsync(int id, CancellationToken ct = default);

    // --- MentorshipTask ---
    Task<IReadOnlyList<MentorshipTask>> GetMentorshipTasksAsync(CancellationToken ct = default);
    Task<MentorshipTask> AddMentorshipTaskAsync(MentorshipTask task, CancellationToken ct = default);
    Task UpdateMentorshipTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteMentorshipTaskAsync(int id, CancellationToken ct = default);

    // --- AdHocRequest ---
    Task<IReadOnlyList<AdHocRequest>> GetAdHocRequestsAsync(CancellationToken ct = default);
    Task<AdHocRequest> AddAdHocRequestAsync(AdHocRequest request, CancellationToken ct = default);
    Task UpdateAdHocRequestStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteAdHocRequestAsync(int id, CancellationToken ct = default);

    // --- Rollover ---
    /// <summary>Marks all incomplete tasks from previous days as RolledOver.</summary>
    Task RolloverIncompleteTasksAsync(CancellationToken ct = default);
}
