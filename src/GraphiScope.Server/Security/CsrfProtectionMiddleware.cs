using System.Net;

namespace GraphiScope.Server.Security;

/// <summary>
/// Middleware anti-CSRF exigeant le jeton X-GraphiScope-CSRF sur toute opération de mutation d'état (ARB-02).
/// </summary>
public sealed class CsrfProtectionMiddleware
{
    private const string CsrfHeaderName = "X-GraphiScope-CSRF";
    private readonly RequestDelegate _next;
    private readonly CsrfTokenProvider _tokenProvider;

    public CsrfProtectionMiddleware(RequestDelegate _next, CsrfTokenProvider tokenProvider)
    {
        this._next = _next;
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string method = context.Request.Method;

        // Les méthodes GET, HEAD et OPTIONS sont sûres et idempotentes
        if (HttpMethods.IsGet(method) || HttpMethods.IsHead(method) || HttpMethods.IsOptions(method))
        {
            await _next(context);
            return;
        }

        // Pour toute méthode mutatrice (POST, PUT, DELETE, PATCH) : exigence du jeton CSRF
        if (context.Request.Headers.TryGetValue(CsrfHeaderName, out var tokenCandidate))
        {
            if (_tokenProvider.ValidateToken(tokenCandidate.ToString()))
            {
                await _next(context);
                return;
            }
        }

        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        await context.Response.WriteAsync($"Forbidden: Missing or invalid '{CsrfHeaderName}' header.");
    }
}

