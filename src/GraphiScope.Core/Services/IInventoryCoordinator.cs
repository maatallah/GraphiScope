using GraphiScope.Core.Domain;

namespace GraphiScope.Core.Services;

/// <summary>
/// Contrat d'orchestration et de suivi des sessions d'inventaire physique et de réconciliation de doublons.
/// </summary>
public interface IInventoryCoordinator
{
    /// <summary>
    /// Démarre une session d'inventaire complet sur une racine supervisée.
    /// </summary>
    Task<ScanResult> ScanRootAsync(Guid rootId, CancellationToken ct = default);

    /// <summary>
    /// Exécute la passe de résolution différée des doublons (respect de Q-003 : calcul SHA-256 uniquement sur les fichiers de même taille).
    /// </summary>
    Task<int> ResolveDuplicatesAsync(Guid rootId, CancellationToken ct = default);
}

/// <summary>
/// Résultat synthétique d'une passe d'inventaire physique.
/// </summary>
public sealed record ScanResult(
    Guid RootId,
    long FilesDiscovered,
    long TotalBytesDiscovered,
    TimeSpan ElapsedDuration,
    bool CompletedSuccessfully
);

