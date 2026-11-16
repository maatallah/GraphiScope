using System.Text.RegularExpressions;

namespace GraphiScope.Core.Domain;

/// <summary>
/// Objet valeur immuable représentant une empreinte cryptographique SHA-256 validée.
/// </summary>
public readonly record struct FileHash
{
    private static readonly Regex HexRegex = new("^[0-9A-Fa-f]{64}$", RegexOptions.Compiled);

    public string Value { get; }

    public FileHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !HexRegex.IsMatch(value))
            throw new ArgumentException("L'empreinte SHA-256 doit être une chaîne hexadécimale de 64 caractères.", nameof(value));

        Value = value.ToUpperInvariant();
    }

    public override string ToString() => Value;

    public static implicit operator string(FileHash hash) => hash.Value;
    public static explicit operator FileHash(string value) => new(value);
}

/// <summary>
/// Objet valeur immuable représentant une taille en octets avec formatage lisible.
/// </summary>
public readonly record struct ByteSize(long Bytes)
{
    public long Bytes { get; } = Bytes >= 0 ? Bytes : throw new ArgumentOutOfRangeException(nameof(Bytes), "La taille en octets ne peut être négative.");

    public double KiloBytes => Bytes / 1024.0;
    public double MegaBytes => Bytes / (1024.0 * 1024.0);
    public double GigaBytes => Bytes / (1024.0 * 1024.0 * 1024.0);

    public string ToDisplayString()
    {
        return Bytes switch
        {
            < 1024 => $"{Bytes} o",
            < 1024 * 1024 => $"{KiloBytes:F2} Ko",
            < 1024 * 1024 * 1024 => $"{MegaBytes:F2} Mo",
            _ => $"{GigaBytes:F2} Go"
        };
    }

    public override string ToString() => ToDisplayString();
    public static implicit operator long(ByteSize size) => size.Bytes;
    public static implicit operator ByteSize(long bytes) => new(bytes);
}

/// <summary>
/// Objet valeur immuable pour un chemin système normalisé.
/// </summary>
public readonly record struct NormalizedPath
{
    public string Value { get; }

    public NormalizedPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Le chemin ne peut être vide.", nameof(path));

        // Normalisation standard Windows sans trailing slash sauf pour la racine
        string normalized = Path.GetFullPath(path).Trim();
        if (normalized.Length > 3 && (normalized.EndsWith('\\') || normalized.EndsWith('/')))
        {
            normalized = normalized.TrimEnd('\\', '/');
        }
        Value = normalized;
    }

    public override string ToString() => Value;
    public static implicit operator string(NormalizedPath path) => path.Value;
    public static explicit operator NormalizedPath(string path) => new(path);
}

