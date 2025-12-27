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
            Message = request.Message,
            UserId = request.UserId,
            UserGroupId = request.UserGroupId,
            TaskId = request.TaskId,
            MetricTypeId = request.MetricTypeId,
            ReminderType = request.ReminderType,
            Schedule = request.Schedule,
            IsActive = request.IsActive
        };

        await _reminderRepository.AddAsync(reminder);
        return reminder.Id;
    }
}