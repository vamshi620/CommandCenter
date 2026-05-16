namespace CommandCenter.Domain.Enums;

public enum ProjectStatus
{
    Planning,
    Active,
    AtRisk,
    Delayed,
    OnHold,
    Completed,
    Cancelled
}

public enum ProjectItemType
{
    Issue,
    Delay,
    PendingItem,
    Risk,
    Milestone
}

public enum ProjectPriority
{
    Low,
    Medium,
    High,
    Critical
}
