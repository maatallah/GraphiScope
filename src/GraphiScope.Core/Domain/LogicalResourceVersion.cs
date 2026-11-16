namespace GraphiScope.Core.Domain;

/// <summary>
/// Historique temporel liant une ressource logique à ses identités de contenu successives (V1.6.0).
/// </summary>
public sealed class LogicalResourceVersion
{
    public Guid VersionId { get; set; } = Guid.NewGuid();
    public Guid LogicalId { get; set; }
    public Guid IdentityId { get; set; }
    public int VersionNumber { get; set; } = 1;
    public DateTime AttachedUtc { get; set; } = DateTime.UtcNow;
    public string? ChangeComment { get; set; }

    public LogicalResourceVersion() { }

    public LogicalResourceVersion(Guid logicalId, Guid identityId, int versionNumber = 1, string? changeComment = null)
    {
        LogicalId = logicalId;
        IdentityId = identityId;
        VersionNumber = versionNumber;
        AttachedUtc = DateTime.UtcNow;
        ChangeComment = changeComment;
    }
}

