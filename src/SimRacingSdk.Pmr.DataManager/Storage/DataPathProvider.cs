using System.IO;

namespace SimRacingSdk.Pmr.DataManager.Storage;

// Resolves paths under the repo's checked-in "data/pmr" folder - the source-controlled catalog
// that replaces pmr-cars.csv/pmr-tracks.csv, readable by anyone browsing the repo, not just Mike's
// machine. Finds the repo root by walking up from the running assembly looking for ".git" - unlike
// CLAUDE.md, that's guaranteed to exist for anyone who actually cloned the repo.
public class DataPathProvider : IDataPathProvider
{
    private static readonly Lazy<string> RepoRoot = new(FindRepoRoot);

    public string GetEntityFolder(string entityName)
    {
        var folder = Path.Combine(RepoRoot.Value, "data", "pmr", entityName);
        Directory.CreateDirectory(folder);
        return folder;
    }

    public string GetRepoRoot()
    {
        return RepoRoot.Value;
    }

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while(directory is not null && !Directory.Exists(Path.Combine(directory.FullName, ".git")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
               ?? throw new InvalidOperationException(
                   "Could not find repo root (walked up from AppContext.BaseDirectory looking for a .git folder).");
    }
}
