using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class UpdateGoalCommandHandler : IRequestHandler<UpdateGoalCommand>
{
    private readonly IRepository<Goal> _goalRepository;

    public UpdateGoalCommandHandler(IRepository<Goal> goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async System.Threading.Tasks.Task Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = await _goalRepository.GetByIdAsync(request.Id);
        if (goal == null)
        {
            throw new KeyNotFoundException("Goal not found");
        }

        if (goal.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("User does not own this goal");
        }

        goal.TargetValue = request.TargetValue;
        goal.TimeframeStart = request.TimeframeStart;
        goal.TimeframeEnd = request.TimeframeEnd;
        goal.GoalType = request.GoalType;
        goal.Frequency = request.Frequency;
        goal.Comparison = request.Comparison;
        goal.Status = request.Status;

        await _goalRepository.UpdateAsync(goal);
    }
}