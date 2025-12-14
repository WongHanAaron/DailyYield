using DailyYield.Api.Controllers;
using DailyYield.Application.Commands;
using DailyYield.Application.Queries;
using DailyYield.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace DailyYield.Api.Controllers;

/// <summary>
/// Controller for managing reminders
/// </summary>
[Route("api/reminders")]
public class RemindersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RemindersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all reminders for the authenticated user
    /// </summary>
    /// <param name="isActive">Optional filter by active status</param>
    /// <returns>List of reminders</returns>
    [HttpGet]
    public async Task<IActionResult> GetReminders([FromQuery] bool? isActive)
    {
        var userId = GetCurrentUserId();
        var query = new GetRemindersQuery { UserId = userId, IsActive = isActive };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific reminder by ID
    /// </summary>
    /// <param name="id">The reminder ID</param>
    /// <returns>The reminder details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReminder(Guid id)
    {
        var userId = GetCurrentUserId();
        var query = new GetReminderQuery { Id = id, UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new reminder
    /// </summary>
    /// <param name="request">The reminder creation data</param>
    /// <returns>The created reminder ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateReminder([FromBody] CreateReminderRequest request)
    {
        var userId = GetCurrentUserId();
        var command = _mapper.Map<CreateReminderCommand>(request);
        command.UserId = userId;
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetReminder), new { id = result }, result);
    }

    /// <summary>
    /// Updates an existing reminder
    /// </summary>
    /// <param name="id">The reminder ID</param>
    /// <param name="request">The updated reminder data</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReminder(Guid id, [FromBody] UpdateReminderRequest request)
    {
        var userId = GetCurrentUserId();
        var command = _mapper.Map<UpdateReminderCommand>(request);
        command.Id = id;
        command.UserId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a reminder
    /// </summary>
    /// <param name="id">The reminder ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReminder(Guid id)
    {
        var userId = GetCurrentUserId();
        var command = new DeleteReminderCommand { Id = id, UserId = userId };
        await _mediator.Send(command);
        return NoContent();
    }
}

public class CreateReminderRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? UserGroupId { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? MetricTypeId { get; set; }
    public ReminderScheduleType ScheduleType { get; set; } = ReminderScheduleType.Simple;
    public string Schedule { get; set; } = string.Empty;
}

public class UpdateReminderRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? MetricTypeId { get; set; }
    public ReminderScheduleType ScheduleType { get; set; } = ReminderScheduleType.Simple;
    public string Schedule { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}