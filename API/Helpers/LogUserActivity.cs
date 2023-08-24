using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // This waits the ActionExecutingContext to finish its execution
        var resultContext = await next();

        // This exits the method if the user is not authenticated, even thought that is going to be executed in
        // Controllers that require to be authenticated
        if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

        var userId = resultContext.HttpContext.User.GetUserId();

        var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var user = await repo.GetUserByIdAsync(userId);

        user.LastActive = DateTime.UtcNow;
        await repo.SaveAllAsync();
    }
}