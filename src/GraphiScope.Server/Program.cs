using System.Net;
using GraphiScope.Core.Persistence;
using GraphiScope.Core.Persistence.Repositories;
using GraphiScope.Core.Security;
using GraphiScope.Core.Services;
using GraphiScope.Server.Endpoints;
using GraphiScope.Server.Security;

var builder = WebApplication.CreateBuilder(args);

// Confinement Kestrel strict (ARB-02)
builder.WebHost.ConfigureKestrel(options =>
{
    // Écoute exclusivement sur 127.0.0.1 sur port dynamique éphémère (port 0)
    options.Listen(IPAddress.Loopback, 0);

    // Limites de saturation Kestrel
    options.Limits.MaxRequestBodySize = 1024 * 1024; // 1 Mo
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(15);
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(120);
});

// Enregistrement des services Core et Sécurité
builder.Services.AddSingleton<PathConfinementService>();
builder.Services.AddSingleton<CsrfTokenProvider>();
builder.Services.AddSingleton<IDbConnectionFactory, SqliteConnectionFactory>();
builder.Services.AddSingleton<SqliteSchemaManager>();
builder.Services.AddScoped<IStorageRootRepository, StorageRootRepository>();
builder.Services.AddScoped<IFileInventoryRepository, FileInventoryRepository>();
builder.Services.AddScoped<IFileSystemCrawler, FileSystemCrawler>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IInventoryCoordinator, InventoryCoordinator>();

var app = builder.Build();

// Initialisation du schéma SQLite au démarrage
using (var scope = app.Services.CreateScope())
{
    var schemaManager = scope.ServiceProvider.GetRequiredService<SqliteSchemaManager>();
    await schemaManager.InitializeSchemaAsync();
}

// Pipeline des middlewares de sécurité HTTP
app.UseMiddleware<LocalHostValidationMiddleware>();
app.UseMiddleware<CsrfProtectionMiddleware>();

// Enregistrement des points d'accès
app.MapStatusEndpoints();
app.MapInventoryEndpoints();

app.Run();

// Classe partielle pour permettre le chargement par WebApplicationFactory dans GraphiScope.Tests
public partial class Program { }

