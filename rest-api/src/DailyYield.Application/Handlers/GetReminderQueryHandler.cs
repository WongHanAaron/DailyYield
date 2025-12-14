using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class GetReminderQueryHandler : IRequestHandler<GetReminderQuery, ReminderDto>
{
    private readonly IRepository<Reminder> _reminderRepository;
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;
    private readonly IMapper _mapper;

    public GetReminderQueryHandler(
        IRepository<Reminder> reminderRepository,
        IRepository<TaskEntity> taskRepository,
        IRepository<MetricType> metricTypeRepository,
        IRepository<UserGroupMember> memberRepository,
        IMapper mapper)
    {
        _reminderRepository = reminderRepository;
        _taskRepository = taskRepository;
        _metricTypeRepository = metricTypeRepository;
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<ReminderDto> Handle(GetReminderQuery request, CancellationToken cancellationToken)
    {
        var reminder = await _reminderRepository.GetByIdAsync(request.Id);
        if (reminder == null)
        {
            throw new KeyNotFoundException("Reminder not found");
        }

        // Check access - user must own the reminder
        if (reminder.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("User does not have access to this reminder");
        }

        return _mapper.Map<ReminderDto>(reminder);
    }
}