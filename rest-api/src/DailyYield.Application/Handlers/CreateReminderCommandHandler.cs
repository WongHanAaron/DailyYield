using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;

namespace DailyYield.Application.Handlers;

public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand, Guid>
{
    private readonly IRepository<Reminder> _reminderRepository;

    public CreateReminderCommandHandler(IRepository<Reminder> reminderRepository)
    {
        _reminderRepository = reminderRepository;
    }

    public async Task<Guid> Handle(CreateReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = new Reminder
        {
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
            UserGroupId = request.UserGroupId,
            TaskId = request.TaskId,
            MetricTypeId = request.MetricTypeId,
            ScheduleType = request.ScheduleType,
            Schedule = request.Schedule
        };

        await _reminderRepository.AddAsync(reminder);
        return reminder.Id;
    }
}