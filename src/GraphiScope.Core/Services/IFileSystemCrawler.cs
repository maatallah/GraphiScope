using System.Runtime.CompilerServices;
using GraphiScope.Core.Domain;

namespace GraphiScope.Core.Services;

/// <summary>
/// Contrat de parcours d'arborescence et d'extraction des métadonnées du système de fichiers local.
/// </summary>
public interface IFileSystemCrawler
{
    /// <summary>
    /// Énumère récursivement les fichiers sous une racine donnée en flux asynchrone tolérant aux erreurs d'accès.
    /// </summary>
    IAsyncEnumerable<PhysicalFile> CrawlAsync(PhysicalRoot root, CancellationToken ct = default);
}

