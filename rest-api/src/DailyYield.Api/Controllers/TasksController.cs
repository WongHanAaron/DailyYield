using DailyYield.Api.Controllers;
using DailyYield.Application.Commands;
using DailyYield.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace DailyYield.Api.Controllers;

/// <summary>
/// Controller for managing tasks
/// </summary>
[Route("api/tasks")]
public class TasksController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TasksController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all tasks for the authenticated user
    /// </summary>
    /// <param name="isCompleted">Optional filter by completion status</param>
    /// <returns>List of tasks</returns>
    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] bool? isCompleted)
    {
        var userId = GetCurrentUserId();
        var query = new GetTasksQuery { UserId = userId, IsCompleted = isCompleted };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a specific task by ID
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <returns>The task details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id)
    {
        var userId = GetCurrentUserId();
        var query = new GetTaskQuery { Id = id, UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new task
    /// </summary>
    /// <param name="request">The task creation data</param>
    /// <returns>The created task ID</returns>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var userId = GetCurrentUserId();
        var command = _mapper.Map<CreateTaskCommand>(request);
        command.UserId = userId;
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTask), new { id = result }, result);
    }

    /// <summary>
    /// Updates an existing task
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <param name="request">The updated task data</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
    {
        var userId = GetCurrentUserId();
        var command = _mapper.Map<UpdateTaskCommand>(request);
        command.Id = id;
        command.UserId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a task
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var userId = GetCurrentUserId();
        var command = new DeleteTaskCommand { Id = id, UserId = userId };
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Starts a timer for a task
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <returns>The timer ID</returns>
    [HttpPost("{id}/start-timer")]
    public async Task<IActionResult> StartTimer(Guid id)
    {
        var userId = GetCurrentUserId();
        var command = new StartTaskTimerCommand { TaskId = id, UserId = userId };
        var result = await _mediator.Send(command);
        return Ok(new { TimerId = result });
    }

    /// <summary>
    /// Stops a task timer
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <param name="timerId">The timer ID</param>
    /// <returns>No content</returns>
    [HttpPost("{id}/stop-timer")]
    public async Task<IActionResult> StopTimer(Guid id, [FromQuery] Guid timerId)
    {
        var userId = GetCurrentUserId();
        var command = new StopTaskTimerCommand { TimerId = timerId, UserId = userId };
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Gets timers for a task
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <returns>List of timers</returns>
    [HttpGet("{id}/timers")]
    public async Task<IActionResult> GetTaskTimers(Guid id)
    {
        var userId = GetCurrentUserId();
        var query = new GetTaskTimersQuery { TaskId = id, UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets collaborators for a task
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <returns>List of collaborators</returns>
    [HttpGet("{id}/collaborators")]
    public async Task<IActionResult> GetTaskCollaborators(Guid id)
    {
        var userId = GetCurrentUserId();
        var query = new GetTaskCollaboratorsQuery { TaskId = id, UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Adds a collaborator to a task
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <param name="request">The collaborator data</param>
    /// <returns>No content</returns>
    [HttpPost("{id}/collaborators")]
    public async Task<IActionResult> AddCollaborator(Guid id, [FromBody] AddCollaboratorRequest request)
    {
        var userId = GetCurrentUserId();
        var command = new AddTaskCollaboratorCommand
        {
            TaskId = id,
            CollaboratorUserId = request.UserId,
            OwnerUserId = userId
        };
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Removes a collaborator from a task
    /// </summary>
    /// <param name="id">The task ID</param>
    /// <param name="userId">The collaborator user ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}/collaborators/{userId}")]
    public async Task<IActionResult> RemoveCollaborator(Guid id, Guid userId)
    {
        var ownerUserId = GetCurrentUserId();
        var command = new RemoveTaskCollaboratorCommand
        {
            TaskId = id,
            CollaboratorUserId = userId,
            OwnerUserId = ownerUserId
        };
        await _mediator.Send(command);
        return NoContent();
    }
}

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? UserGroupId { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
}

public class AddCollaboratorRequest
{
    public Guid UserId { get; set; }
}