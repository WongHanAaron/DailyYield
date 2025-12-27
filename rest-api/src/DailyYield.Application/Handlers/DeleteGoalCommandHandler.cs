using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class DeleteGoalCommandHandler : IRequestHandler<DeleteGoalCommand>
{
    private readonly IRepository<Goal> _goalRepository;

    public DeleteGoalCommandHandler(IRepository<Goal> goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async System.Threading.Tasks.Task Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
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

        await _goalRepository.DeleteAsync(goal);
    }
}