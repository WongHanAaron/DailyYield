namespace DailyYield.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserGroupMember> GroupMemberships { get; set; } = new List<UserGroupMember>();
    public ICollection<MetricEntry> MetricEntries { get; set; } = new List<MetricEntry>();
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}