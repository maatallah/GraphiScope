namespace GraphiScope.Core.Domain;

/// <summary>
/// Identité binaire intrinsèque immuable d'un contenu par empreinte cryptographique SHA-256 (Niveau 2 de V1.6.0).
/// </summary>
public sealed class ResourceIdentity
{
    public Guid IdentityId { get; set; } = Guid.NewGuid();
    public string PrimaryHashSha256 { get; set; } = string.Empty;
    public string? QuickHash { get; set; }
    public long ByteLength { get; set; }
    public string? MimeType { get; set; }
    public DateTime FirstIdentifiedUtc { get; set; } = DateTime.UtcNow;

    public ResourceIdentity() { }

    public ResourceIdentity(string sha256, long byteLength, string? quickHash = null, string? mimeType = null)
    {
        PrimaryHashSha256 = sha256?.ToUpperInvariant() ?? throw new ArgumentNullException(nameof(sha256));
        ByteLength = byteLength;
        QuickHash = quickHash?.ToUpperInvariant();
        MimeType = mimeType;
        FirstIdentifiedUtc = DateTime.UtcNow;
    }
}

