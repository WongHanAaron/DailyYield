namespace DailyYield.Domain.Entities;

public class TaskTimer
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public TimeSpan? Duration { get; set; }
}