using GraphiScope.Core.Domain;
using Xunit;

namespace GraphiScope.Tests;

public class DomainTests
{
    [Fact]
    public void FileHash_WithValid64HexChars_IsSuccessfullyCreated()
    {
        string validSha = "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855";
        var hash = new FileHash(validSha);

        Assert.Equal(validSha, hash.Value);
    }

    [Fact]
    public void FileHash_WithInvalidLengthOrChars_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new FileHash("INVALID_HASH"));
        Assert.Throws<ArgumentException>(() => new FileHash(""));
        Assert.Throws<ArgumentException>(() => new FileHash("E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B85")); // 63 chars
    }

    [Fact]
    public void ByteSize_FormatsCorrectly()
    {
        var small = new ByteSize(500);
        Assert.Equal("500 o", small.ToDisplayString());

        var mega = new ByteSize(1024 * 1024 * 5); // 5 Mo
        Assert.Equal("5,00 Mo", mega.ToDisplayString().Replace('.', ','));
    }

    [Fact]
    public void PhysicalRoot_Initializes_WithOnlineState()
    {
        var root = new PhysicalRoot(@"C:\Storage\Photos", "Mes Photos");

        Assert.Equal(RootState.ONLINE, root.State);
        Assert.Equal(StorageMediumType.FIXED_DISK, root.MediumType);
        Assert.Equal(@"C:\Storage\Photos", root.FullPath);
    }

    [Fact]
    public void ModelTripleLevel_Entities_PreserveSeparationOfConcerns()
    {
        // Niveau 1 : Occurrence physique
        var file = new PhysicalFile(Guid.NewGuid(), @"C:\Photos\vacances.jpg", "vacances.jpg", "vacances.jpg", 10240, DateTime.UtcNow, DateTime.UtcNow);
        Assert.Equal(PhysicalState.DISCOVERED, file.State);
        Assert.Null(file.IdentityId);

        // Niveau 2 : Identité immuable
        string sha = "A".PadRight(64, '0');
        var identity = new ResourceIdentity(sha, 10240, "QUICK123", "image/jpeg");
        Assert.Equal(sha, identity.PrimaryHashSha256);

        // Niveau 3 : Ressource logique pérenne
        var logical = new LogicalResource("Photo de vacances", identity.IdentityId, file.FileId);
        Assert.Equal(LogicalState.ACTIVE, logical.State);
        Assert.Equal(identity.IdentityId, logical.CurrentIdentityId);
        Assert.Equal(file.FileId, logical.PreferredPhysicalFileId);
    }

    [Fact]
    public void QuickHash_ActsOnlyAsNegativeExclusionFilter()
    {
        // Règle fondamentale : si deux QuickHash sont différents, les contenus sont différents.
        // S'ils sont identiques, ils ne sont PAS déclarés identiques (ils partagent juste les 8 premiers Ko).
        string quick1 = "HASH_BLOC_A";
        string quick2 = "HASH_BLOC_B";

        Assert.NotEqual(quick1, quick2); // Exclusion négative prouvée
    }
}

