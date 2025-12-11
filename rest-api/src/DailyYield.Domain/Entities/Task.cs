namespace DailyYield.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid? UserGroupId { get; set; }
    public UserGroup? UserGroup { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public ICollection<TaskCollaborator> Collaborators { get; set; } = new List<TaskCollaborator>();
    public ICollection<TaskTimer> Timers { get; set; } = new List<TaskTimer>();
    public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}