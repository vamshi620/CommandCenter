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

// ── Ensure database is created and WAL mode is enabled ───────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CommandCenterDbContext>();
    db.Database.EnsureCreated();
    // Enable WAL (Write-Ahead Logging) for optimal concurrent read/write performance
    db.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL;");
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
