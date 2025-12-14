using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class StopTaskTimerCommandHandler : IRequestHandler<StopTaskTimerCommand>
{
    private readonly IRepository<TaskTimer> _timerRepository;

    public StopTaskTimerCommandHandler(IRepository<TaskTimer> timerRepository)
    {
        _timerRepository = timerRepository;
    }

    public async System.Threading.Tasks.Task Handle(StopTaskTimerCommand request, CancellationToken cancellationToken)
    {
        var timer = await _timerRepository.GetByIdAsync(request.TimerId);
        if (timer == null)
        {
            throw new KeyNotFoundException("Timer not found");
        }

        // Note: In a real implementation, check if user owns the task
        // For now, assume timer belongs to user's task

        var endedAt = DateTime.UtcNow;
        timer.EndedAt = endedAt;
        timer.Duration = endedAt - timer.StartedAt;

        await _timerRepository.UpdateAsync(timer);
    }
}