using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Core.Models;

namespace SimRacingSdk.R3e.Core;

public class R3ePathProvider : IR3ePathProvider
{
    private const string CatalogFileName = "r3e-data.json";
    private const string CatalogFolderName = @"GameData\General";
    private const string DefaultRootFolderName = "RaceRoom Racing Experience";
    private const string ExecutableFileName = "RRRE64.exe";
    private const string ExecutablesFolderName = "Game";
    private const string GameName = "raceroom racing experience";
    private const string GameOptionsFileName = @"MenuSettings\game_options";
    private const string InstallDirectoryFileName = "GameInstallDir.ini";
    private const string InstallDirectoryKey = "GameInstallDir=";
    private const string ResultsFolderName = @"Log\Results";
    private const string RootFolderSearchPattern = DefaultRootFolderName + "*";
    private const string SetupsFolderName = "CarSetups";
    private const string SimBinFolderName = @"My Games\SimBin";
    private const string TimeAttackFolderName = "TimeAttack";
    private const string UserDataFolderName = "UserData";

    private static R3ePathProvider? singletonInstance;

    public R3ePathProvider(ISteamInfoProvider steamInfoProvider)
    {
        var simBinFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            SimBinFolderName);
        var userFolders = FindUserFoldersMostRecentFirst(simBinFolderPath);

        this.GamePath = ResolveGamePath(steamInfoProvider, userFolders);
        this.ExecutablesFolderPath = Path.Combine(this.GamePath, ExecutablesFolderName);
        this.CatalogFilePath = Path.Combine(this.ExecutablesFolderPath, CatalogFolderName, CatalogFileName);

        var rootFolderPath = ResolveRootFolderPath(userFolders, this.ExecutablesFolderPath, simBinFolderPath);
        this.UserDataFolderPath = Path.Combine(rootFolderPath, UserDataFolderName);
        this.GameOptionsFilePath = Path.Combine(this.UserDataFolderPath, GameOptionsFileName);
        this.ResultsFolderPath = Path.Combine(this.UserDataFolderPath, ResultsFolderName);
        this.SetupsFolderPath = Path.Combine(this.UserDataFolderPath, SetupsFolderName);
        this.TimeAttackFolderPath = Path.Combine(this.UserDataFolderPath, TimeAttackFolderName);
    }

    public static R3ePathProvider Instance =>
        singletonInstance ??= new R3ePathProvider(SteamInfoProvider.Instance);

    public string CatalogFilePath { get; }
    public string ExecutablesFolderPath { get; }
    public string GameOptionsFilePath { get; }
    public string GamePath { get; }
    public string ResultsFolderPath { get; }
    public string SetupsFolderPath { get; }
    public string TimeAttackFolderPath { get; }
    public string UserDataFolderPath { get; }

    // Moving the game to another Steam library leaves an empty folder behind in the old one,
    // which Steam's folder lookup can still return, so only a folder holding the game counts.
    private static string ResolveGamePath(ISteamInfoProvider steamInfoProvider,
        IReadOnlyList<R3eUserFolder> userFolders)
    {
        var steamGamePath = steamInfoProvider.GetGamePath(GameName);
        if(ContainsGame(Path.Combine(steamGamePath, ExecutablesFolderName)))
        {
            return steamGamePath;
        }

        var recordedExecutablesFolderPath = userFolders.Select(userFolder => userFolder.ExecutablesFolderPath)
                                                       .FirstOrDefault(ContainsGame);

        return recordedExecutablesFolderPath == null
                   ? steamGamePath
                   : Path.GetDirectoryName(recordedExecutablesFolderPath) ?? string.Empty;
    }

    private static string ResolveRootFolderPath(IReadOnlyList<R3eUserFolder> userFolders,
        string executablesFolderPath,
        string simBinFolderPath)
    {
        var matchingUserFolder = userFolders.FirstOrDefault(userFolder =>
                                     IsSameFolder(userFolder.ExecutablesFolderPath, executablesFolderPath))
                                 ?? userFolders.FirstOrDefault();

        return matchingUserFolder?.RootFolderPath ?? Path.Combine(simBinFolderPath, DefaultRootFolderName);
    }

    private static IReadOnlyList<R3eUserFolder> FindUserFoldersMostRecentFirst(string simBinFolderPath)
    {
        if(!Directory.Exists(simBinFolderPath))
        {
            return [];
        }

        return Directory.EnumerateDirectories(simBinFolderPath, RootFolderSearchPattern)
                        .Select(ReadUserFolder)
                        .OfType<R3eUserFolder>()
                        .OrderByDescending(userFolder => userFolder.LastLaunchedUtc)
                        .ToList();
    }

    private static R3eUserFolder? ReadUserFolder(string rootFolderPath)
    {
        var installDirectoryFilePath = Path.Combine(rootFolderPath, InstallDirectoryFileName);
        if(!File.Exists(installDirectoryFilePath))
        {
            return null;
        }

        return new R3eUserFolder
        {
            ExecutablesFolderPath = ReadExecutablesFolderPath(installDirectoryFilePath),
            LastLaunchedUtc = File.GetLastWriteTimeUtc(installDirectoryFilePath),
            RootFolderPath = rootFolderPath
        };
    }

    private static string ReadExecutablesFolderPath(string installDirectoryFilePath)
    {
        return File.ReadLines(installDirectoryFilePath)
                   .Where(line => line.StartsWith(InstallDirectoryKey, StringComparison.OrdinalIgnoreCase))
                   .Select(line => line[InstallDirectoryKey.Length..].Trim().TrimEnd('\\'))
                   .FirstOrDefault() ?? string.Empty;
    }

    private static bool ContainsGame(string executablesFolderPath)
    {
        return !string.IsNullOrWhiteSpace(executablesFolderPath)
               && File.Exists(Path.Combine(executablesFolderPath, ExecutableFileName));
    }

    private static bool IsSameFolder(string firstFolderPath, string secondFolderPath)
    {
        return string.Equals(firstFolderPath.TrimEnd('\\'),
            secondFolderPath.TrimEnd('\\'),
            StringComparison.OrdinalIgnoreCase);
    }
}
