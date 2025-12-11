namespace DailyYield.Domain.Entities;

public class Reminder
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public Guid? UserGroupId { get; set; }
    public UserGroup? UserGroup { get; set; }
    public Guid? TaskId { get; set; }
    public Task? Task { get; set; }
    public Guid? MetricTypeId { get; set; }
    public MetricType? MetricType { get; set; }
    public string Schedule { get; set; } = string.Empty; // cron or simple
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}