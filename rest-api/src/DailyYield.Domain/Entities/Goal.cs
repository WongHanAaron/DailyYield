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
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
    public string TargetValue { get; set; } = string.Empty; // Target as string, matching metric type
    public string? Frequency { get; set; } // daily, weekly, monthly (required for recurring)
    public DateTime? TimeframeStart { get; set; } // Optional start date
    public DateTime? TimeframeEnd { get; set; } // Optional end date
    public GoalStatus Status { get; set; } = GoalStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}