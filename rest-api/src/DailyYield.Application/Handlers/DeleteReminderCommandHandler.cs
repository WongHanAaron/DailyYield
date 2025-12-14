using DailyYield.Application.Commands;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using System.Threading.Tasks;

namespace DailyYield.Application.Handlers;

public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand>
{
    private readonly IRepository<Reminder> _reminderRepository;

    public DeleteReminderCommandHandler(IRepository<Reminder> reminderRepository)
    {
        _reminderRepository = reminderRepository;
    }

    public async System.Threading.Tasks.Task Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
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

        await _reminderRepository.DeleteAsync(reminder);
    }
}