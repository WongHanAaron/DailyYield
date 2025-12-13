using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand>
{
    private readonly IRepository<Reminder> _reminderRepository;

    public UpdateReminderCommandHandler(IRepository<Reminder> reminderRepository)
    {
        _reminderRepository = reminderRepository;
    }

    public async System.Threading.Tasks.Task Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = await _reminderRepository.GetByIdAsync(request.Id);
        if (reminder == null)
        {
            throw new KeyNotFoundException("Reminder not found");
        }

        if (reminder.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("User does not own this reminder");
        }

        reminder.Title = request.Title;
        reminder.Description = request.Description;
        reminder.ScheduledAt = request.ScheduledAt;
        reminder.IsRecurring = request.IsRecurring;
        reminder.RecurrencePattern = request.RecurrencePattern;
        reminder.Status = request.Status;

        await _reminderRepository.UpdateAsync(reminder);
    }
}