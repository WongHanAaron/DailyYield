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
    public GoalTimeframe Timeframe { get; set; } = GoalTimeframe.Daily;
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; } // for recurring
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}