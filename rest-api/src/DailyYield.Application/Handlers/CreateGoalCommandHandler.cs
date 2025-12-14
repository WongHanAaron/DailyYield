using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, Guid>
{
    private readonly IRepository<Goal> _goalRepository;

    public CreateGoalCommandHandler(IRepository<Goal> goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<Guid> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = new Goal
        {
            MetricTypeId = request.MetricTypeId,
            UserId = request.UserId,
            UserGroupId = request.UserGroupId,
            TargetValue = request.TargetValue,
            TimeframeStart = request.TimeframeStart,
            TimeframeEnd = request.TimeframeEnd,
            GoalType = request.GoalType,
            Frequency = request.Frequency,
            Comparison = request.Comparison
        };

        await _goalRepository.AddAsync(goal);
        return goal.Id;
    }
}