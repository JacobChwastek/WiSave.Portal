using System.Security.Claims;

namespace WiSave.Portal.WebApi.Middleware;

public class UserContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = GetUserId(context.User);
            var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
            
            if (!string.IsNullOrEmpty(userId))
            {
                context.Request.Headers.Append("X-User-Id", userId);
            }

            if (!string.IsNullOrEmpty(userEmail))
            {
                context.Request.Headers.Append("X-User-Email", userEmail);
            }

            if (userRoles.Length > 0)
            {
                context.Request.Headers.Append("X-User-Roles", string.Join(",", userRoles));
            }
            
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader))
            {
                context.Request.Headers.Append("X-Original-Authorization", authHeader);
            }
        }

        await next(context);
    }

    private static string? GetUserId(ClaimsPrincipal user) =>
        user.FindFirst("sub")?.Value ??
        user.FindFirst("userId")?.Value ??
        user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
        user.FindFirst("id")?.Value;
}