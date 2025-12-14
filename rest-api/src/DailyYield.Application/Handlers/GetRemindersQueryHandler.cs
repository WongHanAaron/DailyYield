using AutoMapper;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using DailyYield.Domain.Ports;
using MediatR;
using TaskEntity = DailyYield.Domain.Entities.Task;

namespace DailyYield.Application.Handlers;

public class GetRemindersQueryHandler : IRequestHandler<GetRemindersQuery, IEnumerable<ReminderDto>>
{
    private readonly IRepository<Reminder> _reminderRepository;
    private readonly IRepository<TaskEntity> _taskRepository;
    private readonly IRepository<MetricType> _metricTypeRepository;
    private readonly IRepository<UserGroupMember> _memberRepository;
    private readonly IMapper _mapper;

    public GetRemindersQueryHandler(
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

    public async Task<IEnumerable<ReminderDto>> Handle(GetRemindersQuery request, CancellationToken cancellationToken)
    {
        var reminders = await _reminderRepository.GetAllAsync();
        var userReminders = reminders.Where(r => r.UserId == request.UserId);

        if (request.IsActive.HasValue)
        {
            userReminders = userReminders.Where(r => r.IsActive == request.IsActive.Value);
        }

        return _mapper.Map<IEnumerable<ReminderDto>>(userReminders);
    }
}