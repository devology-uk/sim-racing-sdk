using SimRacingSdk.R3e.Core.Abstractions;

namespace SimRacingSdk.R3e.Core;

public class R3eCompatibilityChecker : IR3eCompatibilityChecker
{
    private const string ResultsFilePattern = "*.txt";
    private const string SetupFilePattern = "*.svm";

    private static R3eCompatibilityChecker? singletonInstance;
    private readonly IR3ePathProvider r3ePathProvider;

    public R3eCompatibilityChecker(IR3ePathProvider r3ePathProvider)
    {
        this.r3ePathProvider = r3ePathProvider;
    }

    public static R3eCompatibilityChecker Instance =>
        singletonInstance ??= new R3eCompatibilityChecker(R3ePathProvider.Instance);

    public bool HasDrivenAtLeastOneSession()
    {
        return this.ContainsFiles(this.r3ePathProvider.ResultsFolderPath, ResultsFilePattern, SearchOption.TopDirectoryOnly);
    }

    public bool HasLaunchedGame()
    {
        return Directory.Exists(this.r3ePathProvider.UserDataFolderPath);
    }

    public bool HasSavedSetup()
    {
        return this.ContainsFiles(this.r3ePathProvider.SetupsFolderPath, SetupFilePattern, SearchOption.AllDirectories);
    }

    public bool IsR3eInstalled()
    {
        return Directory.Exists(this.r3ePathProvider.ExecutablesFolderPath);
    }

    private bool ContainsFiles(string folderPath, string searchPattern, SearchOption searchOption)
    {
        return Directory.Exists(folderPath) && Directory.EnumerateFiles(folderPath, searchPattern, searchOption)
                                                        .Any();
    }
}
