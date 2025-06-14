using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WiSave.Core.Models.Dto;
using WiSave.Core.Models.Requests;
using WiSave.Core.Models.Responses;
using WiSave.Core.Services;

namespace WiSave.Portal.WebApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/auth").WithTags("Authentication");

        auth.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("Register a new user")
            .Produces<AuthResponse>(StatusCodes.Status201Created)
            .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest);

        auth.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Authenticate user")
            .Produces<AuthResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);

        auth.MapPost("/refresh", RefreshTokenAsync)
            .WithName("RefreshToken")
            .WithSummary("Refresh access token")
            .Produces<AuthResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);

        auth.MapPost("/change-password", ChangePasswordAsync)
            .WithName("ChangePassword")
            .WithSummary("Change user password")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        auth.MapGet("/me", GetCurrentUserAsync)
            .WithName("GetCurrentUser")
            .WithSummary("Get current user information")
            .RequireAuthorization()
            .Produces<UserDto>()
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> RegisterAsync([FromBody] RegisterRequest request, [FromServices] IAuthService authService)
    {
        var result = await authService.RegisterAsync(request);
        return result != null
            ? Results.Created("/api/auth/me", result)
            : Results.BadRequest(new ProblemDetails
            {
                Title = "Registration failed",
                Detail = "Unable to register user. Please check your input."
            });
    }

    private static async Task<IResult> LoginAsync([FromBody] LoginRequest request, [FromServices] IAuthService authService)
    {
        var result = await authService.LoginAsync(request);
        return result != null
            ? Results.Ok(result)
            : Results.Unauthorized();
    }

    private static async Task<IResult> RefreshTokenAsync(RefreshTokenRequest request, [FromServices] IAuthService authService)
    {
        var result = await authService.RefreshTokenAsync(request.RefreshToken);
        return result != null
            ? Results.Ok(result)
            : Results.Unauthorized();
    }

    private static async Task<IResult> ChangePasswordAsync(ChangePasswordRequest request, [FromServices] IAuthService authService, ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Results.Unauthorized();

        var result = await authService.ChangePasswordAsync(userId, request);
        return result
            ? Results.Ok()
            : Results.BadRequest(new ProblemDetails
            {
                Title = "Password change failed",
                Detail = "Unable to change password. Please check your current password."
            });
    }

    private static async Task<IResult> GetCurrentUserAsync([FromServices] IAuthService authService, ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Results.Unauthorized();

        var userDto = await authService.GetUserByIdAsync(userId);
        return userDto != null
            ? Results.Ok(userDto)
            : Results.NotFound();
    }
}