namespace DailyYield.Domain.Entities;

/// <summary>
/// Represents the comparison type for goal evaluation
/// </summary>
public enum GoalComparison
{
    /// <summary>
    /// Goal is achieved when the value is at least the target
    /// </summary>
    AtLeast,

    /// <summary>
    /// Goal is achieved when the value is at most the target
    /// </summary>
    AtMost
}