namespace DailyYield.Domain.Entities;

public class UserGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = "UTC";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserGroupMember> Members { get; set; } = new List<UserGroupMember>();
}