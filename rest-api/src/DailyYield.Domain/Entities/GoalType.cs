namespace DailyYield.Domain.Entities;

/// <summary>
/// Represents the type of goal
/// </summary>
public enum GoalType
{
    /// <summary>
    /// One-time goal that is achieved once
    /// </summary>
    OneTime,

    /// <summary>
    /// Recurring goal that repeats over time
    /// </summary>
    Recurring
}