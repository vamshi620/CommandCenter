using CommandCenter.Application.Interfaces;
using CommandCenter.Data;
using CommandCenter.Domain.Enums;
using CommandCenter.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandCenter.Application.Services;

public sealed class CommandCenterService : ICommandCenterService
{
    private readonly CommandCenterDbContext _db;
    public CommandCenterService(CommandCenterDbContext db) => _db = db;

    // ── Daily quadrant ────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<DeepWorkTask>> GetDeepWorkTasksAsync(DateOnly? d = null, CancellationToken ct = default)
    { var q = _db.DeepWorkTasks.AsQueryable(); if (d.HasValue) { var dt = d.Value.ToDateTime(TimeOnly.MinValue); q = q.Where(x => x.CreatedDate.Date == dt.Date); } return await q.OrderByDescending(x => x.CreatedDate).ToListAsync(ct); }
    public async Task<DeepWorkTask> AddDeepWorkTaskAsync(DeepWorkTask t, CancellationToken ct = default)
    { t.CreatedDate = DateTime.UtcNow; _db.DeepWorkTasks.Add(t); await _db.SaveChangesAsync(ct); return t; }
    public async Task UpdateDeepWorkTaskStatusAsync(int id, WorkItemStatus s, CancellationToken ct = default)
    { var e = await _db.DeepWorkTasks.FindAsync([id], ct) ?? throw new KeyNotFoundException(); e.Status = s; await _db.SaveChangesAsync(ct); }
    public async Task DeleteDeepWorkTaskAsync(int id, CancellationToken ct = default)
    { var e = await _db.DeepWorkTasks.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.DeepWorkTasks.Remove(e); await _db.SaveChangesAsync(ct); }

    public async Task<IReadOnlyList<TeamSyncItem>> GetTeamSyncItemsAsync(DateOnly? d = null, CancellationToken ct = default)
    { var q = _db.TeamSyncItems.AsQueryable(); if (d.HasValue) { var dt = d.Value.ToDateTime(TimeOnly.MinValue); q = q.Where(x => x.CreatedDate.Date == dt.Date); } return await q.OrderByDescending(x => x.CreatedDate).ToListAsync(ct); }
    public async Task<TeamSyncItem> AddTeamSyncItemAsync(TeamSyncItem t, CancellationToken ct = default)
    { t.CreatedDate = DateTime.UtcNow; _db.TeamSyncItems.Add(t); await _db.SaveChangesAsync(ct); return t; }
    public async Task UpdateTeamSyncItemStatusAsync(int id, WorkItemStatus s, CancellationToken ct = default)
    { var e = await _db.TeamSyncItems.FindAsync([id], ct) ?? throw new KeyNotFoundException(); e.Status = s; await _db.SaveChangesAsync(ct); }
    public async Task DeleteTeamSyncItemAsync(int id, CancellationToken ct = default)
    { var e = await _db.TeamSyncItems.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.TeamSyncItems.Remove(e); await _db.SaveChangesAsync(ct); }

    public async Task<IReadOnlyList<MentorshipTask>> GetMentorshipTasksAsync(DateOnly? d = null, CancellationToken ct = default)
    { var q = _db.MentorshipTasks.AsQueryable(); if (d.HasValue) { var dt = d.Value.ToDateTime(TimeOnly.MinValue); q = q.Where(x => x.CreatedDate.Date == dt.Date); } return await q.OrderByDescending(x => x.CreatedDate).ToListAsync(ct); }
    public async Task<MentorshipTask> AddMentorshipTaskAsync(MentorshipTask t, CancellationToken ct = default)
    { t.CreatedDate = DateTime.UtcNow; _db.MentorshipTasks.Add(t); await _db.SaveChangesAsync(ct); return t; }
    public async Task UpdateMentorshipTaskStatusAsync(int id, WorkItemStatus s, CancellationToken ct = default)
    { var e = await _db.MentorshipTasks.FindAsync([id], ct) ?? throw new KeyNotFoundException(); e.Status = s; await _db.SaveChangesAsync(ct); }
    public async Task DeleteMentorshipTaskAsync(int id, CancellationToken ct = default)
    { var e = await _db.MentorshipTasks.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.MentorshipTasks.Remove(e); await _db.SaveChangesAsync(ct); }

    public async Task<IReadOnlyList<AdHocRequest>> GetAdHocRequestsAsync(DateOnly? d = null, CancellationToken ct = default)
    { var q = _db.AdHocRequests.AsQueryable(); if (d.HasValue) { var dt = d.Value.ToDateTime(TimeOnly.MinValue); q = q.Where(x => x.CreatedDate.Date == dt.Date); } return await q.OrderByDescending(x => x.CreatedDate).ToListAsync(ct); }
    public async Task<AdHocRequest> AddAdHocRequestAsync(AdHocRequest r, CancellationToken ct = default)
    { r.CreatedDate = DateTime.UtcNow; _db.AdHocRequests.Add(r); await _db.SaveChangesAsync(ct); return r; }
    public async Task UpdateAdHocRequestStatusAsync(int id, WorkItemStatus s, CancellationToken ct = default)
    { var e = await _db.AdHocRequests.FindAsync([id], ct) ?? throw new KeyNotFoundException(); e.Status = s; await _db.SaveChangesAsync(ct); }
    public async Task DeleteAdHocRequestAsync(int id, CancellationToken ct = default)
    { var e = await _db.AdHocRequests.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.AdHocRequests.Remove(e); await _db.SaveChangesAsync(ct); }

    public async Task RolloverIncompleteTasksAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        var inc = new[] { WorkItemStatus.Pending, WorkItemStatus.InProgress, WorkItemStatus.Blocked };
        (await _db.DeepWorkTasks.Where(x => inc.Contains(x.Status) && x.CreatedDate.Date < today).ToListAsync(ct)).ForEach(x => x.Status = WorkItemStatus.RolledOver);
        (await _db.TeamSyncItems.Where(x => inc.Contains(x.Status) && x.CreatedDate.Date < today).ToListAsync(ct)).ForEach(x => x.Status = WorkItemStatus.RolledOver);
        (await _db.MentorshipTasks.Where(x => inc.Contains(x.Status) && x.CreatedDate.Date < today).ToListAsync(ct)).ForEach(x => x.Status = WorkItemStatus.RolledOver);
        (await _db.AdHocRequests.Where(x => inc.Contains(x.Status) && x.CreatedDate.Date < today).ToListAsync(ct)).ForEach(x => x.Status = WorkItemStatus.RolledOver);
        await _db.SaveChangesAsync(ct);
    }

    // ── Team Members ──────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<TeamMember>> GetTeamMembersAsync(bool activeOnly = true, CancellationToken ct = default)
    {
        var q = _db.TeamMembers.Include(m => m.ProjectAssignments).ThenInclude(pa => pa.Project)
                               .Include(m => m.AssignedWorkItems).AsQueryable();
        if (activeOnly) q = q.Where(m => m.IsActive);
        return await q.OrderBy(m => m.Name).ToListAsync(ct);
    }

    public async Task<TeamMember> AddTeamMemberAsync(TeamMember member, CancellationToken ct = default)
    {
        member.JoinedDate = DateTime.UtcNow;
        // Auto-assign color index based on count
        member.AvatarColorIndex = (await _db.TeamMembers.CountAsync(ct)) % 8;
        _db.TeamMembers.Add(member);
        await _db.SaveChangesAsync(ct);
        return member;
    }

    public async Task UpdateTeamMemberAsync(TeamMember member, CancellationToken ct = default)
    {
        _db.TeamMembers.Update(member);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteTeamMemberAsync(int id, CancellationToken ct = default)
    {
        var m = await _db.TeamMembers.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        _db.TeamMembers.Remove(m);
        await _db.SaveChangesAsync(ct);
    }

    // ── Projects ──────────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<Project>> GetProjectsAsync(CancellationToken ct = default)
        => await _db.Projects
            .Include(p => p.WorkItems).ThenInclude(wi => wi.AssignedTeamMember)
            .Include(p => p.Members).ThenInclude(pm => pm.TeamMember)
            .OrderBy(p => p.Priority).ThenBy(p => p.Name)
            .ToListAsync(ct);

    public async Task<Project?> GetProjectDetailAsync(int id, CancellationToken ct = default)
        => await _db.Projects
            .Include(p => p.WorkItems).ThenInclude(wi => wi.AssignedTeamMember)
            .Include(p => p.Members).ThenInclude(pm => pm.TeamMember)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Project> AddProjectAsync(Project project, CancellationToken ct = default)
    { project.CreatedDate = DateTime.UtcNow; _db.Projects.Add(project); await _db.SaveChangesAsync(ct); return project; }

    public async Task UpdateProjectStatusAsync(int id, ProjectStatus status, CancellationToken ct = default)
    { var p = await _db.Projects.FindAsync([id], ct) ?? throw new KeyNotFoundException(); p.Status = status; await _db.SaveChangesAsync(ct); }

    public async Task DeleteProjectAsync(int id, CancellationToken ct = default)
    { var p = await _db.Projects.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.Projects.Remove(p); await _db.SaveChangesAsync(ct); }

    // ── Project Members ───────────────────────────────────────────────────────
    public async Task AddProjectMemberAsync(int projectId, int teamMemberId, string role, CancellationToken ct = default)
    {
        var exists = await _db.ProjectMembers.AnyAsync(pm => pm.ProjectId == projectId && pm.TeamMemberId == teamMemberId, ct);
        if (exists) return;
        _db.ProjectMembers.Add(new ProjectMember { ProjectId = projectId, TeamMemberId = teamMemberId, ProjectRole = role });
        await _db.SaveChangesAsync(ct);
    }

    public async Task RemoveProjectMemberAsync(int projectMemberId, CancellationToken ct = default)
    { var pm = await _db.ProjectMembers.FindAsync([projectMemberId], ct) ?? throw new KeyNotFoundException(); _db.ProjectMembers.Remove(pm); await _db.SaveChangesAsync(ct); }

    // ── Project Work Items ────────────────────────────────────────────────────
    public async Task<IReadOnlyList<ProjectWorkItem>> GetProjectWorkItemsAsync(int projectId, ProjectItemType? typeFilter = null, CancellationToken ct = default)
    {
        var q = _db.ProjectWorkItems.Include(wi => wi.AssignedTeamMember).Where(x => x.ProjectId == projectId);
        if (typeFilter.HasValue) q = q.Where(x => x.ItemType == typeFilter.Value);
        return await q.OrderByDescending(x => x.Priority).ThenByDescending(x => x.CreatedDate).ToListAsync(ct);
    }

    public async Task<ProjectWorkItem> AddProjectWorkItemAsync(ProjectWorkItem item, CancellationToken ct = default)
    { item.CreatedDate = DateTime.UtcNow; _db.ProjectWorkItems.Add(item); await _db.SaveChangesAsync(ct); return item; }

    public async Task UpdateProjectWorkItemAsync(int id, WorkItemStatus status, int? assignedTeamMemberId, CancellationToken ct = default)
    {
        var wi = await _db.ProjectWorkItems.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        wi.Status = status;
        wi.AssignedTeamMemberId = assignedTeamMemberId;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteProjectWorkItemAsync(int id, CancellationToken ct = default)
    { var wi = await _db.ProjectWorkItems.FindAsync([id], ct) ?? throw new KeyNotFoundException(); _db.ProjectWorkItems.Remove(wi); await _db.SaveChangesAsync(ct); }
}
