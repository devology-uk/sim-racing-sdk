using System.IO;

namespace SimRacingSdk.DataManager.Shared.Storage;

// Resolves paths under the repo's checked-in "data/<game>" folder - the source-controlled catalog,
// readable by anyone browsing the repo, not just Mike's machine. Finds the repo root by walking up
// from the running assembly looking for ".git" - unlike CLAUDE.md, that's guaranteed to exist for
// anyone who actually cloned the repo.
public class DataPathProvider : IDataPathProvider
{
    private static readonly Lazy<string> RepoRoot = new(FindRepoRoot);

    private readonly DataManagerOptions options;

    public DataPathProvider(DataManagerOptions options)
    {
        this.options = options;
    }

    public string GetEntityFolder(string entityName)
    {
        var folder = Path.Combine(RepoRoot.Value, "data", this.options.DataFolderName, entityName);
        Directory.CreateDirectory(folder);
        return folder;
    }

    public string GetProjectFolder(string projectName)
    {
        return Path.Combine(RepoRoot.Value, "src", projectName);
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
