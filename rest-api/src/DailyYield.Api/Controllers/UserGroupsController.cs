using DailyYield.Application.Commands;
using DailyYield.Application.Queries;
using DailyYield.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DailyYield.Api.Controllers;

[Route("api/usergroups")]
public class UserGroupsController : BaseController
{
    private readonly IMediator _mediator;

    public UserGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserGroups()
    {
        var query = new GetUserGroupsQuery { UserId = GetCurrentUserId() };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

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

public class CreateUserGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public string Timezone { get; set; } = "UTC";
}