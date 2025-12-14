namespace DailyYield.Domain.Entities;

/// <summary>
/// Represents the type of reminder schedule
/// </summary>
public enum ReminderScheduleType
{
    /// <summary>
    /// Simple schedule (e.g., daily at specific time)
    /// </summary>
    Simple,

    /// <summary>
    /// Cron-based schedule for complex recurring patterns
    /// </summary>
    Cron
}