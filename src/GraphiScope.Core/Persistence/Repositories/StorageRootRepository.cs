using Microsoft.Data.Sqlite;
using GraphiScope.Core.Domain;

namespace GraphiScope.Core.Persistence.Repositories;

/// <summary>
/// Implémentation SQLite sécurisée pour la gestion des racines de stockage PhysicalRoot.
/// </summary>
public sealed class StorageRootRepository : IStorageRootRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public StorageRootRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<PhysicalRoot?> GetByIdAsync(Guid rootId, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT RootId, FullPath, Label, MediumType, State, CreatedUtc, LastScannedUtc
            FROM PhysicalRoots
            WHERE RootId = @RootId;
        ";
        cmd.Parameters.AddWithValue("@RootId", rootId.ToString());

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<PhysicalRoot?> GetByPathAsync(string fullPath, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT RootId, FullPath, Label, MediumType, State, CreatedUtc, LastScannedUtc
            FROM PhysicalRoots
            WHERE FullPath = @FullPath;
        ";
        cmd.Parameters.AddWithValue("@FullPath", fullPath);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<IReadOnlyList<PhysicalRoot>> GetAllAsync(CancellationToken ct = default)
    {
        var list = new List<PhysicalRoot>();
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT RootId, FullPath, Label, MediumType, State, CreatedUtc, LastScannedUtc
            FROM PhysicalRoots
            ORDER BY FullPath ASC;
        ";

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapFromReader(reader));
        }

        return list;
    }

    public async Task AddAsync(PhysicalRoot root, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(root);

        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO PhysicalRoots (RootId, FullPath, Label, MediumType, State, CreatedUtc, LastScannedUtc)
            VALUES (@RootId, @FullPath, @Label, @MediumType, @State, @CreatedUtc, @LastScannedUtc);
        ";
        cmd.Parameters.AddWithValue("@RootId", root.RootId.ToString());
        cmd.Parameters.AddWithValue("@FullPath", root.FullPath);
        cmd.Parameters.AddWithValue("@Label", (object?)root.Label ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@MediumType", (int)root.MediumType);
        cmd.Parameters.AddWithValue("@State", (int)root.State);
        cmd.Parameters.AddWithValue("@CreatedUtc", root.CreatedUtc.ToString("o"));
        cmd.Parameters.AddWithValue("@LastScannedUtc", root.LastScannedUtc.HasValue ? root.LastScannedUtc.Value.ToString("o") : DBNull.Value);

        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task UpdateStateAsync(Guid rootId, RootState state, DateTime? lastScannedUtc = null, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();

        if (lastScannedUtc.HasValue)
        {
            cmd.CommandText = @"
                UPDATE PhysicalRoots
                SET State = @State, LastScannedUtc = @LastScannedUtc
                WHERE RootId = @RootId;
            ";
            cmd.Parameters.AddWithValue("@LastScannedUtc", lastScannedUtc.Value.ToString("o"));
        }
        else
        {
            cmd.CommandText = @"
                UPDATE PhysicalRoots
                SET State = @State
                WHERE RootId = @RootId;
            ";
        }

        cmd.Parameters.AddWithValue("@RootId", rootId.ToString());
        cmd.Parameters.AddWithValue("@State", (int)state);

        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task DeleteAsync(Guid rootId, CancellationToken ct = default)
    {
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM PhysicalRoots WHERE RootId = @RootId;";
        cmd.Parameters.AddWithValue("@RootId", rootId.ToString());

        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static PhysicalRoot MapFromReader(SqliteDataReader reader)
    {
        return new PhysicalRoot
        {
            RootId = Guid.Parse(reader.GetString(0)),
            FullPath = reader.GetString(1),
            Label = reader.IsDBNull(2) ? null : reader.GetString(2),
            MediumType = (StorageMediumType)reader.GetInt32(3),
            State = (RootState)reader.GetInt32(4),
            CreatedUtc = DateTime.Parse(reader.GetString(5)),
            LastScannedUtc = reader.IsDBNull(6) ? null : DateTime.Parse(reader.GetString(6))
        };
    }
}

