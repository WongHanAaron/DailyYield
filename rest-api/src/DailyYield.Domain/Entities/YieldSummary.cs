namespace DailyYield.Domain.Entities;

public class YieldSummary
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;
    public DateTime Date { get; set; }
    public string MetricsSummary { get; set; } = "{}"; // JSON: aggregated values per metric type
    public string TasksSummary { get; set; } = "{}"; // JSON: total time, completed tasks
    public DateTime ComputedAt { get; set; } = DateTime.UtcNow;
}