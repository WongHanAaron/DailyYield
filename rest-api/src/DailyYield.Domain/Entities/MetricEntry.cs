namespace DailyYield.Domain.Entities;

public class MetricEntry
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid MetricTypeId { get; set; }
    public MetricType MetricType { get; set; } = null!;
    public MetricEntryType Type { get; set; }
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime? StartedAt { get; set; } // Optional, for durations
    public DateTime? EndedAt { get; set; } // Optional
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Metadata { get; set; } // JSON for extra context
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}