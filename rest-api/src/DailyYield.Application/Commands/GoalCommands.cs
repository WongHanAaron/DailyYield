using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Commands;

public class CreateGoalCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid MetricTypeId { get; set; }
    public Guid? UserGroupId { get; set; }
    public decimal TargetValue { get; set; }
    public GoalTimeframe Timeframe { get; set; } = GoalTimeframe.Daily;
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; }
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
}

public class UpdateGoalCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TargetValue { get; set; }
    public GoalTimeframe Timeframe { get; set; } = GoalTimeframe.Daily;
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; }
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
}

public class DeleteGoalCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}