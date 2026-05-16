using CommandCenter.Application.Interfaces;
using CommandCenter.Data;
using CommandCenter.Domain.Enums;
using CommandCenter.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandCenter.Application.Services;

/// <summary>
/// Concrete implementation of <see cref="ICommandCenterService"/>.
/// Uses a scoped DbContext via DI; all methods are async-first.
/// </summary>
public sealed class CommandCenterService : ICommandCenterService
{
    private readonly CommandCenterDbContext _db;

    public CommandCenterService(CommandCenterDbContext db)
    {
        _db = db;
    }

    // ── DeepWorkTask ────────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<DeepWorkTask>> GetDeepWorkTasksAsync(CancellationToken ct = default)
        => await _db.DeepWorkTasks.OrderByDescending(x => x.CreatedDate).ToListAsync(ct);

    public async Task<DeepWorkTask> AddDeepWorkTaskAsync(DeepWorkTask task, CancellationToken ct = default)
    {
        task.CreatedDate = DateTime.UtcNow;
        _db.DeepWorkTasks.Add(task);
        await _db.SaveChangesAsync(ct);
        return task;
    }

    public async Task UpdateDeepWorkTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    {
        var task = await _db.DeepWorkTasks.FindAsync([id], ct)
                   ?? throw new KeyNotFoundException($"DeepWorkTask {id} not found.");
        task.Status = status;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteDeepWorkTaskAsync(int id, CancellationToken ct = default)
    {
        var task = await _db.DeepWorkTasks.FindAsync([id], ct)
                   ?? throw new KeyNotFoundException($"DeepWorkTask {id} not found.");
        _db.DeepWorkTasks.Remove(task);
        await _db.SaveChangesAsync(ct);
    }

    // ── TeamSyncItem ────────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<TeamSyncItem>> GetTeamSyncItemsAsync(CancellationToken ct = default)
        => await _db.TeamSyncItems.OrderByDescending(x => x.CreatedDate).ToListAsync(ct);

    public async Task<TeamSyncItem> AddTeamSyncItemAsync(TeamSyncItem item, CancellationToken ct = default)
    {
        item.CreatedDate = DateTime.UtcNow;
        _db.TeamSyncItems.Add(item);
        await _db.SaveChangesAsync(ct);
        return item;
    }

    public async Task UpdateTeamSyncItemStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    {
        var item = await _db.TeamSyncItems.FindAsync([id], ct)
                   ?? throw new KeyNotFoundException($"TeamSyncItem {id} not found.");
        item.Status = status;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteTeamSyncItemAsync(int id, CancellationToken ct = default)
    {
        var item = await _db.TeamSyncItems.FindAsync([id], ct)
                   ?? throw new KeyNotFoundException($"TeamSyncItem {id} not found.");
        _db.TeamSyncItems.Remove(item);
        await _db.SaveChangesAsync(ct);
    }

    // ── MentorshipTask ──────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<MentorshipTask>> GetMentorshipTasksAsync(CancellationToken ct = default)
        => await _db.MentorshipTasks.OrderByDescending(x => x.CreatedDate).ToListAsync(ct);

    public async Task<MentorshipTask> AddMentorshipTaskAsync(MentorshipTask task, CancellationToken ct = default)
    {
        task.CreatedDate = DateTime.UtcNow;
        _db.MentorshipTasks.Add(task);
        await _db.SaveChangesAsync(ct);
        return task;
    }

    public async Task UpdateMentorshipTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    {
        var task = await _db.MentorshipTasks.FindAsync([id], ct)
                   ?? throw new KeyNotFoundException($"MentorshipTask {id} not found.");
        task.Status = status;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteMentorshipTaskAsync(int id, CancellationToken ct = default)
    {
        var task = await _db.MentorshipTasks.FindAsync([id], ct)
                   ?? throw new KeyNotFoundException($"MentorshipTask {id} not found.");
        _db.MentorshipTasks.Remove(task);
        await _db.SaveChangesAsync(ct);
    }

    // ── AdHocRequest ────────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<AdHocRequest>> GetAdHocRequestsAsync(CancellationToken ct = default)
        => await _db.AdHocRequests.OrderByDescending(x => x.CreatedDate).ToListAsync(ct);

    public async Task<AdHocRequest> AddAdHocRequestAsync(AdHocRequest request, CancellationToken ct = default)
    {
        request.CreatedDate = DateTime.UtcNow;
        _db.AdHocRequests.Add(request);
        await _db.SaveChangesAsync(ct);
        return request;
    }

    public async Task UpdateAdHocRequestStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    {
        var req = await _db.AdHocRequests.FindAsync([id], ct)
                  ?? throw new KeyNotFoundException($"AdHocRequest {id} not found.");
        req.Status = status;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAdHocRequestAsync(int id, CancellationToken ct = default)
    {
        var req = await _db.AdHocRequests.FindAsync([id], ct)
                  ?? throw new KeyNotFoundException($"AdHocRequest {id} not found.");
        _db.AdHocRequests.Remove(req);
        await _db.SaveChangesAsync(ct);
    }

    // ── Rollover ────────────────────────────────────────────────────────────────
    public async Task RolloverIncompleteTasksAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;

        var incompleteStatuses = new[]
        {
            WorkItemStatus.Pending,
            WorkItemStatus.InProgress,
            WorkItemStatus.Blocked
        };

        var dwTasks = await _db.DeepWorkTasks
            .Where(x => incompleteStatuses.Contains(x.Status) && x.CreatedDate.Date < today)
            .ToListAsync(ct);
        dwTasks.ForEach(x => x.Status = WorkItemStatus.RolledOver);

        var tsItems = await _db.TeamSyncItems
            .Where(x => incompleteStatuses.Contains(x.Status) && x.CreatedDate.Date < today)
            .ToListAsync(ct);
        tsItems.ForEach(x => x.Status = WorkItemStatus.RolledOver);

        var mTasks = await _db.MentorshipTasks
            .Where(x => incompleteStatuses.Contains(x.Status) && x.CreatedDate.Date < today)
            .ToListAsync(ct);
        mTasks.ForEach(x => x.Status = WorkItemStatus.RolledOver);

        var ahReqs = await _db.AdHocRequests
            .Where(x => incompleteStatuses.Contains(x.Status) && x.CreatedDate.Date < today)
            .ToListAsync(ct);
        ahReqs.ForEach(x => x.Status = WorkItemStatus.RolledOver);

        await _db.SaveChangesAsync(ct);
    }
}
