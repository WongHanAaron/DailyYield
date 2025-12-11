using MediatR;

namespace DailyYield.Application.Commands;

public class CreateUserGroupCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = "UTC";
    public Guid OwnerId { get; set; }
}