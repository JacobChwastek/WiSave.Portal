using Microsoft.AspNetCore.Mvc;
using WiSave.Core.Models.Dto;
using WiSave.Core.Models.Requests;
using WiSave.Core.Services;

namespace WiSave.Portal.WebApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("/api/users").WithTags("User Management");
        
        users.MapGet("/", GetAllUsersAsync)
            .WithName("GetAllUsers")
            .WithSummary("Get all users")
            .RequireAuthorization("AdminOnly")
            .Produces<IEnumerable<UserDto>>(StatusCodes.Status200OK);

        users.MapGet("/{id}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithSummary("Get user by ID")
            .RequireAuthorization("ModeratorOrAdmin")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        users.MapPost("/{id}/assign-role", AssignRoleAsync)
            .WithName("AssignRole")
            .WithSummary("Assign role to user")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        users.MapDelete("/{id}/remove-role", RemoveRoleAsync)
            .WithName("RemoveRole")
            .WithSummary("Remove role from user")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        users.MapPatch("/{id}/deactivate", DeactivateUserAsync)
            .WithName("DeactivateUser")
            .WithSummary("Deactivate user account")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        users.MapPatch("/{id}/activate", ActivateUserAsync)
            .WithName("ActivateUser")
            .WithSummary("Activate user account")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllUsersAsync([FromServices] IAuthService authService)
    {
        var users = await authService.GetAllUsersAsync();
        return Results.Ok(users);
    }

    private static async Task<IResult> GetUserByIdAsync(string id, [FromServices] IAuthService authService)
    {
        var user = await authService.GetUserByIdAsync(id);
        return user != null
            ? Results.Ok(user)
            : Results.NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = $"User with ID {id} was not found."
            });
    }

    private static async Task<IResult> AssignRoleAsync(string id, [FromBody] AssignRoleRequest request, [FromServices] IAuthService authService)
    {
        var result = await authService.AssignRoleAsync(request.UserId, request.RoleName);
        return result
            ? Results.Ok()
            : Results.BadRequest(new ProblemDetails
            {
                Title = "Role assignment failed",
                Detail = "Unable to assign role to user."
            });
    }

    private static async Task<IResult> RemoveRoleAsync(string id, [FromBody] AssignRoleRequest request, [FromServices] IAuthService authService)
    {
        var result = await authService.RemoveRoleAsync(request.UserId, request.RoleName);
        return result
            ? Results.Ok()
            : Results.BadRequest(new ProblemDetails
            {
                Title = "Role removal failed",
                Detail = "Unable to remove role from user."
            });
    }

    private static async Task<IResult> DeactivateUserAsync(string id, [FromServices] IAuthService authService)
    {
        var result = await authService.DeactivateUserAsync(id);
        return result
            ? Results.Ok()
            : Results.NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = $"User with ID {id} was not found."
            });
    }

    private static async Task<IResult> ActivateUserAsync(string id, [FromServices] IAuthService authService)
    {
        var result = await authService.ActivateUserAsync(id);
        return result
            ? Results.Ok()
            : Results.NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = $"User with ID {id} was not found."
            });
    }
}