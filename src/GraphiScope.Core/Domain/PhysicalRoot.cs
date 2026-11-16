namespace GraphiScope.Core.Domain;

/// <summary>
/// Racine physique de stockage supervisée par GraphiScope (V1.6.0).
/// </summary>
public sealed class PhysicalRoot
{
    public Guid RootId { get; set; } = Guid.NewGuid();
    public string FullPath { get; set; } = string.Empty;
    public string? Label { get; set; }
    public StorageMediumType MediumType { get; set; } = StorageMediumType.FIXED_DISK;
    public RootState State { get; set; } = RootState.ONLINE;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastScannedUtc { get; set; }

    public PhysicalRoot() { }

    public PhysicalRoot(string fullPath, string? label = null, StorageMediumType mediumType = StorageMediumType.FIXED_DISK)
    {
        FullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
        Label = label;
        MediumType = mediumType;
        State = RootState.ONLINE;
        CreatedUtc = DateTime.UtcNow;
    }
}

