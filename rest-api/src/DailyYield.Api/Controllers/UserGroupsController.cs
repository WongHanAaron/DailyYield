using DailyYield.Application.Commands;
using DailyYield.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DailyYield.Api.Controllers;

/// <summary>
/// Controller for managing user groups
/// </summary>
[Route("api/usergroups")]
public class UserGroupsController : BaseController
{
    private readonly IMediator _mediator;

    public UserGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets all user groups for the current user
    /// </summary>
    /// <returns>A list of user groups</returns>
    /// <response code="200">Returns the list of user groups</response>
    [HttpGet]
    public async Task<IActionResult> GetUserGroups()
    {
        var query = new GetUserGroupsQuery { UserId = GetCurrentUserId() };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new user group
    /// </summary>
    /// <param name="request">The user group creation request</param>
    /// <returns>The ID of the created user group</returns>
    /// <response code="201">Returns the ID of the newly created user group</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    public async Task<IActionResult> CreateUserGroup([FromBody] CreateUserGroupRequest request)
    {
        var command = new CreateUserGroupCommand
        {
            Name = request.Name,
            Timezone = request.Timezone,
            OwnerId = GetCurrentUserId()
        };
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserGroups), new { id }, new { Id = id });
    }
}

/// <summary>
/// Request model for creating a user group
/// </summary>
public class CreateUserGroupRequest
{
    /// <summary>
    /// The name of the user group
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The timezone of the user group (defaults to UTC)
    /// </summary>
    public string Timezone { get; set; } = "UTC";
}