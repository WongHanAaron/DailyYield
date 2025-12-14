using DailyYield.Api.Controllers;
using DailyYield.Application.Commands;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace DailyYield.Api.Controllers;

/// <summary>
/// Controller for managing goals
/// </summary>
[Route("api/goals")]
public class GoalsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GoalsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all goals for the authenticated user
    /// </summary>
    /// <param name="metricTypeId">Optional filter by metric type</param>
    /// <param name="timeframe">Optional filter by timeframe</param>
    /// <returns>List of goals</returns>
    [HttpGet]
    public async Task<IActionResult> GetGoals(
        [FromQuery] Guid? metricTypeId)
    {
        var userId = GetCurrentUserId();
        var query = new GetGoalsQuery
        {
            UserId = userId,
            MetricTypeId = metricTypeId
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific goal by ID
    /// </summary>
    /// <param name="id">The goal ID</param>
    /// <returns>The goal details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoal(Guid id)
    {
        var userId = GetCurrentUserId();
        var query = new GetGoalQuery { Id = id, UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new goal
    /// </summary>
    /// <param name="request">The goal creation data</param>
    /// <returns>The created goal ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalRequest request)
    {
        var userId = GetCurrentUserId();
        var command = _mapper.Map<CreateGoalCommand>(request);
        command.UserId = userId;
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetGoal), new { id = result }, result);
    }

    /// <summary>
    /// Updates an existing goal
    /// </summary>
    /// <param name="id">The goal ID</param>
    /// <param name="request">The updated goal data</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] UpdateGoalRequest request)
    {
        var userId = GetCurrentUserId();
        var command = _mapper.Map<UpdateGoalCommand>(request);
        command.Id = id;
        command.UserId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a goal
    /// </summary>
    /// <param name="id">The goal ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal(Guid id)
    {
        var userId = GetCurrentUserId();
        var command = new DeleteGoalCommand { Id = id, UserId = userId };
        await _mediator.Send(command);
        return NoContent();
    }
}

public class CreateGoalRequest
{
    public Guid MetricTypeId { get; set; }
    public Guid? UserGroupId { get; set; }
    public decimal TargetValue { get; set; }
    public GoalTimeframe Timeframe { get; set; } = GoalTimeframe.Daily;
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; }
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
}

public class UpdateGoalRequest
{
    public decimal TargetValue { get; set; }
    public GoalTimeframe Timeframe { get; set; } = GoalTimeframe.Daily;
    public GoalType GoalType { get; set; } = GoalType.OneTime;
    public string? Frequency { get; set; }
    public GoalComparison Comparison { get; set; } = GoalComparison.AtLeast;
}