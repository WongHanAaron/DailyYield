namespace DailyYield.Domain.Entities;

public class UserGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = "UTC";
    public string? Settings { get; set; } // JSON for user group preferences
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserGroupMember> Members { get; set; } = new List<UserGroupMember>();
}