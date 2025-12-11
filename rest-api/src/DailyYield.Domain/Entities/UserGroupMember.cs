namespace DailyYield.Domain.Entities;

public class UserGroupMember
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;
    public string Role { get; set; } = "member"; // owner, member
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}