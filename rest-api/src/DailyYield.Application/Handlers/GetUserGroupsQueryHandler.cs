using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using AutoMapper;

namespace DailyYield.Application.Handlers;

public class GetUserGroupsQueryHandler : IRequestHandler<GetUserGroupsQuery, IEnumerable<UserGroupDto>>
{
    private readonly IRepository<UserGroupMember> _memberRepository;
    private readonly IMapper _mapper;

    public GetUserGroupsQueryHandler(IRepository<UserGroupMember> memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserGroupDto>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
    {
        var memberships = await _memberRepository.GetAllAsync();
        var userMemberships = memberships.Where(m => m.UserId == request.UserId);

        var dtos = _mapper.Map<IEnumerable<UserGroupDto>>(userMemberships);

        return dtos;
    }
}