namespace DailyYield.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public Guid UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}