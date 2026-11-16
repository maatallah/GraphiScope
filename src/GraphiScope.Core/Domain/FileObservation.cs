namespace GraphiScope.Core.Domain;

/// <summary>
/// Enregistrement factuel d'observation d'un fichier physique lors d'un cycle de scan (V1.6.0).
/// </summary>
public sealed class FileObservation
{
    public Guid ObservationId { get; set; } = Guid.NewGuid();
    public Guid FileId { get; set; }
    public DateTime ObservedUtc { get; set; } = DateTime.UtcNow;
    public long ObservedSizeBytes { get; set; }
    public DateTime ObservedLastModifiedUtc { get; set; }
    public PhysicalState ObservedState { get; set; }

    public FileObservation() { }

    public FileObservation(Guid fileId, long observedSizeBytes, DateTime observedLastModifiedUtc, PhysicalState observedState)
    {
        FileId = fileId;
        ObservedSizeBytes = observedSizeBytes;
        ObservedLastModifiedUtc = observedLastModifiedUtc;
        ObservedState = observedState;
        ObservedUtc = DateTime.UtcNow;
    }
}

