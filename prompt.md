System Role
You are an Expert .NET 10 Enterprise Architect and Blazor Developer. Your task is to generate the foundational code for a local "Command Center" web application.

Objective
Build a lightweight, strictly formatted Blazor Web App that acts as a personal productivity dashboard. The application will not be hosted; it will run locally via Kestrel (dotnet run) in a restricted Virtual Desktop environment.

Technology Stack
Framework: .NET 10

Frontend: Blazor Web App (Interactive Server render mode) with PWA capabilities enabled.

Backend/Data Access: Entity Framework Core (Code-First).

Database: SQLite.

Architecture Pattern: Clean Architecture principles, ensuring separation of concerns between the UI, Domain Models, and Data Access layers.

Core Data Models (Domain Layer)
Create the following Entity Framework Core models with appropriate properties (Id, Title, Description, CreatedDate, DueDate, Status, etc.):

DeepWorkTask: For individual dev deliverables (needs a TimeEstimatedHours property).

TeamSyncItem: For tracking dependencies and reviews for 2 teammates (needs a BlockerStatus and PullRequestUrl property).

MentorshipTask: For tracking assignments given to 2 junior developers (needs an AssignedJuniorName and ExpectedSopStandard property).

AdHocRequest: For unplanned estimations/guidance (needs a RequestorName, TimeboxMinutes, and Deadline property).

Database Configuration Requirements
Provide the exact DbContext implementation.

Provide the appsettings.json connection string mapped to a local file (CommandCenter.db).

In the Program.cs configuration, enforce SQLite WAL (Write-Ahead Logging) mode via the connection string for optimal concurrent read/write performance. Ensure the database is auto-created on startup using context.Database.EnsureCreated().

UI Layout (Blazor Frontend)
Generate a single-page dashboard (Home.razor) divided into a 2x2 CSS Grid representing four quadrants:

Quadrant 1: Maker Work: Displays DeepWorkTask items.

Quadrant 2: Team Orchestration: Displays TeamSyncItem items.

Quadrant 3: Mentorship Queue: Displays MentorshipTask items.

Quadrant 4: Ad-Hoc Triage: Displays AdHocRequest items.

UI Constraints: Use basic Bootstrap or Tailwind CSS classes (whichever is standard in the default template) for a clean, distraction-free UI. Include a "Rollover Incomplete Tasks" button that visually represents a sweep of yesterday's items.

Execution Steps (Output Generation)
Please generate the code in the following order:

The Domain Models and DbContext.

The appsettings.json and Program.cs setup logic.

A lightweight Service interface and implementation for data retrieval/updates.

The Home.razor dashboard component utilizing the service.

Ensure all generated code follows strict SOLID principles and contains no deprecated .NET features.