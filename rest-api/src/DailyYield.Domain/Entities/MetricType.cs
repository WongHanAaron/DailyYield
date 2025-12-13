namespace DailyYield.Domain.Entities;

public class MetricType
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public MetricDataType Type { get; set; } = MetricDataType.Numeric;
    public string? Unit { get; set; }
    public string? Aggregation { get; set; } // sum, avg, count for numeric types
    public string? Categories { get; set; } // JSON array of strings for categorical types
    public Guid UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<MetricEntry> Entries { get; set; } = new List<MetricEntry>();
    public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}