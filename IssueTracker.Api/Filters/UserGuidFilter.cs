using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

public class UserGuidFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var userGuid))
        {
            context.ActionArguments["userGuid"] = userGuid;
        }
        else
        {
            throw new UnauthorizedAccessException("User is not authenticated or has an invalid ID.");
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // do nothing)
    }
}
