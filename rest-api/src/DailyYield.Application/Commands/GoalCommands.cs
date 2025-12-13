using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Commands;

public class CreateGoalCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid MetricTypeId { get; set; }
    public Guid? UserGroupId { get; set; }
    public string TargetValue { get; set; } = string.Empty;
    public DateTime? TimeframeStart { get; set; }
    public DateTime? TimeframeEnd { get; set; }
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; }
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
}

public class UpdateGoalCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TargetValue { get; set; } = string.Empty;
    public DateTime? TimeframeStart { get; set; }
    public DateTime? TimeframeEnd { get; set; }
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; }
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
    public GoalStatus Status { get; set; } = GoalStatus.Active;
}

public class DeleteGoalCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}