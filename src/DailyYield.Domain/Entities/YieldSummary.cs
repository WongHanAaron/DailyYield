namespace DailyYield.Domain.Entities;

public class YieldSummary
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;
    public DateTime Date { get; set; }
    public string MetricTotalsJson { get; set; } = "{}";
    public TimeSpan TotalTaskTime { get; set; }
    public int CompletedTasks { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}