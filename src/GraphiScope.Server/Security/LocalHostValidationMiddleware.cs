using System.Net;

namespace GraphiScope.Server.Security;

/// <summary>
/// Middleware de sécurité HTTP validant le Host, l'Origin et rejetant inconditionnellement Origin: null (ARB-02).
/// </summary>
public sealed class LocalHostValidationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalHostValidationMiddleware(RequestDelegate _next)
    {
        this._next = _next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Rejet strict de Origin: null (iframes sandboxées, contextes opaques)
        if (context.Request.Headers.TryGetValue("Origin", out var originValues))
        {
            string? origin = originValues.ToString();
            if (string.Equals(origin, "null", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Forbidden: 'Origin: null' is rejected by security policy.");
                return;
            }

            // Si une origine est spécifiée, elle doit pointer vers loopback
            if (!string.IsNullOrEmpty(origin) &&
                !origin.StartsWith("http://127.0.0.1", StringComparison.OrdinalIgnoreCase) &&
                !origin.StartsWith("http://localhost", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Forbidden: Cross-origin access rejected.");
                return;
            }
        }

        // 2. Validation stricte du Host (protection anti-DNS Rebinding)
        string? host = context.Request.Host.Host;
        if (!string.Equals(host, "127.0.0.1", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Bad Request: Host header must be loopback (127.0.0.1 or localhost).");
            return;
        }

        await _next(context);
    }
}

