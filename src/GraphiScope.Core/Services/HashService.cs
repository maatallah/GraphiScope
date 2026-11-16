using System.Security.Cryptography;

namespace GraphiScope.Core.Services;

/// <summary>
/// Moteur de hachage cryptographique économe en mémoire (V1.6.0, respect de Q-003).
/// </summary>
public sealed class HashService : IHashService
{
    private const int BufferSize = 64 * 1024; // Buffer de lecture 64 Ko
    private const int QuickHashSize = 8 * 1024; // 8 Ko pour QuickHash

    public async Task<string> ComputeSha256Async(string filePath, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Fichier introuvable pour calcul de hachage.", filePath);

        await using var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite,
            BufferSize,
            FileOptions.Asynchronous | FileOptions.SequentialScan);

        using var sha = SHA256.Create();
        byte[] hashBytes = await sha.ComputeHashAsync(stream, ct);
        return Convert.ToHexString(hashBytes);
    }

    public async Task<string?> ComputeQuickHashAsync(string filePath, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        if (!File.Exists(filePath))
            return null;

        await using var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite,
            QuickHashSize,
            FileOptions.Asynchronous);

        byte[] buffer = new byte[QuickHashSize];
        int bytesRead = await stream.ReadAsync(buffer.AsMemory(0, QuickHashSize), ct);
        if (bytesRead <= 0)
            return null;

        using var sha = SHA256.Create();
        byte[] hashBytes = sha.ComputeHash(buffer, 0, bytesRead);
        return Convert.ToHexString(hashBytes);
    }
}

