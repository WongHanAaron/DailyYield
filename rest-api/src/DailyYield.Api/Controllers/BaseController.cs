using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyYield.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("sub")?.Value;
        return Guid.Parse(userIdClaim ?? Guid.Empty.ToString());
    }

    protected IEnumerable<Guid> GetCurrentUserGroupIds()
    {
        var groupIdsClaim = User.FindFirst("user_group_ids")?.Value;
        if (string.IsNullOrEmpty(groupIdsClaim))
            return Enumerable.Empty<Guid>();
        return groupIdsClaim.Split(',').Select(Guid.Parse);
    }
}