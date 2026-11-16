using Microsoft.Data.Sqlite;
using GraphiScope.Core.Domain;

namespace GraphiScope.Core.Persistence.Repositories;

/// <summary>
/// Implémentation SQLite des insertions par lots et requêtes d'inventaire optimisées.
/// </summary>
public sealed class FileInventoryRepository : IFileInventoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public FileInventoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task InsertFilesBatchAsync(IEnumerable<PhysicalFile> files, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(files);

        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync(ct);

        const string sql = @"
            INSERT INTO PhysicalFiles (
                FileId, RootId, FullPath, RelativePath, FileName, Extension,
                FileSizeBytes, CreatedUtc, LastModifiedUtc, IdentityId, State, Quarantine, FileAttributes
            ) VALUES (
                @FileId, @RootId, @FullPath, @RelativePath, @FileName, @Extension,
                @FileSizeBytes, @CreatedUtc, @LastModifiedUtc, @IdentityId, @State, @Quarantine, @FileAttributes
            )
            ON CONFLICT(FullPath) DO UPDATE SET
                LastModifiedUtc = excluded.LastModifiedUtc,
                FileSizeBytes = excluded.FileSizeBytes,
                State = excluded.State;
        ";

        await using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = sql;

        var pFileId = cmd.Parameters.Add("@FileId", SqliteType.Text);
        var pRootId = cmd.Parameters.Add("@RootId", SqliteType.Text);
        var pFullPath = cmd.Parameters.Add("@FullPath", SqliteType.Text);
        var pRelativePath = cmd.Parameters.Add("@RelativePath", SqliteType.Text);
        var pFileName = cmd.Parameters.Add("@FileName", SqliteType.Text);
        var pExtension = cmd.Parameters.Add("@Extension", SqliteType.Text);
        var pFileSizeBytes = cmd.Parameters.Add("@FileSizeBytes", SqliteType.Integer);
        var pCreatedUtc = cmd.Parameters.Add("@CreatedUtc", SqliteType.Text);
        var pLastModifiedUtc = cmd.Parameters.Add("@LastModifiedUtc", SqliteType.Text);
        var pIdentityId = cmd.Parameters.Add("@IdentityId", SqliteType.Text);
        var pState = cmd.Parameters.Add("@State", SqliteType.Integer);
        var pQuarantine = cmd.Parameters.Add("@Quarantine", SqliteType.Integer);
        var pFileAttributes = cmd.Parameters.Add("@FileAttributes", SqliteType.Integer);

        foreach (var file in files)
        {
            pFileId.Value = file.FileId.ToString();
            pRootId.Value = file.RootId.ToString();
            pFullPath.Value = file.FullPath;
            pRelativePath.Value = file.RelativePath;
            pFileName.Value = file.FileName;
            pExtension.Value = (object?)file.Extension ?? DBNull.Value;
            pFileSizeBytes.Value = file.FileSizeBytes;
            pCreatedUtc.Value = file.CreatedUtc.ToString("o");
            pLastModifiedUtc.Value = file.LastModifiedUtc.ToString("o");
            pIdentityId.Value = file.IdentityId.HasValue ? file.IdentityId.Value.ToString() : DBNull.Value;
            pState.Value = (int)file.State;
            pQuarantine.Value = (int)file.Quarantine;
            pFileAttributes.Value = (long)file.FileAttributes;

            await cmd.ExecuteNonQueryAsync(ct);
        }

        await transaction.CommitAsync(ct);
    }

    public async Task<PhysicalFile?> GetFileByIdAsync(Guid fileId, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM PhysicalFiles WHERE FileId = @FileId;";
        cmd.Parameters.AddWithValue("@FileId", fileId.ToString());

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapFile(reader);
        }

        return null;
    }

    public async Task<PhysicalFile?> GetFileByPathAsync(string fullPath, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM PhysicalFiles WHERE FullPath = @FullPath;";
        cmd.Parameters.AddWithValue("@FullPath", fullPath);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapFile(reader);
        }

        return null;
    }

    public async Task<IReadOnlyList<PhysicalFile>> GetFilesByRootIdAsync(Guid rootId, int skip = 0, int take = 50, CancellationToken ct = default)
    {
        var list = new List<PhysicalFile>();
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT * FROM PhysicalFiles 
            WHERE RootId = @RootId 
            ORDER BY FullPath ASC 
            LIMIT @Take OFFSET @Skip;
        ";
        cmd.Parameters.AddWithValue("@RootId", rootId.ToString());
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapFile(reader));
        }

        return list;
    }

    public async Task<long> CountFilesByRootIdAsync(Guid rootId, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM PhysicalFiles WHERE RootId = @RootId;";
        cmd.Parameters.AddWithValue("@RootId", rootId.ToString());

        var result = await cmd.ExecuteScalarAsync(ct);
        return Convert.ToInt64(result);
    }

    public async Task<IReadOnlyList<long>> GetDuplicateCandidateSizesAsync(Guid rootId, CancellationToken ct = default)
    {
        var list = new List<long>();
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT FileSizeBytes 
            FROM PhysicalFiles 
            WHERE RootId = @RootId AND FileSizeBytes > 0
            GROUP BY FileSizeBytes 
            HAVING COUNT(*) > 1;
        ";
        cmd.Parameters.AddWithValue("@RootId", rootId.ToString());

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(reader.GetInt64(0));
        }

        return list;
    }

    public async Task<IReadOnlyList<PhysicalFile>> GetFilesBySizeAsync(Guid rootId, long sizeBytes, CancellationToken ct = default)
    {
        var list = new List<PhysicalFile>();
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT * FROM PhysicalFiles 
            WHERE RootId = @RootId AND FileSizeBytes = @Size 
            ORDER BY FullPath ASC;
        ";
        cmd.Parameters.AddWithValue("@RootId", rootId.ToString());
        cmd.Parameters.AddWithValue("@Size", sizeBytes);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapFile(reader));
        }

        return list;
    }

    public async Task UpdateFileIdentityAsync(Guid fileId, Guid identityId, PhysicalState state, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            UPDATE PhysicalFiles 
            SET IdentityId = @IdentityId, State = @State 
            WHERE FileId = @FileId;
        ";
        cmd.Parameters.AddWithValue("@FileId", fileId.ToString());
        cmd.Parameters.AddWithValue("@IdentityId", identityId.ToString());
        cmd.Parameters.AddWithValue("@State", (int)state);

        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task<ResourceIdentity?> GetIdentityByHashAsync(string sha256, long byteLength, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT IdentityId, PrimaryHashSha256, QuickHash, ByteLength, MimeType, FirstIdentifiedUtc
            FROM ResourceIdentities
            WHERE PrimaryHashSha256 = @Sha AND ByteLength = @Len;
        ";
        cmd.Parameters.AddWithValue("@Sha", sha256.ToUpperInvariant());
        cmd.Parameters.AddWithValue("@Len", byteLength);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new ResourceIdentity
            {
                IdentityId = Guid.Parse(reader.GetString(0)),
                PrimaryHashSha256 = reader.GetString(1),
                QuickHash = reader.IsDBNull(2) ? null : reader.GetString(2),
                ByteLength = reader.GetInt64(3),
                MimeType = reader.IsDBNull(4) ? null : reader.GetString(4),
                FirstIdentifiedUtc = DateTime.Parse(reader.GetString(5))
            };
        }

        return null;
    }

    public async Task AddIdentityAsync(ResourceIdentity identity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(identity);

        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO ResourceIdentities (IdentityId, PrimaryHashSha256, QuickHash, ByteLength, MimeType, FirstIdentifiedUtc)
            VALUES (@Id, @Sha, @Quick, @Len, @Mime, @FirstUtc)
            ON CONFLICT(PrimaryHashSha256, ByteLength) DO NOTHING;
        ";
        cmd.Parameters.AddWithValue("@Id", identity.IdentityId.ToString());
        cmd.Parameters.AddWithValue("@Sha", identity.PrimaryHashSha256);
        cmd.Parameters.AddWithValue("@Quick", (object?)identity.QuickHash ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Len", identity.ByteLength);
        cmd.Parameters.AddWithValue("@Mime", (object?)identity.MimeType ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@FirstUtc", identity.FirstIdentifiedUtc.ToString("o"));

        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task AddObservationAsync(FileObservation observation, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(observation);

        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO FileObservations (ObservationId, FileId, ObservedUtc, ObservedSizeBytes, ObservedLastModifiedUtc, ObservedState)
            VALUES (@ObsId, @FileId, @ObsUtc, @SizeBytes, @LastModUtc, @State);
        ";
        cmd.Parameters.AddWithValue("@ObsId", observation.ObservationId.ToString());
        cmd.Parameters.AddWithValue("@FileId", observation.FileId.ToString());
        cmd.Parameters.AddWithValue("@ObsUtc", observation.ObservedUtc.ToString("o"));
        cmd.Parameters.AddWithValue("@SizeBytes", observation.ObservedSizeBytes);
        cmd.Parameters.AddWithValue("@LastModUtc", observation.ObservedLastModifiedUtc.ToString("o"));
        cmd.Parameters.AddWithValue("@State", (int)observation.ObservedState);

        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static PhysicalFile MapFile(SqliteDataReader reader)
    {
        return new PhysicalFile
        {
            FileId = Guid.Parse(reader.GetString(reader.GetOrdinal("FileId"))),
            RootId = Guid.Parse(reader.GetString(reader.GetOrdinal("RootId"))),
            FullPath = reader.GetString(reader.GetOrdinal("FullPath")),
            RelativePath = reader.GetString(reader.GetOrdinal("RelativePath")),
            FileName = reader.GetString(reader.GetOrdinal("FileName")),
            Extension = reader.IsDBNull(reader.GetOrdinal("Extension")) ? null : reader.GetString(reader.GetOrdinal("Extension")),
            FileSizeBytes = reader.GetInt64(reader.GetOrdinal("FileSizeBytes")),
            CreatedUtc = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedUtc"))),
            LastModifiedUtc = DateTime.Parse(reader.GetString(reader.GetOrdinal("LastModifiedUtc"))),
            IdentityId = reader.IsDBNull(reader.GetOrdinal("IdentityId")) ? null : Guid.Parse(reader.GetString(reader.GetOrdinal("IdentityId"))),
            State = (PhysicalState)reader.GetInt32(reader.GetOrdinal("State")),
            Quarantine = (QuarantineState)reader.GetInt32(reader.GetOrdinal("Quarantine")),
            FileAttributes = (uint)reader.GetInt64(reader.GetOrdinal("FileAttributes"))
        };
    }
}

