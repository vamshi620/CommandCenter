# 📊 PM Command Center

A personal **Project Management Command Center** built with Blazor (.NET 9) — a fast, offline-capable, single-user tool for a Project Manager to track projects, team workload, issues, delays, and daily tasks — all from a premium dark-mode dashboard.

> **For personal use only.** Runs locally with SQLite — zero cloud costs, zero latency, fully private.

---

## ✨ Features

### 📊 Overview Dashboard
- **Project Health Table** — every project at a glance with status, priority, progress bar, team avatars, open issues, delays, and target date
- **KPI Stats Bar** — total projects, active, at-risk, delayed, open issues, team size, and today's log item count
- **📅 Today's Daily Log Summary** — compact 2×2 quadrant summary card showing all 4 categories (Deep Work, Team Sync, Mentorship, Ad-Hoc) with task completion status and an overall day-progress bar; links to the full Daily Log page
- **👥 Team Workload Bars** — visual bars per team member showing open item count (color-coded green/amber/red)
- **📅 Upcoming Deadlines** — next 14 days of due items across all projects, urgency-highlighted
- **📝 Multi-Scratchpads** — create, rename, switch between, and delete multiple named scratchpads — all saved persistently in SQLite (not browser storage)
- **📥 Export CSV** — one-click export of project health table to CSV for stakeholder emails

### 📁 Projects (Kanban Board)
- **Project Selector Tabs** — switch between projects with status color-coding
- **4-Column Kanban** — To Do → In Progress → Blocked → Done
- **‹ › Move Buttons** — move cards between columns without drag-and-drop
- **Work Item Types** — 🔴 Issue, ⚠️ Delay, 🕐 Pending, ⚡ Risk, 🎯 Milestone
- **Priority Badges** — Critical / High / Medium / Low
- **Due Date + Overdue Detection** — cards highlight red when past due date
- **Team Assignment** — assign any project member to a work item inline
- **✏️ Work Item Edit Modal** — click the edit button on any card to open a full detail panel:
  - Edit title, description, type, priority, due date, and assigned team member
  - Full **comment thread** with author name, timestamp, and persistent DB storage
- **Project Notes** — per-project free-text notes field saved to the database
- **Project Progress Bar** — % of items completed, updates in real time
- **Add/Remove Team Members** per project

### 👥 Team Management
- **Team Member Cards** — avatar initials with 8 distinct colors, name, role, department, email
- **Workload Stats** — open items, projects count, completed items per member
- **Project Chips** — shows which projects each member is assigned to
- **Add / Edit / Deactivate / Delete** team members
- **Inactive Badge** — deactivate without deleting historical data

### 📅 Daily Log
- **4 Quadrants** — Deep Work, Team Sync (with Work Item ID/URL), Mentorship, Ad-Hoc Requests
- **Previous Days** — filter by date to review past entries
- **Rollover** — roll incomplete tasks forward to today
- **Status Tracking** — Pending / In Progress / Blocked / Completed / Rolled Over per item

### 👤 PM Profile
- **Editable name** in the top navigation bar — click to rename; saved in the database
- Name automatically used as the **comment author** on all work item comments

### 🌓 Dark / Light Mode
- Toggle between premium dark and clean light themes from the navigation bar
- Preference saved to `localStorage` and persists across page navigations and browser restarts

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| Framework | [Blazor Server (.NET 9)](https://learn.microsoft.com/en-us/aspnet/core/blazor/) |
| Database | SQLite via [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) |
| Styling | Vanilla CSS with CSS Custom Properties (design tokens) |
| Font | [Inter](https://fonts.google.com/specimen/Inter) from Google Fonts |
| Persistence | SQLite — all data including scratchpads, notes, and comments stored server-side |
| Hosting | Localhost (personal tool) |

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git

### Run Locally

```bash
# Clone the repository
git clone https://github.com/vamshi620/CommandCenter.git
cd CommandCenter

# Start the app
cd CommandCenter
dotnet run
```

Open your browser and navigate to **http://localhost:5120**

> The SQLite database is created automatically on first run at `CommandCenter.db` in the project directory.

### First-Time Setup Flow
1. **👥 Team** → Add your team members (name, role, department, email)
2. **📁 Projects** → Create a project, assign team members to it
3. **📊 Overview** → Instantly see health across all projects

---

## 📁 Project Structure

```
CommandCenter/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor       # Top navigation bar
│   │   ├── ThemeToggle.razor      # Dark/Light mode toggle component
│   │   └── UserProfile.razor      # Editable PM name in the nav bar
│   └── Pages/
│       ├── Overview.razor         # / — PM Dashboard (home)
│       ├── Projects.razor         # /projects — Kanban board + Work Item edit modal
│       ├── Team.razor             # /team — Team member management
│       └── Home.razor             # /daily — Daily task log
├── Application/
│   ├── Interfaces/
│   │   └── ICommandCenterService.cs
│   └── Services/
│       └── CommandCenterService.cs
├── Data/
│   └── CommandCenterDbContext.cs
├── Domain/
│   ├── Enums/                     # WorkItemStatus, ProjectStatus, ProjectPriority, etc.
│   └── Models/                    # Project, TeamMember, ProjectMember, ProjectWorkItem,
│                                  # WorkItemComment, Scratchpad, AppSetting, etc.
├── wwwroot/
│   ├── app.css                    # Design system (CSS custom properties, all components)
│   └── app.js                     # JS interop (CSV download)
└── Program.cs                     # Startup + incremental SQLite schema migrations
```

---

## 🗄 Database Schema

The app uses **incremental, idempotent migrations** in `Program.cs` (no EF migrations tooling required). On every startup it:
1. Calls `EnsureCreated()` for fresh databases
2. Runs `ALTER TABLE IF NOT EXISTS` / `CREATE TABLE IF NOT EXISTS` blocks for existing databases
3. Enables **WAL mode** for optimal SQLite performance

### Key Tables
| Table | Purpose |
|---|---|
| `Projects` | Project registry with status, priority, target date, notes |
| `ProjectWorkItems` | Issues, delays, risks, milestones per project |
| `WorkItemComments` | Comment thread per work item with author + timestamp |
| `TeamMembers` | Team member roster with avatar color index |
| `ProjectMembers` | Many-to-many: which members are on which project |
| `DeepWorkTasks` | Daily deep/maker focus tasks |
| `TeamSyncItems` | Daily team sync items with work item URLs |
| `MentorshipTasks` | Junior member mentorship tracking |
| `AdHocRequests` | Ad-hoc requests with requestor name |
| `AppSettings` | Key-value store for app-wide settings (PM name, theme, etc.) |
| `Scratchpads` | Named scratchpad pads with title, content, and timestamps |

---

## 🔑 Known Behaviour

- **`fail:` log lines on startup** — The `ALTER TABLE` statements in `Program.cs` intentionally fail silently when columns already exist (wrapped in `try/catch`). This is expected and harmless.
- **Lock error on rebuild** — If you get `MSB3026` when running `dotnet build`, the old process is still running. Kill it with:
  ```powershell
  Get-Process -Name "CommandCenter" | Stop-Process -Force
  ```
- **Data location** — The SQLite database file (`CommandCenter.db`) is created in the project output directory (`bin/Debug/net9.0/`).

---

## 📄 License

Personal use project. All rights reserved.
