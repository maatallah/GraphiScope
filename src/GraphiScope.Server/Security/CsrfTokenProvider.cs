using System.Security.Cryptography;

namespace GraphiScope.Server.Security;

/// <summary>
/// Gestionnaire en mémoire volatile du jeton cryptographique anti-CSRF (256 bits).
/// </summary>
public sealed class CsrfTokenProvider
{
    private readonly string _token;

    public string Token => _token;

    public CsrfTokenProvider()
    {
        byte[] bytes = new byte[32]; // 256 bits
        RandomNumberGenerator.Fill(bytes);
        _token = Convert.ToHexString(bytes);
    }

    /// <summary>
    /// Valide un jeton reçu en temps constant pour prévenir les attaques par analyse temporelle.
    /// </summary>
    public bool ValidateToken(string? candidate)
    {
        if (string.IsNullOrWhiteSpace(candidate))
            return false;

        byte[] expectedBytes = Convert.FromHexString(_token);
        try
        {
            byte[] candidateBytes = Convert.FromHexString(candidate);
            return CryptographicOperations.FixedTimeEquals(expectedBytes, candidateBytes);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}

