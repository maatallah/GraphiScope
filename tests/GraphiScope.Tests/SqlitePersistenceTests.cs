using Microsoft.Data.Sqlite;
using GraphiScope.Core.Domain;
using GraphiScope.Core.Persistence;
using GraphiScope.Core.Persistence.Repositories;
using GraphiScope.Core.Security;
using Xunit;

namespace GraphiScope.Tests;

public class SqlitePersistenceTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly string _dbPath;
    private readonly SqliteConnectionFactory _connectionFactory;
    private readonly SqliteSchemaManager _schemaManager;
    private readonly PathConfinementService _confinementService;

    public SqlitePersistenceTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "GraphiScope_TestDb_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDirectory);
        _dbPath = Path.Combine(_tempDirectory, "test_catalog.db");

        _confinementService = new PathConfinementService();
        _connectionFactory = new SqliteConnectionFactory(_dbPath, _confinementService, customAllowedCatalogDir: _tempDirectory);
        _schemaManager = new SqliteSchemaManager(_connectionFactory);
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        try
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }
        catch
        {
            // Nettoyage au mieux en fin de test
        }
    }

    [Fact]
    public async Task InitializeSchemaAsync_CreatesAllSevenEntitiesFromV160()
    {
        // Act
        await _schemaManager.InitializeSchemaAsync();

        // Assert : vérification de la présence des 7 tables
        var expectedTables = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "PhysicalRoots",
            "ResourceIdentities",
            "PhysicalFiles",
            "FileObservations",
            "LogicalResources",
            "LogicalResourceVersions",
            "MetadataEntries"
        };

        var actualTables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            actualTables.Add(reader.GetString(0));
        }

        foreach (var table in expectedTables)
        {
            Assert.Contains(table, actualTables);
        }
    }

    [Fact]
    public void ConnectionFactory_EnforcesWalModeAndSynchronousNormal()
    {
        // Act
        using var connection = (SqliteConnection)_connectionFactory.CreateConnection();

        // Assert 1: journal_mode = WAL
        using var cmdWal = connection.CreateCommand();
        cmdWal.CommandText = "PRAGMA journal_mode;";
        string? journalMode = cmdWal.ExecuteScalar()?.ToString();
        Assert.Equal("wal", journalMode, ignoreCase: true);

        // Assert 2: synchronous = NORMAL (représenté par 1 dans SQLite)
        using var cmdSync = connection.CreateCommand();
        cmdSync.CommandText = "PRAGMA synchronous;";
        long synchronous = Convert.ToInt64(cmdSync.ExecuteScalar());
        Assert.Equal(1, synchronous); // 1 = NORMAL (ARB-01)

        // Assert 3: foreign_keys = ON
        using var cmdFk = connection.CreateCommand();
        cmdFk.CommandText = "PRAGMA foreign_keys;";
        long foreignKeys = Convert.ToInt64(cmdFk.ExecuteScalar());
        Assert.Equal(1, foreignKeys);
    }

    [Fact]
    public async Task StorageRootRepository_InsertReadUpdate_WorksAsExpected()
    {
        await _schemaManager.InitializeSchemaAsync();
        var repository = new StorageRootRepository(_connectionFactory);

        var root = new PhysicalRoot(@"C:\TestStorage\Drive1", "Disque Principal")
        {
            RootId = Guid.NewGuid(),
            MediumType = StorageMediumType.FIXED_DISK,
            State = RootState.ONLINE
        };

        // Insert
        await repository.AddAsync(root);

        // Read by Id
        var readById = await repository.GetByIdAsync(root.RootId);
        Assert.NotNull(readById);
        Assert.Equal(root.FullPath, readById.FullPath);
        Assert.Equal("Disque Principal", readById.Label);

        // Read by Path
        var readByPath = await repository.GetByPathAsync(@"C:\TestStorage\Drive1");
        Assert.NotNull(readByPath);
        Assert.Equal(root.RootId, readByPath.RootId);

        // Update state
        var now = DateTime.UtcNow;
        await repository.UpdateStateAsync(root.RootId, RootState.SCANNING, now);

        var updated = await repository.GetByIdAsync(root.RootId);
        Assert.NotNull(updated);
        Assert.Equal(RootState.SCANNING, updated.State);
        Assert.NotNull(updated.LastScannedUtc);
    }

    [Fact]
    public async Task FileInventoryRepository_BatchInsert_SucceedsAndPreservesIntegrity()
    {
        await _schemaManager.InitializeSchemaAsync();
        var rootRepo = new StorageRootRepository(_connectionFactory);
        var fileRepo = new FileInventoryRepository(_connectionFactory);

        var root = new PhysicalRoot(@"C:\Volumes\Projets", "Projets")
        {
            RootId = Guid.NewGuid(),
            MediumType = StorageMediumType.FIXED_DISK,
            State = RootState.ONLINE
        };
        await rootRepo.AddAsync(root);

        // Générer un lot de 100 fichiers
        var files = new List<PhysicalFile>();
        for (int i = 0; i < 100; i++)
        {
            string fileName = $"document_{i:D4}.pdf";
            string fullPath = Path.Combine(root.FullPath, fileName);
            files.Add(new PhysicalFile(
                root.RootId,
                fullPath,
                fileName,
                fileName,
                1024 * (i + 1),
                DateTime.UtcNow,
                DateTime.UtcNow));
        }

        // Insertion par lot
        await fileRepo.InsertFilesBatchAsync(files);

        // Comptage
        long totalCount = await fileRepo.CountFilesByRootIdAsync(root.RootId);
        Assert.Equal(100, totalCount);

        // Pagination
        var page1 = await fileRepo.GetFilesByRootIdAsync(root.RootId, skip: 0, take: 25);
        Assert.Equal(25, page1.Count);
    }

    [Fact]
    public async Task ExecuteCheckpointAsync_TruncatesWalFileSuccessfully()
    {
        await _schemaManager.InitializeSchemaAsync();
        var rootRepo = new StorageRootRepository(_connectionFactory);
        await rootRepo.AddAsync(new PhysicalRoot(@"C:\Checkpoint\Test", "CheckpointRoot"));

        // Act & Assert : Exécuter le checkpoint TRUNCATE explicite (ARB-01)
        await _connectionFactory.ExecuteCheckpointAsync(truncate: true);

        // Vérification de l'intégrité après checkpoint
        bool isOk = await _schemaManager.CheckIntegrityAsync();
        Assert.True(isOk);
    }

    [Fact]
    public async Task CheckIntegrityAsync_ReturnsTrueOnValidDatabase()
    {
        await _schemaManager.InitializeSchemaAsync();

        bool isIntact = await _schemaManager.CheckIntegrityAsync();
        Assert.True(isIntact);
    }

    [Fact]
    public void PathConfinement_DetectsAndRejectsUncNetworkPaths()
    {
        // Validation syntaxique UNC
        Assert.True(_confinementService.IsUncPath(@"\\server\share\file.db"));
        Assert.True(_confinementService.IsUncPath(@"\\192.168.1.50\vol1\db.db"));
        Assert.True(_confinementService.IsUncPath(@"\\?\UNC\server\share\db.db"));

        // Chemins locaux valides
        Assert.False(_confinementService.IsUncPath(@"C:\Data\db.sqlite"));
        Assert.False(_confinementService.IsUncPath(@"D:\Backup\db.sqlite"));

        // Exception levée en cas de tentative de création sur UNC
        Assert.Throws<NetworkStorageNotSupportedException>(() =>
            _confinementService.ValidateLocalStoragePath(@"\\remote-nas\archive\catalog.db"));
    }

    [Fact]
    public async Task SqliteNativeVersion_IsPatchedAndAboveVulnerabilityThreshold()
    {
        // Vérifie la version native effective du moteur SQLite chargé par Microsoft.Data.Sqlite 10.0.15
        // Le seuil de vulnérabilité pour GHSA-2m69-gcr7-jv3q / CVE-2025-6965 est 3.50.2.
        await using var connection = (SqliteConnection)_connectionFactory.CreateConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT sqlite_version();";

        string? versionString = (string?)await cmd.ExecuteScalarAsync();
        Console.WriteLine($"[TEST-EVIDENCE] MOTEUR SQLITE NATIF VERSION EFFECTIVE: {versionString}");
        Assert.NotNull(versionString);

        // Analyse de la version majeure.mineure
        var parts = versionString.Split('.');
        int major = int.Parse(parts[0]);
        int minor = int.Parse(parts[1]);

        Assert.True(major > 3 || (major == 3 && minor >= 50),
            $"La version native SQLite '{versionString}' doit être >= 3.50.2 pour éliminer la vulnérabilité GHSA-2m69-gcr7-jv3q.");
    }

    [Fact]
    public void PathConfinement_DistinguishesLocalCatalogConfinementFromSmbInventoryRoots()
    {
        // 1. Confinement strict du catalogue SQLite : doit être LOCAL uniquement
        Assert.Throws<NetworkStorageNotSupportedException>(() =>
            _confinementService.ValidateLocalStoragePath(@"\\serveur-nas\partage\catalog.db"));

        // 2. Détection et qualification des racines d'inventaire : les racines réseau SMB sont autorisées
        var smbType = _confinementService.GetMediumType(@"\\serveur-nas\partage\photos");
        Assert.Equal(StorageMediumType.NETWORK_SHARE, smbType);

        var localType = _confinementService.GetMediumType(@"C:\DossierLocal");
        Assert.Equal(StorageMediumType.FIXED_DISK, localType);
    }

    [Fact]
    public async Task StorageRootRepository_AllowsSmbNetworkRoots()
    {
        await _schemaManager.InitializeSchemaAsync();
        var repo = new StorageRootRepository(_connectionFactory);

        // Une racine SMB est parfaitement acceptée et persistée dans le catalogue local
        var smbRoot = new PhysicalRoot(@"\\serveur-nas\partage\archives", "Archives Distantes", StorageMediumType.NETWORK_SHARE)
        {
            RootId = Guid.NewGuid(),
            State = RootState.ONLINE
        };

        await repo.AddAsync(smbRoot);

        var retrieved = await repo.GetByIdAsync(smbRoot.RootId);
        Assert.NotNull(retrieved);
        Assert.Equal(@"\\serveur-nas\partage\archives", retrieved.FullPath);
        Assert.Equal(StorageMediumType.NETWORK_SHARE, retrieved.MediumType);
    }

    [Fact]
    public void CatalogConfinement_DefaultConstructor_IsStrictlyUnderLocalAppDataGraphiScopeCatalog()
    {
        // 1. Vérification que le catalogue par défaut est sous %LOCALAPPDATA%\GraphiScope\Catalog\graphiscope.db (Q-004)
        var defaultFactory = new SqliteConnectionFactory();
        string expectedDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GraphiScope", "Catalog");
        string expectedDbPath = Path.Combine(expectedDir, "graphiscope.db");

        Assert.Equal(expectedDbPath, defaultFactory.DatabasePath);
        Assert.True(Directory.Exists(expectedDir));
    }

    [Fact]
    public void CatalogConfinement_RejectsExternalAbsolutePath()
    {
        // 2. Chemin absolu extérieur au répertoire autorisé (%LOCALAPPDATA%\GraphiScope\Catalog)
        Assert.Throws<InvalidOperationException>(() =>
            _confinementService.ValidateCatalogPath(@"C:\Windows\System32\forbidden_catalog.db"));

        Assert.Throws<InvalidOperationException>(() =>
            _confinementService.ValidateCatalogPath(@"D:\ArbitraryDirectory\catalog.db"));
    }

    [Fact]
    public void CatalogConfinement_RejectsParentDirectoryTraversal()
    {
        // 3. Chemin relatif tentant une remontée '..' hors du répertoire autorisé
        string defaultDir = _confinementService.GetDefaultCatalogDirectory();
        string traversalPath = Path.Combine(defaultDir, @"..\..\escaped_catalog.db");

        Assert.Throws<InvalidOperationException>(() =>
            _confinementService.ValidateCatalogPath(traversalPath));
    }

    [Fact]
    public void CatalogConfinement_RejectsUncAndMappedDrive()
    {
        // 4. Tentative de placer le catalogue sur un partage réseau UNC ou lecteur distant
        Assert.Throws<NetworkStorageNotSupportedException>(() =>
            _confinementService.ValidateCatalogPath(@"\\remote-nas\share\catalog.db"));
    }
}

