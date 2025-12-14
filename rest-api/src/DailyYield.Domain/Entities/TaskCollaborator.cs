namespace DailyYield.Domain.Entities;

public class TaskCollaborator
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Role { get; set; } = "collaborator"; // owner, collaborator
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}