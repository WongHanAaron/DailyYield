using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class GetGoalQueryHandler : IRequestHandler<GetGoalQuery, GoalDto>
{
    private readonly IRepository<Goal> _goalRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;
    private readonly IMapper _mapper;

    public GetGoalQueryHandler(
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

    public async Task<GoalDto> Handle(GetGoalQuery request, CancellationToken cancellationToken)
    {
        var goal = await _goalRepository.GetByIdAsync(request.Id);
        if (goal == null)
        {
            throw new KeyNotFoundException("Goal not found");
        }

        // Check access
        var hasAccess = goal.UserId == request.UserId;
        if (!hasAccess && goal.UserGroupId.HasValue)
        {
            var memberships = await _memberRepository.GetAllAsync();
            hasAccess = memberships.Any(m => m.UserId == request.UserId && m.UserGroupId == goal.UserGroupId.Value);
        }

        if (!hasAccess)
        {
            throw new UnauthorizedAccessException("User does not have access to this goal");
        }

        var metricType = await _metricTypeRepository.GetByIdAsync(goal.MetricTypeId);

        var goalDto = _mapper.Map<GoalDto>(goal);
        goalDto.MetricTypeKey = metricType?.Key ?? string.Empty;
        goalDto.MetricTypeDisplayName = metricType?.DisplayName ?? string.Empty;

        return goalDto;
    }
}