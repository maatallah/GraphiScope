using System.Data.Common;
using Microsoft.Data.Sqlite;
using GraphiScope.Core.Domain;
using GraphiScope.Core.Security;

namespace GraphiScope.Core.Persistence;

/// <summary>
/// Fabrique de connexions SQLite appliquant le mode WAL et l'arbitrage souverain synchronous = NORMAL (ARB-01).
/// </summary>
public sealed class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _databasePath;
    private readonly string _connectionString;

    public string DatabasePath => _databasePath;

    public SqliteConnectionFactory(string? databasePath = null, PathConfinementService? confinementService = null, string? customAllowedCatalogDir = null)
    {
        confinementService ??= new PathConfinementService();

        string defaultCatalogDir = confinementService.GetDefaultCatalogDirectory();
        string allowedDir = customAllowedCatalogDir ?? defaultCatalogDir;

        if (string.IsNullOrWhiteSpace(databasePath))
        {
            Directory.CreateDirectory(defaultCatalogDir);
            _databasePath = Path.Combine(defaultCatalogDir, "graphiscope.db");
            confinementService.ValidateCatalogPath(_databasePath, defaultCatalogDir);
        }
        else
        {
            confinementService.ValidateCatalogPath(databasePath, allowedDir);
            string? dir = Path.GetDirectoryName(databasePath);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }
            _databasePath = Path.GetFullPath(databasePath);
        }

        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = _databasePath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Cache = SqliteCacheMode.Shared,
            DefaultTimeout = 5
        };

        _connectionString = builder.ToString();
    }

    public DbConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        // Configuration pragmas conforme à V1.6.0 et arbitrage ARB-01
        cmd.CommandText = @"
            PRAGMA journal_mode = WAL;
            PRAGMA synchronous = NORMAL;
            PRAGMA foreign_keys = ON;
            PRAGMA busy_timeout = 5000;
        ";
        cmd.ExecuteNonQuery();

        return connection;
    }

    public async Task ExecuteCheckpointAsync(bool truncate = true, CancellationToken ct = default)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(ct);

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = truncate
            ? "PRAGMA wal_checkpoint(TRUNCATE);"
            : "PRAGMA wal_checkpoint(PASSIVE);";

        await cmd.ExecuteNonQueryAsync(ct);
    }
}

