using System.Diagnostics;
using GraphiScope.Core.Domain;
using GraphiScope.Core.Persistence;
using GraphiScope.Core.Persistence.Repositories;

namespace GraphiScope.Core.Services;

/// <summary>
/// Orchestrateur de session d'inventaire combinant l'ingestion par lots et la résolution différée des doublons (Q-003, ARB-01).
/// </summary>
public sealed class InventoryCoordinator : IInventoryCoordinator
{
    private const int BatchSize = 1000; // Taille des lots fermés sous transaction

    private readonly IStorageRootRepository _rootRepository;
    private readonly IFileInventoryRepository _inventoryRepository;
    private readonly IFileSystemCrawler _crawler;
    private readonly IHashService _hashService;
    private readonly IDbConnectionFactory _connectionFactory;

    public InventoryCoordinator(
        IStorageRootRepository rootRepository,
        IFileInventoryRepository inventoryRepository,
        IFileSystemCrawler crawler,
        IHashService hashService,
        IDbConnectionFactory connectionFactory)
    {
        _rootRepository = rootRepository ?? throw new ArgumentNullException(nameof(rootRepository));
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        _crawler = crawler ?? throw new ArgumentNullException(nameof(crawler));
        _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<ScanResult> ScanRootAsync(Guid rootId, CancellationToken ct = default)
    {
        var root = await _rootRepository.GetByIdAsync(rootId, ct)
            ?? throw new ArgumentException($"Racine introuvable pour l'identifiant '{rootId}'.", nameof(rootId));

        await _rootRepository.UpdateStateAsync(rootId, RootState.SCANNING, null, ct);

        var stopwatch = Stopwatch.StartNew();
        long totalFiles = 0;
        long totalBytes = 0;
        var batch = new List<PhysicalFile>(BatchSize);

        try
        {
            await foreach (var file in _crawler.CrawlAsync(root, ct))
            {
                batch.Add(file);
                totalFiles++;
                totalBytes += file.FileSizeBytes;

                if (batch.Count >= BatchSize)
                {
                    await _inventoryRepository.InsertFilesBatchAsync(batch, ct);
                    batch.Clear();

                    // Checkpoint explicite périodique pour borner la fenêtre de perte en mode NORMAL
                    await _connectionFactory.ExecuteCheckpointAsync(truncate: true, ct);
                }
            }

            if (batch.Count > 0)
            {
                await _inventoryRepository.InsertFilesBatchAsync(batch, ct);
                batch.Clear();
            }

            // Checkpoint explicite final de fin de racine
            await _connectionFactory.ExecuteCheckpointAsync(truncate: true, ct);

            stopwatch.Stop();
            await _rootRepository.UpdateStateAsync(rootId, RootState.ONLINE, DateTime.UtcNow, ct);

            return new ScanResult(rootId, totalFiles, totalBytes, stopwatch.Elapsed, true);
        }
        catch (VolumeUnavailableException)
        {
            // Déconnexion matérielle ou réseau de la racine : passage à DISCONNECTED (sans altérer le catalogue)
            await _rootRepository.UpdateStateAsync(rootId, RootState.DISCONNECTED, null, ct);
            throw;
        }
        catch (UnauthorizedAccessException)
        {
            // Erreur de droits ou d'authentification réseau : passage à UNREACHABLE
            await _rootRepository.UpdateStateAsync(rootId, RootState.UNREACHABLE, null, ct);
            throw;
        }
        catch
        {
            await _rootRepository.UpdateStateAsync(rootId, RootState.UNREACHABLE, null, ct);
            throw;
        }
    }

    public async Task<int> ResolveDuplicatesAsync(Guid rootId, CancellationToken ct = default)
    {
        // 1. Respect strict de Q-003 : identification des tailles partagées par au moins 2 fichiers
        var candidateSizes = await _inventoryRepository.GetDuplicateCandidateSizesAsync(rootId, ct);
        int identitiesAssigned = 0;

        foreach (long size in candidateSizes)
        {
            ct.ThrowIfCancellationRequested();

            var files = await _inventoryRepository.GetFilesBySizeAsync(rootId, size, ct);
            if (files.Count < 2)
                continue;

            // Dictionnaire des empreintes par fichier
            var computedHashes = new Dictionary<Guid, (string Quick, string Sha256)>();

            foreach (var file in files)
            {
                if (!File.Exists(file.FullPath))
                    continue;

                // 2. Calcul du QuickHash (filtre négatif d'exclusion rapide)
                string? quick = await _hashService.ComputeQuickHashAsync(file.FullPath, ct);
                if (string.IsNullOrEmpty(quick))
                    continue;

                // 3. Calcul du SHA-256 complet uniquement sur les candidats de même taille
                string sha256 = await _hashService.ComputeSha256Async(file.FullPath, ct);

                computedHashes[file.FileId] = (quick, sha256);

                // 4. Recherche ou création de l'identité binaire
                var existingIdentity = await _inventoryRepository.GetIdentityByHashAsync(sha256, file.FileSizeBytes, ct);
                Guid identityId;

                if (existingIdentity == null)
                {
                    var newIdentity = new ResourceIdentity(sha256, file.FileSizeBytes, quick, null);
                    await _inventoryRepository.AddIdentityAsync(newIdentity, ct);
                    identityId = newIdentity.IdentityId;
                }
                else
                {
                    identityId = existingIdentity.IdentityId;
                }

                // 5. Association de l'identité à l'occurrence physique
                await _inventoryRepository.UpdateFileIdentityAsync(file.FileId, identityId, PhysicalState.STABLE, ct);
                identitiesAssigned++;
            }
        }

        // Checkpoint final après réconciliation
        await _connectionFactory.ExecuteCheckpointAsync(truncate: true, ct);
        return identitiesAssigned;
    }
}

