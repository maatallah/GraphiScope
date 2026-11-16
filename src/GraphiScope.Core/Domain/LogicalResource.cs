namespace GraphiScope.Core.Domain;

/// <summary>
/// Ressource documentaire pérenne pour l'utilisateur, survivant aux renommages et déplacements (Niveau 3 de V1.6.0).
/// </summary>
public sealed class LogicalResource
{
    public Guid LogicalId { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public LogicalState State { get; set; } = LogicalState.ACTIVE;
    public Guid? CurrentIdentityId { get; set; }
    public Guid? PreferredPhysicalFileId { get; set; }
    public bool IsManualOverride { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;

    public LogicalResource() { }

    public LogicalResource(string title, Guid? identityId = null, Guid? preferredPhysicalFileId = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        CurrentIdentityId = identityId;
        PreferredPhysicalFileId = preferredPhysicalFileId;
        State = LogicalState.ACTIVE;
        CreatedUtc = DateTime.UtcNow;
        LastUpdatedUtc = DateTime.UtcNow;
    }
}

