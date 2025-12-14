using DailyYield.Domain.Entities;
using MediatR;

namespace DailyYield.Application.Queries;

public class GetGoalsQuery : IRequest<IEnumerable<GoalDto>>
{
    public Guid UserId { get; set; }
    public Guid? MetricTypeId { get; set; }
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
    public string TargetValue { get; set; } = string.Empty;
    public DateTime? TimeframeStart { get; set; }
    public DateTime? TimeframeEnd { get; set; }
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; }
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
    public GoalStatus Status { get; set; } = GoalStatus.Active;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}