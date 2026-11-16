using GraphiScope.Core.Domain;
using GraphiScope.Core.Persistence.Repositories;
using GraphiScope.Core.Security;
using GraphiScope.Core.Services;

namespace GraphiScope.Server.Endpoints;

/// <summary>
/// Points d'accès REST de gestion des racines et de pilotage d'inventaire (protégés par anti-CSRF).
/// </summary>
public static class InventoryEndpoints
{
    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. Consultation des racines (GET)
        app.MapGet("/api/roots", async (IStorageRootRepository repo, CancellationToken ct) =>
        {
            var roots = await repo.GetAllAsync(ct);
            return Results.Ok(roots);
        });

        // 2. Ajout d'une racine (POST, mutation - autorise racines locales, amovibles et partages réseau SMB/UNC)
        app.MapPost("/api/roots", async (AddRootRequest request, IStorageRootRepository repo, PathConfinementService confinement, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(request.FullPath))
                return Results.BadRequest(new { Error = "Le chemin de la racine est requis." });

            var mediumType = confinement.GetMediumType(request.FullPath);
            var root = new PhysicalRoot(request.FullPath, request.Label, mediumType);

            await repo.AddAsync(root, ct);
            return Results.Created($"/api/roots/{root.RootId}", root);
        });

        // 3. Lancement d'inventaire (POST, mutation)
        app.MapPost("/api/inventory/scan", async (ScanRequest request, IInventoryCoordinator coordinator, CancellationToken ct) =>
        {
            var result = await coordinator.ScanRootAsync(request.RootId, ct);
            return Results.Ok(result);
        });

        // 4. Lancement de réconciliation des doublons (POST, mutation, Q-003)
        app.MapPost("/api/inventory/resolve-duplicates", async (ScanRequest request, IInventoryCoordinator coordinator, CancellationToken ct) =>
        {
            int resolved = await coordinator.ResolveDuplicatesAsync(request.RootId, ct);
            return Results.Ok(new { RootId = request.RootId, IdentitiesResolved = resolved });
        });

        return app;
    }
}

public sealed record AddRootRequest(string FullPath, string? Label);
public sealed record ScanRequest(Guid RootId);

