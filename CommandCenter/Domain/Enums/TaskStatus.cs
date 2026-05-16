namespace CommandCenter.Domain.Enums;

public enum WorkItemStatus
{
    Pending,
    InProgress,
    Blocked,
    Completed,
    RolledOver
}

public enum BlockerStatus
{
    None,
    WaitingForReview,
    WaitingForFeedback,
    Blocked,
    Resolved
}
