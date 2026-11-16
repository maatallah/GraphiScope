using Microsoft.Data.Sqlite;
using GraphiScope.Core.Domain;

namespace GraphiScope.Core.Persistence;

/// <summary>
/// Gestionnaire DDL responsable du déploiement et du contrôle d'intégrité du schéma SQLite (V1.6.0).
/// </summary>
public sealed class SqliteSchemaManager
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqliteSchemaManager(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    /// <summary>
    /// Initialise le schéma complet à 7 entités de V1.6.0 de façon idempotente (IF NOT EXISTS).
    /// </summary>
    public async Task InitializeSchemaAsync(CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync(ct);

        const string ddl = @"
            -- 1. Racines de stockage supervisées
            CREATE TABLE IF NOT EXISTS PhysicalRoots (
                RootId TEXT PRIMARY KEY,
                FullPath TEXT NOT NULL UNIQUE,
                Label TEXT NULL,
                MediumType INTEGER NOT NULL,
                State INTEGER NOT NULL,
                CreatedUtc TEXT NOT NULL,
                LastScannedUtc TEXT NULL
            );

            -- 2. Identité de contenu binaire (Niveau 2)
            CREATE TABLE IF NOT EXISTS ResourceIdentities (
                IdentityId TEXT PRIMARY KEY,
                PrimaryHashSha256 TEXT NOT NULL,
                QuickHash TEXT NULL,
                ByteLength INTEGER NOT NULL,
                MimeType TEXT NULL,
                FirstIdentifiedUtc TEXT NOT NULL,
                CONSTRAINT UQ_Identities_Hash_Size UNIQUE (PrimaryHashSha256, ByteLength)
            );

            -- 3. Occurrences physiques sur disque (Niveau 1)
            CREATE TABLE IF NOT EXISTS PhysicalFiles (
                FileId TEXT PRIMARY KEY,
                RootId TEXT NOT NULL,
                FullPath TEXT NOT NULL UNIQUE,
                RelativePath TEXT NOT NULL,
                FileName TEXT NOT NULL,
                Extension TEXT NULL,
                FileSizeBytes INTEGER NOT NULL,
                CreatedUtc TEXT NOT NULL,
                LastModifiedUtc TEXT NOT NULL,
                IdentityId TEXT NULL,
                State INTEGER NOT NULL,
                Quarantine INTEGER NOT NULL,
                FileAttributes INTEGER NOT NULL,
                FOREIGN KEY (RootId) REFERENCES PhysicalRoots(RootId) ON DELETE CASCADE,
                FOREIGN KEY (IdentityId) REFERENCES ResourceIdentities(IdentityId) ON DELETE SET NULL
            );

            -- 4. Traces temporelles d'observation
            CREATE TABLE IF NOT EXISTS FileObservations (
                ObservationId TEXT PRIMARY KEY,
                FileId TEXT NOT NULL,
                ObservedUtc TEXT NOT NULL,
                ObservedSizeBytes INTEGER NOT NULL,
                ObservedLastModifiedUtc TEXT NOT NULL,
                ObservedState INTEGER NOT NULL,
                FOREIGN KEY (FileId) REFERENCES PhysicalFiles(FileId) ON DELETE CASCADE
            );

            -- 5. Ressources documentaires logiques pérennes (Niveau 3)
            CREATE TABLE IF NOT EXISTS LogicalResources (
                LogicalId TEXT PRIMARY KEY,
                Title TEXT NOT NULL,
                State INTEGER NOT NULL,
                CurrentIdentityId TEXT NULL,
                PreferredPhysicalFileId TEXT NULL,
                IsManualOverride INTEGER NOT NULL DEFAULT 0,
                CreatedUtc TEXT NOT NULL,
                LastUpdatedUtc TEXT NOT NULL,
                FOREIGN KEY (CurrentIdentityId) REFERENCES ResourceIdentities(IdentityId) ON DELETE SET NULL,
                FOREIGN KEY (PreferredPhysicalFileId) REFERENCES PhysicalFiles(FileId) ON DELETE SET NULL
            );

            -- 6. Historique des versions documentaires
            CREATE TABLE IF NOT EXISTS LogicalResourceVersions (
                VersionId TEXT PRIMARY KEY,
                LogicalId TEXT NOT NULL,
                IdentityId TEXT NOT NULL,
                VersionNumber INTEGER NOT NULL,
                AttachedUtc TEXT NOT NULL,
                ChangeComment TEXT NULL,
                FOREIGN KEY (LogicalId) REFERENCES LogicalResources(LogicalId) ON DELETE CASCADE,
                FOREIGN KEY (IdentityId) REFERENCES ResourceIdentities(IdentityId) ON DELETE RESTRICT,
                CONSTRAINT UQ_Logical_Version UNIQUE (LogicalId, VersionNumber)
            );

            -- 7. Métadonnées structurées clé/valeur
            CREATE TABLE IF NOT EXISTS MetadataEntries (
                MetadataId TEXT PRIMARY KEY,
                LogicalId TEXT NOT NULL,
                Namespace TEXT NOT NULL,
                Key TEXT NOT NULL,
                ValueText TEXT NOT NULL,
                FOREIGN KEY (LogicalId) REFERENCES LogicalResources(LogicalId) ON DELETE CASCADE,
                CONSTRAINT UQ_Metadata_Key UNIQUE (LogicalId, Namespace, Key)
            );

            -- Index d'accélération
            CREATE INDEX IF NOT EXISTS IX_PhysicalFiles_RootId ON PhysicalFiles(RootId);
            CREATE INDEX IF NOT EXISTS IX_PhysicalFiles_IdentityId ON PhysicalFiles(IdentityId);
            CREATE INDEX IF NOT EXISTS IX_PhysicalFiles_Size ON PhysicalFiles(FileSizeBytes);
            CREATE INDEX IF NOT EXISTS IX_ResourceIdentities_Sha256 ON ResourceIdentities(PrimaryHashSha256);
            CREATE INDEX IF NOT EXISTS IX_ResourceIdentities_QuickHash ON ResourceIdentities(QuickHash);
            CREATE INDEX IF NOT EXISTS IX_FileObservations_FileId ON FileObservations(FileId);
        ";

        await using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = ddl;
        await cmd.ExecuteNonQueryAsync(ct);

        await transaction.CommitAsync(ct);
    }

    /// <summary>
    /// Exécute un contrôle d'intégrité complet de la base SQLite (PRAGMA integrity_check).
    /// </summary>
    public async Task<bool> CheckIntegrityAsync(CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "PRAGMA integrity_check;";

        var result = (string?)await cmd.ExecuteScalarAsync(ct);
        if (result != "ok")
        {
            throw new CorruptedDatabaseException($"Contrôle d'intégrité SQLite échoué : {result}");
        }

        return true;
    }
}

