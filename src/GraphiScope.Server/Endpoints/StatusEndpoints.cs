using GraphiScope.Server.Security;

namespace GraphiScope.Server.Endpoints;

/// <summary>
/// Points d'accès REST de consultation et d'état du serveur (safe et idempotents).
/// </summary>
public static class StatusEndpoints
{
    public static IEndpointRouteBuilder MapStatusEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/status", () => Results.Ok(new
        {
            Service = "GraphiScope Local Server",
            Status = "ONLINE",
            Version = "1.0.0-B1",
            Framework = ".NET 10 LTS",
            TimestampUtc = DateTime.UtcNow
        }));

        app.MapGet("/api/health", () => Results.Ok(new { Health = "Healthy" }));

        // Fournit le jeton de session CSRF au client légitime
        app.MapGet("/api/csrf-token", (CsrfTokenProvider tokenProvider) =>
            Results.Ok(new { CsrfToken = tokenProvider.Token }));

        return app;
    }
}

