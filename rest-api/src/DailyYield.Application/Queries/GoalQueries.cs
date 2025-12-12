using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Queries;

public class GetGoalsQuery : IRequest<IEnumerable<GoalDto>>
{
    public Guid UserId { get; set; }
    public Guid? MetricTypeId { get; set; }
    public GoalTimeframe? Timeframe { get; set; }
}

public class GetGoalQuery : IRequest<GoalDto>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class GoalDto
{
    public Guid Id { get; set; }
    public Guid MetricTypeId { get; set; }
    public string MetricTypeKey { get; set; } = string.Empty;
    public string MetricTypeDisplayName { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public Guid? UserGroupId { get; set; }
    public decimal TargetValue { get; set; }
    public GoalTimeframe Timeframe { get; set; } = GoalTimeframe.Daily;
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; }
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
    public DateTime CreatedAt { get; set; }
}