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
            ScheduledAt = request.ScheduledAt,
            IsRecurring = request.IsRecurring,
            RecurrencePattern = request.RecurrencePattern
        };

        await _reminderRepository.AddAsync(reminder);
        return reminder.Id;
    }
}