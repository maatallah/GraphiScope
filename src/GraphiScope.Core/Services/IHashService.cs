namespace GraphiScope.Core.Services;

/// <summary>
/// Contrat pour le calcul d'empreintes cryptographiques et le pré-filtrage par hachage partiel.
/// </summary>
public interface IHashService
{
    /// <summary>
    /// Calcule l'empreinte complète SHA-256 en flux continu sans charger l'intégralité du fichier en mémoire.
    /// </summary>
    Task<string> ComputeSha256Async(string filePath, CancellationToken ct = default);

    /// <summary>
    /// Calcule le QuickHash sur les 8 premiers kilo-octets du fichier (filtre négatif d'exclusion rapide uniquement).
    /// </summary>
    Task<string?> ComputeQuickHashAsync(string filePath, CancellationToken ct = default);
}

