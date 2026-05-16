using CommandCenter.Application.Interfaces;
using CommandCenter.Application.Services;
using CommandCenter.Components;
using CommandCenter.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Blazor / Razor Components ───────────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── EF Core with SQLite + WAL mode ─────────────────────────────────────────
// WAL is specified via the connection string in appsettings.json.
// EnsureCreated() runs after app.Build() so the schema is always ready.
builder.Services.AddDbContext<CommandCenterDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Application Services (Scoped to match DbContext lifetime) ───────────────
builder.Services.AddScoped<ICommandCenterService, CommandCenterService>();

var app = builder.Build();

// ── Database startup: create schema + apply incremental changes ────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CommandCenterDbContext>();

    // Creates all tables that don't yet exist (idempotent for new DBs)
    db.Database.EnsureCreated();

    // Enable WAL for optimal concurrent read/write performance
    db.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL;");

    // ── Incremental schema updates (safe: wrapped in try/catch) ─────────────
    // v2: Replace PullRequestUrl with WorkItemId + WorkItemUrl on TeamSyncItems
    try { db.Database.ExecuteSqlRaw("ALTER TABLE TeamSyncItems ADD COLUMN WorkItemId TEXT;"); } catch { /* already exists */ }
    try { db.Database.ExecuteSqlRaw("ALTER TABLE TeamSyncItems ADD COLUMN WorkItemUrl TEXT;"); } catch { /* already exists */ }

    // v2: Project tracking tables (for existing DBs; EnsureCreated covers fresh ones)
    db.Database.ExecuteSqlRaw(
        "CREATE TABLE IF NOT EXISTS Projects (" +
        "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
        "Name TEXT NOT NULL," +
        "Description TEXT," +
        "Status TEXT NOT NULL DEFAULT 'Active'," +
        "Priority TEXT NOT NULL DEFAULT 'Medium'," +
        "CreatedDate TEXT NOT NULL," +
        "TargetDate TEXT);");

    db.Database.ExecuteSqlRaw(
        "CREATE TABLE IF NOT EXISTS ProjectWorkItems (" +
        "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
        "ProjectId INTEGER NOT NULL REFERENCES Projects(Id) ON DELETE CASCADE," +
        "Title TEXT NOT NULL," +
        "Description TEXT," +
        "ItemType TEXT NOT NULL DEFAULT 'Issue'," +
        "Status TEXT NOT NULL DEFAULT 'Pending'," +
        "Priority TEXT NOT NULL DEFAULT 'Medium'," +
        "AssignedTo TEXT," +
        "CreatedDate TEXT NOT NULL," +
        "DueDate TEXT);");

    // v3: Team member management
    db.Database.ExecuteSqlRaw(
        "CREATE TABLE IF NOT EXISTS TeamMembers (" +
        "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
        "Name TEXT NOT NULL," +
        "Role TEXT NOT NULL DEFAULT ''," +
        "Email TEXT," +
        "Department TEXT," +
        "AvatarColorIndex INTEGER NOT NULL DEFAULT 0," +
        "IsActive INTEGER NOT NULL DEFAULT 1," +
        "JoinedDate TEXT NOT NULL);");

    db.Database.ExecuteSqlRaw(
        "CREATE TABLE IF NOT EXISTS ProjectMembers (" +
        "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
        "ProjectId INTEGER NOT NULL REFERENCES Projects(Id) ON DELETE CASCADE," +
        "TeamMemberId INTEGER NOT NULL REFERENCES TeamMembers(Id) ON DELETE CASCADE," +
        "ProjectRole TEXT NOT NULL DEFAULT 'Member');");

    try { db.Database.ExecuteSqlRaw("ALTER TABLE ProjectWorkItems ADD COLUMN AssignedTeamMemberId INTEGER REFERENCES TeamMembers(Id) ON DELETE SET NULL;"); } catch { /* already exists */ }
}

// ── HTTP Pipeline ───────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
