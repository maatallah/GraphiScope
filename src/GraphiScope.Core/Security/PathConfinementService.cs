using System.Runtime.InteropServices;
using System.Text;
using GraphiScope.Core.Domain;

namespace GraphiScope.Core.Security;

/// <summary>
/// Service de confinement assurant la validation des chemins et volumes Windows (Win32 P/Invoke).
/// Interdit formellement tout hébergement de la base SQLite sur un partage réseau (UNC/SMB/Remote).
/// </summary>
public sealed class PathConfinementService
{
    private const uint DRIVE_UNKNOWN = 0;
    private const uint DRIVE_NO_ROOT_DIR = 1;
    private const uint DRIVE_REMOVABLE = 2;
    private const uint DRIVE_FIXED = 3;
    private const uint DRIVE_REMOTE = 4;
    private const uint DRIVE_CDROM = 5;
    private const uint DRIVE_RAMDISK = 6;

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern uint GetDriveTypeW(string lpRootPathName);

    [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int WNetGetConnectionW(string lpLocalName, StringBuilder lpRemoteName, ref int lpnLength);

    /// <summary>
    /// Retourne le chemin du répertoire officiel de stockage du catalogue GraphiScope sous %LOCALAPPDATA%\GraphiScope\Catalog.
    /// Conforme à la décision propriétaire Q-004.
    /// </summary>
    public string GetDefaultCatalogDirectory()
    {
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localAppData, "GraphiScope", "Catalog");
    }

    /// <summary>
    /// Valide de manière étanche que le chemin spécifié pour le fichier de catalogue SQLite est strictement confiné
    /// à l'intérieur du répertoire autorisé (%LOCALAPPDATA%\GraphiScope\Catalog par défaut, décision Q-004).
    /// Rejette formellement :
    /// - les chemins absolus extérieurs au répertoire autorisé (InvalidOperationException) ;
    /// - les chemins relatifs tentant une remontée d'arborescence '..' (InvalidOperationException) ;
    /// - les partages réseau UNC (NetworkStorageNotSupportedException) ;
    /// - les lecteurs réseau distants (NetworkStorageNotSupportedException).
    /// </summary>
    public void ValidateCatalogPath(string path, string? authorizedDirectory = null)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Le chemin du catalogue ne peut être vide.", nameof(path));

        // 1. Validation de stockage strictement local (interdiction UNC et lecteurs réseau distants)
        ValidateLocalStoragePath(path);

        // 2. Détermination du répertoire autorisé canonique (%LOCALAPPDATA%\GraphiScope\Catalog par défaut)
        string authorizedRoot = authorizedDirectory ?? GetDefaultCatalogDirectory();
        string canonicalAuthorized = Path.GetFullPath(authorizedRoot).TrimEnd('\\', '/') + Path.DirectorySeparatorChar;

        // 3. Normalisation canonique complète du chemin candidat (résolution des .. et chemins relatifs)
        string canonicalCandidate = Path.GetFullPath(path);

        // 4. Contrôle de confinement strict
        if (!canonicalCandidate.StartsWith(canonicalAuthorized, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Violation de confinement du catalogue (Q-004) : le chemin '{canonicalCandidate}' est en dehors " +
                $"du répertoire autorisé '{canonicalAuthorized}'. Tout chemin absolu extérieur ou relatif avec remontée '..' est formellement interdit.");
        }
    }

    /// <summary>
    /// Vérifie et valide que l'emplacement destiné à héberger le catalogue SQLite est strictement local.
    /// Lève NetworkStorageNotSupportedException en cas de chemin réseau.
    /// </summary>
    public void ValidateLocalStoragePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Le chemin ne peut être vide.", nameof(path));

        string fullPath = Path.GetFullPath(path);

        // 1. Détection syntaxique immédiate des partages UNC
        if (IsUncPath(fullPath))
        {
            throw new NetworkStorageNotSupportedException(
                $"Le catalogue SQLite ne peut pas être hébergé sur un partage réseau UNC : '{fullPath}'. " +
                "GraphiScope exige un volume de stockage strictement local pour garantir l'intégrité du mode WAL.");
        }

        // 2. Détection des lecteurs mappés réseau via GetDriveTypeW et WNetGetConnectionW
        string? root = Path.GetPathRoot(fullPath);
        if (!string.IsNullOrEmpty(root))
        {
            uint driveType = GetDriveTypeW(root);
            if (driveType == DRIVE_REMOTE)
            {
                throw new NetworkStorageNotSupportedException(
                    $"Le volume '{root}' est un lecteur réseau distant (DRIVE_REMOTE). " +
                    "Le catalogue SQLite ne peut pas être hébergé sur un lecteur réseau.");
            }

            // Vérification complémentaire MPR WNetGetConnection pour les lettres de lecteur (ex. Z:)
            string driveLetter = root.TrimEnd('\\', '/');
            if (driveLetter.Length == 2 && driveLetter[1] == ':')
            {
                var sb = new StringBuilder(512);
                int length = sb.Capacity;
                int result = WNetGetConnectionW(driveLetter, sb, ref length);
                if (result == 0 && sb.Length > 0)
                {
                    throw new NetworkStorageNotSupportedException(
                        $"Le lecteur '{driveLetter}' est mappé vers la ressource réseau distante '{sb}'. " +
                        "Le catalogue SQLite ne supporte pas les lecteurs réseau.");
                }
            }
        }
    }

    /// <summary>
    /// Détermine si un chemin correspond à une syntaxe réseau UNC standard ou étendue.
    /// </summary>
    public bool IsUncPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        // \\?\UNC\serveur\partage
        if (path.StartsWith(@"\\?\UNC\", StringComparison.OrdinalIgnoreCase))
            return true;

        // \\serveur\partage (sans ?\)
        if (path.StartsWith(@"\\") && !path.StartsWith(@"\\?\"))
            return true;

        return false;
    }

    /// <summary>
    /// Qualifie le type de support physique Win32 pour une racine donnée (locale, amovible ou partage réseau SMB/UNC).
    /// Note architecturale : Ne pas confondre le stockage du catalogue SQLite (strictement local) avec les racines
    /// d'inventaire qui peuvent être des disques locaux, des disques amovibles ou des partages réseau SMB/UNC.
    /// </summary>
    public StorageMediumType GetMediumType(string rootPath)
    {
        if (string.IsNullOrWhiteSpace(rootPath))
            return StorageMediumType.UNKNOWN;

        if (IsUncPath(rootPath))
            return StorageMediumType.NETWORK_SHARE;

        string? root = Path.GetPathRoot(rootPath);
        if (string.IsNullOrEmpty(root))
            return StorageMediumType.UNKNOWN;

        uint driveType = GetDriveTypeW(root);
        if (driveType == DRIVE_REMOTE)
            return StorageMediumType.NETWORK_SHARE;

        // Vérification complémentaire MPR pour les lecteurs réseau mappés (ex. Z:)
        string driveLetter = root.TrimEnd('\\', '/');
        if (driveLetter.Length == 2 && driveLetter[1] == ':')
        {
            var sb = new StringBuilder(512);
            int length = sb.Capacity;
            int result = WNetGetConnectionW(driveLetter, sb, ref length);
            if (result == 0 && sb.Length > 0)
            {
                return StorageMediumType.NETWORK_SHARE;
            }
        }

        return driveType switch
        {
            DRIVE_FIXED => StorageMediumType.FIXED_DISK,
            DRIVE_REMOVABLE => StorageMediumType.REMOVABLE_DISK,
            DRIVE_CDROM => StorageMediumType.OPTICAL_DISC,
            DRIVE_RAMDISK => StorageMediumType.RAM_DISK,
            _ => StorageMediumType.UNKNOWN
        };
    }

    /// <summary>
    /// Prépare un chemin pour l'API Windows, en gérant le préfixe étendu \\?\ pour les chemins longs (> 260 car).
    /// </summary>
    public string NormalizeForWin32Extended(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return path;

        string full = Path.GetFullPath(path);
        if (full.Length >= 250 && !full.StartsWith(@"\\?\"))
        {
            if (full.StartsWith(@"\\"))
            {
                // \\server\share -> \\?\UNC\server\share
                return @"\\?\UNC\" + full[2..];
            }
            return @"\\?\" + full;
        }

        return full;
    }
}

