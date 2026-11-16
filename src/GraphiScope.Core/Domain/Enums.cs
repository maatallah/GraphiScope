namespace GraphiScope.Core.Domain;

/// <summary>
/// États d'une racine de stockage physique supervisée (V1.6.0).
/// </summary>
public enum RootState
{
    ONLINE = 0,
    DISCONNECTED = 1,
    UNREACHABLE = 2,
    SCANNING = 3
}

/// <summary>
/// États d'observation matérielle d'un fichier physique sur le disque (V1.6.0).
/// </summary>
public enum PhysicalState
{
    DISCOVERED = 0,
    STABLE = 1,
    UNSTABLE = 2,
    MISSING = 3,
    DELETED_CONFIRMED = 4
}

/// <summary>
/// États d'une ressource documentaire logique pérenne (V1.6.0).
/// </summary>
public enum LogicalState
{
    ACTIVE = 0,
    ORPHANED_CONTENT = 1,
    ARCHIVED = 2,
    RESTRICTED = 3
}

/// <summary>
/// États de quarantaine et de sécurité d'un fichier (V1.6.0).
/// </summary>
public enum QuarantineState
{
    NONE = 0,
    QUARANTINED = 1,
    PENDING_DELETE = 2,
    TRASH_DELETED = 3,
    PERMANENTLY_DELETED = 4
}

/// <summary>
/// Types de supports physiques de stockage (Win32).
/// </summary>
public enum StorageMediumType
{
    FIXED_DISK = 0,
    REMOVABLE_DISK = 1,
    OPTICAL_DISC = 2,
    RAM_DISK = 3,
    NETWORK_SHARE = 4,
    UNKNOWN = 5
}

