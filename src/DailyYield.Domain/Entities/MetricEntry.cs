namespace DailyYield.Domain.Entities;

public class MetricEntry
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid MetricTypeId { get; set; }
    public MetricType MetricType { get; set; } = null!;
    public decimal? NumericValue { get; set; }
    public bool? BooleanValue { get; set; }
    public string? CategoryValue { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}