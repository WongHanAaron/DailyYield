using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetGoalsQueryHandler : IRequestHandler<GetGoalsQuery, IEnumerable<GoalDto>>
{
    private readonly IRepository<Goal> _goalRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;
    private readonly IMapper _mapper;

    public GetGoalsQueryHandler(
        IRepository<Goal> goalRepository,
        IRepository<MetricType> metricTypeRepository,
        IRepository<UserGroupMember> memberRepository,
        IMapper mapper)
    {
        _goalRepository = goalRepository;
        _metricTypeRepository = metricTypeRepository;
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GoalDto>> Handle(GetGoalsQuery request, CancellationToken cancellationToken)
    {
        var goals = await _goalRepository.GetAllAsync();
        var userGoals = goals.Where(g => g.UserId == request.UserId);

        // Also include goals from user groups the user is member of
        var memberships = await _memberRepository.GetAllAsync();
        var userGroupIds = memberships.Where(m => m.UserId == request.UserId).Select(m => m.UserGroupId).ToList();
        var groupGoals = goals.Where(g => g.UserGroupId.HasValue && userGroupIds.Contains(g.UserGroupId.Value));

        var allGoals = userGoals.Concat(groupGoals);

        if (request.MetricTypeId.HasValue)
        {
            allGoals = allGoals.Where(g => g.MetricTypeId == request.MetricTypeId.Value);
        }

        var metricTypes = await _metricTypeRepository.GetAllAsync();
        var metricTypeDict = metricTypes.ToDictionary(mt => mt.Id, mt => (mt.Key, mt.DisplayName));

        var goalDtos = _mapper.Map<IEnumerable<GoalDto>>(allGoals).ToList();
        
        foreach (var goalDto in goalDtos)
        {
            if (metricTypeDict.TryGetValue(goalDto.MetricTypeId, out var mt))
            {
                goalDto.MetricTypeKey = mt.Key;
                goalDto.MetricTypeDisplayName = mt.DisplayName;
            }
        }

        return goalDtos;
    }
}