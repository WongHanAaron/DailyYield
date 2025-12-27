using MediatR;

namespace DailyYield.Application.Queries;

public class GetUserGroupsQuery : IRequest<IEnumerable<UserGroupDto>>
{
    public Guid UserId { get; set; }
}

public class UserGroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = "UTC";
    public string Role { get; set; } = "member";
}