using GraphiScope.Core.Domain;

namespace GraphiScope.Core.Persistence.Repositories;

/// <summary>
/// Contrat de persistance pour les racines de stockage supervisées (PhysicalRoot).
/// </summary>
public interface IStorageRootRepository
{
    Task<PhysicalRoot?> GetByIdAsync(Guid rootId, CancellationToken ct = default);
    Task<PhysicalRoot?> GetByPathAsync(string fullPath, CancellationToken ct = default);
    Task<IReadOnlyList<PhysicalRoot>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(PhysicalRoot root, CancellationToken ct = default);
    Task UpdateStateAsync(Guid rootId, RootState state, DateTime? lastScannedUtc = null, CancellationToken ct = default);
    Task DeleteAsync(Guid rootId, CancellationToken ct = default);
}

