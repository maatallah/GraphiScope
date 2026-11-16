using System.Runtime.CompilerServices;
using System.Security;
using GraphiScope.Core.Domain;
using GraphiScope.Core.Security;

namespace GraphiScope.Core.Services;

/// <summary>
/// Moteur récursif de parcours d'arborescence NTFS/ExFAT tolérant aux erreurs d'accès Windows.
/// </summary>
public sealed class FileSystemCrawler : IFileSystemCrawler
{
    private readonly PathConfinementService _confinementService;

    public FileSystemCrawler(PathConfinementService? confinementService = null)
    {
        _confinementService = confinementService ?? new PathConfinementService();
    }

    public async IAsyncEnumerable<PhysicalFile> CrawlAsync(PhysicalRoot root, [EnumeratorCancellation] CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(root);

        if (!Directory.Exists(root.FullPath))
        {
            throw new VolumeUnavailableException($"La racine d'inventaire '{root.FullPath}' est introuvable ou déconnectée.");
        }

        var dirQueue = new Queue<string>();
        dirQueue.Enqueue(root.FullPath);

        while (dirQueue.Count > 0)
        {
            ct.ThrowIfCancellationRequested();

            string currentDir = dirQueue.Dequeue();
            string normalizedDir = _confinementService.NormalizeForWin32Extended(currentDir);

            // 1. Énumération des sous-dossiers avec tolérance aux erreurs d'accès
            string[] subDirs;
            try
            {
                subDirs = Directory.GetDirectories(normalizedDir);
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or SecurityException or DirectoryNotFoundException or PathTooLongException)
            {
                // Journalisation ou ignorance gracieuse des répertoires sans droits (ex: System Volume Information)
                continue;
            }

            foreach (var sub in subDirs)
            {
                dirQueue.Enqueue(sub);
            }

            // 2. Énumération des fichiers du dossier courant
            string[] files;
            try
            {
                files = Directory.GetFiles(normalizedDir);
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or SecurityException or DirectoryNotFoundException or PathTooLongException)
            {
                continue;
            }

            foreach (var filePath in files)
            {
                ct.ThrowIfCancellationRequested();

                PhysicalFile? physicalFile = null;
                try
                {
                    var fileInfo = new FileInfo(filePath);
                    string relative = Path.GetRelativePath(root.FullPath, fileInfo.FullName);

                    physicalFile = new PhysicalFile(
                        root.RootId,
                        fileInfo.FullName,
                        relative,
                        fileInfo.Name,
                        fileInfo.Length,
                        fileInfo.CreationTimeUtc,
                        fileInfo.LastWriteTimeUtc,
                        (uint)fileInfo.Attributes
                    );
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    // Fichier verrouillé ou inaccessible
                    string name = Path.GetFileName(filePath);
                    string relative = Path.GetRelativePath(root.FullPath, filePath);
                    physicalFile = new PhysicalFile(root.RootId, filePath, relative, name, 0, DateTime.UtcNow, DateTime.UtcNow)
                    {
                        State = PhysicalState.UNSTABLE
                    };
                }

                if (physicalFile != null)
                {
                    yield return physicalFile;
                }
            }

            await Task.Yield();
        }
    }
}

