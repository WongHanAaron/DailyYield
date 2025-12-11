namespace DailyYield.Domain.Entities;

public class Goal
{
    public Guid Id { get; set; }
    public Guid MetricTypeId { get; set; }
    public MetricType MetricType { get; set; } = null!;
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public Guid? UserGroupId { get; set; }
    public UserGroup? UserGroup { get; set; }
    public decimal TargetValue { get; set; }
    public string Timeframe { get; set; } = "daily"; // daily, weekly, monthly
    public string GoalType { get; set; } = "one-time"; // one-time, recurring
    public string? Frequency { get; set; } // for recurring
    public string Comparison { get; set; } = "at-least"; // at-least, at-most
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}