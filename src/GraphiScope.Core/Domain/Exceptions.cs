namespace GraphiScope.Core.Domain;

/// <summary>
/// Levée lorsqu'une tentative d'hébergement ou d'accès sur stockage réseau distant est détectée pour SQLite.
/// </summary>
public sealed class NetworkStorageNotSupportedException : InvalidOperationException
{
    public NetworkStorageNotSupportedException(string message) : base(message) { }
    public NetworkStorageNotSupportedException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Levée lors d'une tentative d'accès non sécurisée ou d'un chemin malformé.
/// </summary>
public sealed class PathSecurityException : InvalidOperationException
{
    public PathSecurityException(string message) : base(message) { }
}

/// <summary>
/// Levée lorsqu'un volume ou une racine est indisponible ou déconnecté.
/// </summary>
public sealed class VolumeUnavailableException : IOException
{
    public VolumeUnavailableException(string message) : base(message) { }
}

/// <summary>
/// Levée en cas d'incohérence ou d'erreur structurelle détectée sur la base de données.
/// </summary>
public sealed class CorruptedDatabaseException : Exception
{
    public CorruptedDatabaseException(string message) : base(message) { }
}

