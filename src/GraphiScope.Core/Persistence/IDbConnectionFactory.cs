using System.Data.Common;

namespace GraphiScope.Core.Persistence;

/// <summary>
/// Contrat de création et d'administration des connexions SQLite pour GraphiScope (V1.6.0).
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Chemin absolu du fichier de base de données SQLite local.
    /// </summary>
    string DatabasePath { get; }

    /// <summary>
    /// Ouvre et configure une nouvelle connexion SQLite conforme aux directives souveraines.
    /// </summary>
    DbConnection CreateConnection();

    /// <summary>
    /// Déclenche un checkpoint WAL explicite sur le fichier journal.
    /// </summary>
    /// <param name="truncate">Si true, applique TRUNCATE pour remettre le journal à zéro octet.</param>
    /// <param name="ct">Jeton d'annulation.</param>
    Task ExecuteCheckpointAsync(bool truncate = true, CancellationToken ct = default);
}

