namespace GraphiScope.Core.Domain;

/// <summary>
/// Métadonnée structurée clé/valeur rattachée à une ressource logique (V1.6.0).
/// </summary>
public sealed class MetadataEntry
{
    public Guid MetadataId { get; set; } = Guid.NewGuid();
    public Guid LogicalId { get; set; }
    public string Namespace { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string ValueText { get; set; } = string.Empty;

    public MetadataEntry() { }

    public MetadataEntry(Guid logicalId, string ns, string key, string valueText)
    {
        LogicalId = logicalId;
        Namespace = ns ?? string.Empty;
        Key = key ?? throw new ArgumentNullException(nameof(key));
        ValueText = valueText ?? string.Empty;
    }
}

