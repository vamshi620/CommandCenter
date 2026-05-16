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
    /// <param name="filterDate">If provided, returns only tasks created on that date. If null, returns all.</param>
    Task<IReadOnlyList<DeepWorkTask>> GetDeepWorkTasksAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<DeepWorkTask> AddDeepWorkTaskAsync(DeepWorkTask task, CancellationToken ct = default);
    Task UpdateDeepWorkTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteDeepWorkTaskAsync(int id, CancellationToken ct = default);

    // --- TeamSyncItem ---
    /// <param name="filterDate">If provided, returns only items created on that date. If null, returns all.</param>
    Task<IReadOnlyList<TeamSyncItem>> GetTeamSyncItemsAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<TeamSyncItem> AddTeamSyncItemAsync(TeamSyncItem item, CancellationToken ct = default);
    Task UpdateTeamSyncItemStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteTeamSyncItemAsync(int id, CancellationToken ct = default);

    // --- MentorshipTask ---
    /// <param name="filterDate">If provided, returns only tasks created on that date. If null, returns all.</param>
    Task<IReadOnlyList<MentorshipTask>> GetMentorshipTasksAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<MentorshipTask> AddMentorshipTaskAsync(MentorshipTask task, CancellationToken ct = default);
    Task UpdateMentorshipTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteMentorshipTaskAsync(int id, CancellationToken ct = default);

    // --- AdHocRequest ---
    /// <param name="filterDate">If provided, returns only requests created on that date. If null, returns all.</param>
    Task<IReadOnlyList<AdHocRequest>> GetAdHocRequestsAsync(DateOnly? filterDate = null, CancellationToken ct = default);
    Task<AdHocRequest> AddAdHocRequestAsync(AdHocRequest request, CancellationToken ct = default);
    Task UpdateAdHocRequestStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default);
    Task DeleteAdHocRequestAsync(int id, CancellationToken ct = default);

    // --- Rollover ---
    /// <summary>Marks all incomplete tasks from previous days as RolledOver.</summary>
    Task RolloverIncompleteTasksAsync(CancellationToken ct = default);
}
