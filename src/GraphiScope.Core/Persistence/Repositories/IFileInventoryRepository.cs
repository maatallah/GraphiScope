using GraphiScope.Core.Domain;

namespace GraphiScope.Core.Persistence.Repositories;

/// <summary>
/// Contrat de persistance pour l'inventaire physique des fichiers et des identités de contenu.
/// </summary>
public interface IFileInventoryRepository
{
    Task InsertFilesBatchAsync(IEnumerable<PhysicalFile> files, CancellationToken ct = default);
    Task<PhysicalFile?> GetFileByIdAsync(Guid fileId, CancellationToken ct = default);
    Task<PhysicalFile?> GetFileByPathAsync(string fullPath, CancellationToken ct = default);
    Task<IReadOnlyList<PhysicalFile>> GetFilesByRootIdAsync(Guid rootId, int skip = 0, int take = 50, CancellationToken ct = default);
    Task<long> CountFilesByRootIdAsync(Guid rootId, CancellationToken ct = default);
    Task<IReadOnlyList<long>> GetDuplicateCandidateSizesAsync(Guid rootId, CancellationToken ct = default);
    Task<IReadOnlyList<PhysicalFile>> GetFilesBySizeAsync(Guid rootId, long sizeBytes, CancellationToken ct = default);
    Task UpdateFileIdentityAsync(Guid fileId, Guid identityId, PhysicalState state, CancellationToken ct = default);
    Task<ResourceIdentity?> GetIdentityByHashAsync(string sha256, long byteLength, CancellationToken ct = default);
    Task AddIdentityAsync(ResourceIdentity identity, CancellationToken ct = default);
    Task AddObservationAsync(FileObservation observation, CancellationToken ct = default);
}

