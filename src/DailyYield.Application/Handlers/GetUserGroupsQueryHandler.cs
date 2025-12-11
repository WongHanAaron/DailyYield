using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetUserGroupsQueryHandler : IRequestHandler<GetUserGroupsQuery, IEnumerable<UserGroupDto>>
{
    private readonly IRepository<UserGroupMember> _memberRepository;

    public GetUserGroupsQueryHandler(IRepository<UserGroupMember> memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<UserGroupDto>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
    {
        var memberships = await _memberRepository.GetAllAsync();
        var userMemberships = memberships.Where(m => m.UserId == request.UserId);

        var dtos = userMemberships.Select(m => new UserGroupDto
        {
            Id = m.UserGroup.Id,
            Name = m.UserGroup.Name,
            Timezone = m.UserGroup.Timezone,
            Role = m.Role
        });

        return dtos;
    }
}