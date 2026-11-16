using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using GraphiScope.Core.Domain;
using GraphiScope.Core.Persistence;
using GraphiScope.Core.Persistence.Repositories;
using GraphiScope.Core.Security;
using GraphiScope.Core.Services;
using GraphiScope.Server.Endpoints;
using GraphiScope.Server.Security;
using Xunit;

namespace GraphiScope.Tests;

public class ServerSecurityTests
{
    [Fact]
    public void CsrfTokenProvider_Generates256BitHexToken()
    {
        var provider = new CsrfTokenProvider();

        // 32 octets = 256 bits = 64 caractères hexadécimaux
        Assert.NotNull(provider.Token);
        Assert.Equal(64, provider.Token.Length);

        // Validation du jeton valide
        Assert.True(provider.ValidateToken(provider.Token));

        // Rejet des jetons altérés ou invalides
        Assert.False(provider.ValidateToken(null));
        Assert.False(provider.ValidateToken(""));
        Assert.False(provider.ValidateToken("INVALID_TOKEN"));
        Assert.False(provider.ValidateToken(provider.Token[..^1] + "0")); // altéré
    }

    [Fact]
    public async Task LocalHostValidationMiddleware_AllowsLoopbackHost()
    {
        var middleware = new LocalHostValidationMiddleware((innerContext) =>
        {
            innerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Task.CompletedTask;
        });

        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("127.0.0.1", 5000);

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task LocalHostValidationMiddleware_RejectsExternalHost_WithBadRequest()
    {
        var middleware = new LocalHostValidationMiddleware((innerContext) =>
        {
            innerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Task.CompletedTask;
        });

        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("evil.com", 5000);

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task LocalHostValidationMiddleware_RejectsOriginNull_WithForbidden()
    {
        var middleware = new LocalHostValidationMiddleware((innerContext) =>
        {
            innerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Task.CompletedTask;
        });

        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("127.0.0.1", 5000);
        context.Request.Headers["Origin"] = "null"; // ARB-02 : rejet inconditionnel

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task LocalHostValidationMiddleware_RejectsRemoteOrigin_WithForbidden()
    {
        var middleware = new LocalHostValidationMiddleware((innerContext) =>
        {
            innerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Task.CompletedTask;
        });

        var context = new DefaultHttpContext();
        context.Request.Host = new HostString("127.0.0.1", 5000);
        context.Request.Headers["Origin"] = "http://attacker.com";

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task CsrfProtectionMiddleware_AllowsSafeGetWithoutToken()
    {
        var tokenProvider = new CsrfTokenProvider();
        var middleware = new CsrfProtectionMiddleware((innerContext) =>
        {
            innerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Task.CompletedTask;
        }, tokenProvider);

        var context = new DefaultHttpContext();
        context.Request.Method = "GET";

        await middleware.InvokeAsync(context);

        // GET traverse sans jeton
        Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task CsrfProtectionMiddleware_RejectsMutatingPost_WithoutToken()
    {
        var tokenProvider = new CsrfTokenProvider();
        var middleware = new CsrfProtectionMiddleware((innerContext) =>
        {
            innerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Task.CompletedTask;
        }, tokenProvider);

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";

        await middleware.InvokeAsync(context);

        // POST sans jeton -> 403 Forbidden
        Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task CsrfProtectionMiddleware_RejectsMutatingPost_WithInvalidToken()
    {
        var tokenProvider = new CsrfTokenProvider();
        var middleware = new CsrfProtectionMiddleware((innerContext) =>
        {
            innerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Task.CompletedTask;
        }, tokenProvider);

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Headers["X-GraphiScope-CSRF"] = "CORRUPTED_HEX_TOKEN_1234567890";

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task CsrfProtectionMiddleware_AllowsMutatingPost_WithValidToken()
    {
        var tokenProvider = new CsrfTokenProvider();
        var middleware = new CsrfProtectionMiddleware((innerContext) =>
        {
            innerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return Task.CompletedTask;
        }, tokenProvider);

        var context = new DefaultHttpContext();
        context.Request.Method = "POST";
        context.Request.Headers["X-GraphiScope-CSRF"] = tokenProvider.Token;

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task Kestrel_RealHttpPipeline_DemonstratesEndToEndConfinementAndCsrf()
    {
        // 1. Démarrer Kestrel en écoute réelle sur loopback 127.0.0.1 avec port dynamique (0)
        var tempDir = Path.Combine(Path.GetTempPath(), "GraphiScope_KestrelTest_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        var dbPath = Path.Combine(tempDir, "kestrel_test.db");

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Loopback, 0); // Écoute exclusivement 127.0.0.1, port 0
        });

        // Enregistrement des composants réels
        var confinement = new PathConfinementService();
        var tokenProvider = new CsrfTokenProvider();
        var connectionFactory = new SqliteConnectionFactory(dbPath, confinement, customAllowedCatalogDir: tempDir);
        var schemaManager = new SqliteSchemaManager(connectionFactory);
        await schemaManager.InitializeSchemaAsync();

        builder.Services.AddSingleton(confinement);
        builder.Services.AddSingleton(tokenProvider);
        builder.Services.AddSingleton<IDbConnectionFactory>(connectionFactory);
        builder.Services.AddSingleton(schemaManager);
        builder.Services.AddScoped<IStorageRootRepository, StorageRootRepository>();
        builder.Services.AddScoped<IFileInventoryRepository, FileInventoryRepository>();
        builder.Services.AddScoped<IFileSystemCrawler, FileSystemCrawler>();
        builder.Services.AddScoped<IHashService, HashService>();
        builder.Services.AddScoped<IInventoryCoordinator, InventoryCoordinator>();

        var app = builder.Build();
        app.UseMiddleware<LocalHostValidationMiddleware>();
        app.UseMiddleware<CsrfProtectionMiddleware>();
        app.MapStatusEndpoints();
        app.MapInventoryEndpoints();

        await app.StartAsync();

        try
        {
            // Récupérer l'adresse et le port dynamique effectivement attribués
            var serverAddress = app.Urls.First();
            var uri = new Uri(serverAddress);

            // C.1 : Vérification que l'adresse est loopback 127.0.0.1 et que le port dynamique est > 0
            Assert.Equal("127.0.0.1", uri.Host);
            Assert.True(uri.Port > 0);

            using var handler = new HttpClientHandler();
            using var client = new HttpClient(handler) { BaseAddress = uri };

            // C.2 : Requête GET légitime vers /api/status -> 200 OK
            var statusResp = await client.GetAsync("/api/status");
            Assert.Equal(HttpStatusCode.OK, statusResp.StatusCode);

            // C.2 : Rejet de Origin: null via vraie requête HTTP -> 403 Forbidden
            using var originNullReq = new HttpRequestMessage(HttpMethod.Get, "/api/status");
            originNullReq.Headers.Add("Origin", "null");
            var originNullResp = await client.SendAsync(originNullReq);
            Assert.Equal(HttpStatusCode.Forbidden, originNullResp.StatusCode);

            // C.2 : Rejet d'une origine externe via vraie requête HTTP -> 403 Forbidden
            using var remoteOriginReq = new HttpRequestMessage(HttpMethod.Get, "/api/status");
            remoteOriginReq.Headers.Add("Origin", "http://evil-attacker.com");
            var remoteOriginResp = await client.SendAsync(remoteOriginReq);
            Assert.Equal(HttpStatusCode.Forbidden, remoteOriginResp.StatusCode);

            // C.2 : Rejet d'un Host header falsifié (attaque DNS Rebinding) -> 400 Bad Request
            using var fakeHostReq = new HttpRequestMessage(HttpMethod.Get, "/api/status");
            fakeHostReq.Headers.Host = "attacker.dnsrebinding.com";
            var fakeHostResp = await client.SendAsync(fakeHostReq);
            Assert.Equal(HttpStatusCode.BadRequest, fakeHostResp.StatusCode);

            // C.3 : Parcours CSRF légitime complet
            // Étape 1 : Récupération du jeton CSRF par le client légitime via GET /api/csrf-token
            var tokenResp = await client.GetAsync("/api/csrf-token");
            Assert.Equal(HttpStatusCode.OK, tokenResp.StatusCode);
            var tokenJson = await tokenResp.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            string? token = tokenJson.GetProperty("csrfToken").GetString();
            Assert.NotNull(token);
            Assert.Equal(64, token.Length); // 256 bits = 64 caractères hex

            // Étape 2 : Tentative de POST sans jeton CSRF -> 403 Forbidden
            var postData = new System.Net.Http.StringContent(
                "{\"fullPath\":\"C:\\\\TestDir\",\"label\":\"Test\"}",
                System.Text.Encoding.UTF8,
                "application/json");
            var postWithoutTokenResp = await client.PostAsync("/api/roots", postData);
            Assert.Equal(HttpStatusCode.Forbidden, postWithoutTokenResp.StatusCode);

            // Étape 3 : Tentative de POST avec un jeton CSRF altéré/corrompu -> 403 Forbidden
            using var postCorruptedReq = new HttpRequestMessage(HttpMethod.Post, "/api/roots")
            {
                Content = new System.Net.Http.StringContent(
                    "{\"fullPath\":\"C:\\\\TestDir\",\"label\":\"Test\"}",
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
            postCorruptedReq.Headers.Add("X-GraphiScope-CSRF", "CORRUPTED_TOKEN_0123456789ABCDEF");
            var postCorruptedResp = await client.SendAsync(postCorruptedReq);
            Assert.Equal(HttpStatusCode.Forbidden, postCorruptedResp.StatusCode);

            // Étape 4 : Mutation légitime : POST avec jeton CSRF valide -> 201 Created
            using var postValidReq = new HttpRequestMessage(HttpMethod.Post, "/api/roots")
            {
                Content = new System.Net.Http.StringContent(
                    "{\"fullPath\":\"C:\\\\TestDir\",\"label\":\"Test\"}",
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
            postValidReq.Headers.Add("X-GraphiScope-CSRF", token);
            var postValidResp = await client.SendAsync(postValidReq);
            Assert.Equal(HttpStatusCode.Created, postValidResp.StatusCode);

            // Étape 5 : Vérification des autres méthodes mutatrices sans jeton (PUT, DELETE, PATCH) -> 403 Forbidden
            var putResp = await client.PutAsync("/api/roots", postData);
            Assert.Equal(HttpStatusCode.Forbidden, putResp.StatusCode);

            var deleteResp = await client.DeleteAsync("/api/roots");
            Assert.Equal(HttpStatusCode.Forbidden, deleteResp.StatusCode);

            var patchResp = await client.PatchAsync("/api/roots", postData);
            Assert.Equal(HttpStatusCode.Forbidden, patchResp.StatusCode);

            // Étape 5b : Vérification qu'une méthode non prévue / arbitraire est également bloquée (politique default-deny anti-contournement)
            using var unexpectedReq = new HttpRequestMessage(new HttpMethod("UNEXPECTED_MUTATE"), "/api/roots");
            var unexpectedResp = await client.SendAsync(unexpectedReq);
            Assert.Equal(HttpStatusCode.Forbidden, unexpectedResp.StatusCode);

            // Étape 6 : Vérification que l'ajout d'une racine SMB / UNC via POST avec jeton valide est bien accepté (Axe B)
            using var postSmbReq = new HttpRequestMessage(HttpMethod.Post, "/api/roots")
            {
                Content = new System.Net.Http.StringContent(
                    "{\"fullPath\":\"\\\\\\\\serveur-nas\\\\partage\\\\documents\",\"label\":\"Partage SMB\"}",
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
            postSmbReq.Headers.Add("X-GraphiScope-CSRF", token);
            var postSmbResp = await client.SendAsync(postSmbReq);
            Assert.Equal(HttpStatusCode.Created, postSmbResp.StatusCode);
        }
        finally
        {
            await app.StopAsync();
            SqliteConnection.ClearAllPools();
            try { Directory.Delete(tempDir, true); } catch { }
        }
    }
}

