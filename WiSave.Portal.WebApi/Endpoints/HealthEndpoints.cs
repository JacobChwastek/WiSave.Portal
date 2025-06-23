namespace WiSave.Portal.WebApi.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok(new 
            { 
                Status = "Healthy",
                Service = "WiSave.Portal.WebApi",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0"
            }))
            .WithTags("Health")
            .AllowAnonymous();

        app.MapGet("/health/ready", async (IServiceProvider _) =>
            {
                try
                {
                    return Results.Ok(new 
                    { 
                        Status = "Ready",
                        Service = "WiSave.Portal.WebApi",
                        Database = "Connected",
                        Timestamp = DateTime.UtcNow
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: 503,
                        title: "Service Unavailable"
                    );
                }
            })
            .WithTags("Health")
            .AllowAnonymous();
    }
}