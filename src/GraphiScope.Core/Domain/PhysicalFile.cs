namespace GraphiScope.Core.Domain;

/// <summary>
/// Occurrence matérielle d'un fichier physique sur le disque (Niveau 1 de V1.6.0).
/// </summary>
public sealed class PhysicalFile
{
    public Guid FileId { get; set; } = Guid.NewGuid();
    public Guid RootId { get; set; }
    public string FullPath { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string? Extension { get; set; }
    public long FileSizeBytes { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime LastModifiedUtc { get; set; }
    public Guid? IdentityId { get; set; }
    public PhysicalState State { get; set; } = PhysicalState.DISCOVERED;
    public QuarantineState Quarantine { get; set; } = QuarantineState.NONE;
    public uint FileAttributes { get; set; }

    public PhysicalFile() { }

    public PhysicalFile(Guid rootId, string fullPath, string relativePath, string fileName, long sizeBytes, DateTime createdUtc, DateTime lastModifiedUtc, uint attributes = 0)
    {
        RootId = rootId;
        FullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
        RelativePath = relativePath ?? string.Empty;
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        Extension = Path.GetExtension(fileName);
        FileSizeBytes = sizeBytes;
        CreatedUtc = createdUtc;
        LastModifiedUtc = lastModifiedUtc;
        FileAttributes = attributes;
        State = PhysicalState.DISCOVERED;
        Quarantine = QuarantineState.NONE;
    }
}

