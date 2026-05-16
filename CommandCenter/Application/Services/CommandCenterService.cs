using CommandCenter.Application.Interfaces;
using CommandCenter.Data;
using CommandCenter.Domain.Enums;
using CommandCenter.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandCenter.Application.Services;

/// <summary>
/// Concrete implementation of <see cref="ICommandCenterService"/>.
/// </summary>
public sealed class CommandCenterService : ICommandCenterService
{
    private readonly CommandCenterDbContext _db;
    public CommandCenterService(CommandCenterDbContext db) => _db = db;

    // ── DeepWorkTask ─────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<DeepWorkTask>> GetDeepWorkTasksAsync(DateOnly? filterDate = null, CancellationToken ct = default)
    {
        var q = _db.DeepWorkTasks.AsQueryable();
        if (filterDate.HasValue) { var d = filterDate.Value.ToDateTime(TimeOnly.MinValue); q = q.Where(x => x.CreatedDate.Date == d.Date); }
        return await q.OrderByDescending(x => x.CreatedDate).ToListAsync(ct);
    }
    public async Task<DeepWorkTask> AddDeepWorkTaskAsync(DeepWorkTask task, CancellationToken ct = default)
    { task.CreatedDate = DateTime.UtcNow; _db.DeepWorkTasks.Add(task); await _db.SaveChangesAsync(ct); return task; }
    public async Task UpdateDeepWorkTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    { var e = await _db.DeepWorkTasks.FindAsync([id], ct) ?? throw new KeyNotFoundException(); e.Status = status; await _db.SaveChangesAsync(ct); }
    public async Task DeleteDeepWorkTaskAsync(int id, CancellationToken ct = default)
    { var e = await _db.DeepWorkTasks.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.DeepWorkTasks.Remove(e); await _db.SaveChangesAsync(ct); }

    // ── TeamSyncItem ─────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<TeamSyncItem>> GetTeamSyncItemsAsync(DateOnly? filterDate = null, CancellationToken ct = default)
    {
        var q = _db.TeamSyncItems.AsQueryable();
        if (filterDate.HasValue) { var d = filterDate.Value.ToDateTime(TimeOnly.MinValue); q = q.Where(x => x.CreatedDate.Date == d.Date); }
        return await q.OrderByDescending(x => x.CreatedDate).ToListAsync(ct);
    }
    public async Task<TeamSyncItem> AddTeamSyncItemAsync(TeamSyncItem item, CancellationToken ct = default)
    { item.CreatedDate = DateTime.UtcNow; _db.TeamSyncItems.Add(item); await _db.SaveChangesAsync(ct); return item; }
    public async Task UpdateTeamSyncItemStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    { var e = await _db.TeamSyncItems.FindAsync([id], ct) ?? throw new KeyNotFoundException(); e.Status = status; await _db.SaveChangesAsync(ct); }
    public async Task DeleteTeamSyncItemAsync(int id, CancellationToken ct = default)
    { var e = await _db.TeamSyncItems.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.TeamSyncItems.Remove(e); await _db.SaveChangesAsync(ct); }

    // ── MentorshipTask ───────────────────────────────────────────────────────
    public async Task<IReadOnlyList<MentorshipTask>> GetMentorshipTasksAsync(DateOnly? filterDate = null, CancellationToken ct = default)
    {
        var q = _db.MentorshipTasks.AsQueryable();
        if (filterDate.HasValue) { var d = filterDate.Value.ToDateTime(TimeOnly.MinValue); q = q.Where(x => x.CreatedDate.Date == d.Date); }
        return await q.OrderByDescending(x => x.CreatedDate).ToListAsync(ct);
    }
    public async Task<MentorshipTask> AddMentorshipTaskAsync(MentorshipTask task, CancellationToken ct = default)
    { task.CreatedDate = DateTime.UtcNow; _db.MentorshipTasks.Add(task); await _db.SaveChangesAsync(ct); return task; }
    public async Task UpdateMentorshipTaskStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    { var e = await _db.MentorshipTasks.FindAsync([id], ct) ?? throw new KeyNotFoundException(); e.Status = status; await _db.SaveChangesAsync(ct); }
    public async Task DeleteMentorshipTaskAsync(int id, CancellationToken ct = default)
    { var e = await _db.MentorshipTasks.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.MentorshipTasks.Remove(e); await _db.SaveChangesAsync(ct); }

    // ── AdHocRequest ─────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<AdHocRequest>> GetAdHocRequestsAsync(DateOnly? filterDate = null, CancellationToken ct = default)
    {
        var q = _db.AdHocRequests.AsQueryable();
        if (filterDate.HasValue) { var d = filterDate.Value.ToDateTime(TimeOnly.MinValue); q = q.Where(x => x.CreatedDate.Date == d.Date); }
        return await q.OrderByDescending(x => x.CreatedDate).ToListAsync(ct);
    }
    public async Task<AdHocRequest> AddAdHocRequestAsync(AdHocRequest request, CancellationToken ct = default)
    { request.CreatedDate = DateTime.UtcNow; _db.AdHocRequests.Add(request); await _db.SaveChangesAsync(ct); return request; }
    public async Task UpdateAdHocRequestStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    { var e = await _db.AdHocRequests.FindAsync([id], ct) ?? throw new KeyNotFoundException(); e.Status = status; await _db.SaveChangesAsync(ct); }
    public async Task DeleteAdHocRequestAsync(int id, CancellationToken ct = default)
    { var e = await _db.AdHocRequests.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.AdHocRequests.Remove(e); await _db.SaveChangesAsync(ct); }

    // ── Rollover ─────────────────────────────────────────────────────────────
    public async Task RolloverIncompleteTasksAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        var incomplete = new[] { WorkItemStatus.Pending, WorkItemStatus.InProgress, WorkItemStatus.Blocked };

        (await _db.DeepWorkTasks.Where(x => incomplete.Contains(x.Status) && x.CreatedDate.Date < today).ToListAsync(ct))
            .ForEach(x => x.Status = WorkItemStatus.RolledOver);
        (await _db.TeamSyncItems.Where(x => incomplete.Contains(x.Status) && x.CreatedDate.Date < today).ToListAsync(ct))
            .ForEach(x => x.Status = WorkItemStatus.RolledOver);
        (await _db.MentorshipTasks.Where(x => incomplete.Contains(x.Status) && x.CreatedDate.Date < today).ToListAsync(ct))
            .ForEach(x => x.Status = WorkItemStatus.RolledOver);
        (await _db.AdHocRequests.Where(x => incomplete.Contains(x.Status) && x.CreatedDate.Date < today).ToListAsync(ct))
            .ForEach(x => x.Status = WorkItemStatus.RolledOver);

        await _db.SaveChangesAsync(ct);
    }

    // ── Projects ──────────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<Project>> GetProjectsAsync(CancellationToken ct = default)
        => await _db.Projects
            .Include(p => p.WorkItems)
            .OrderBy(p => p.Priority)
            .ThenBy(p => p.Name)
            .ToListAsync(ct);

    public async Task<Project> AddProjectAsync(Project project, CancellationToken ct = default)
    {
        project.CreatedDate = DateTime.UtcNow;
        _db.Projects.Add(project);
        await _db.SaveChangesAsync(ct);
        return project;
    }

    public async Task UpdateProjectStatusAsync(int id, ProjectStatus status, CancellationToken ct = default)
    {
        var p = await _db.Projects.FindAsync([id], ct) ?? throw new KeyNotFoundException($"Project {id} not found.");
        p.Status = status;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteProjectAsync(int id, CancellationToken ct = default)
    {
        var p = await _db.Projects.FindAsync([id], ct) ?? throw new KeyNotFoundException($"Project {id} not found.");
        _db.Projects.Remove(p);
        await _db.SaveChangesAsync(ct);
    }

    // ── Project Work Items ────────────────────────────────────────────────────
    public async Task<IReadOnlyList<ProjectWorkItem>> GetProjectWorkItemsAsync(
        int projectId, ProjectItemType? typeFilter = null, CancellationToken ct = default)
    {
        var q = _db.ProjectWorkItems.Where(x => x.ProjectId == projectId);
        if (typeFilter.HasValue) q = q.Where(x => x.ItemType == typeFilter.Value);
        return await q.OrderByDescending(x => x.Priority).ThenByDescending(x => x.CreatedDate).ToListAsync(ct);
    }

    public async Task<ProjectWorkItem> AddProjectWorkItemAsync(ProjectWorkItem item, CancellationToken ct = default)
    {
        item.CreatedDate = DateTime.UtcNow;
        _db.ProjectWorkItems.Add(item);
        await _db.SaveChangesAsync(ct);
        return item;
    }

    public async Task UpdateProjectWorkItemStatusAsync(int id, WorkItemStatus status, CancellationToken ct = default)
    {
        var wi = await _db.ProjectWorkItems.FindAsync([id], ct) ?? throw new KeyNotFoundException($"ProjectWorkItem {id} not found.");
        wi.Status = status;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteProjectWorkItemAsync(int id, CancellationToken ct = default)
    {
        var wi = await _db.ProjectWorkItems.FindAsync([id], ct) ?? throw new KeyNotFoundException($"ProjectWorkItem {id} not found.");
        _db.ProjectWorkItems.Remove(wi);
        await _db.SaveChangesAsync(ct);
    }
}
