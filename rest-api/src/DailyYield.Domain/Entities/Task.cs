namespace DailyYield.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<TaskCollaborator> Collaborators { get; set; } = new List<TaskCollaborator>();
    public ICollection<TaskTimer> Timers { get; set; } = new List<TaskTimer>();
    public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}