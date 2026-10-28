using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.R3e.Core.Abstractions;

namespace SimRacingSdk.R3e.Core;

public class R3ePathProvider : IR3ePathProvider
{
    private const string CatalogFileName = "r3e-data.json";
    private const string CatalogFolderName = @"GameData\General";
    private const string ExecutablesFolderName = "Game";
    private const string GameName = "raceroom racing experience";
    private const string GameOptionsFileName = @"MenuSettings\game_options";
    private const string InstallDirectoryFileName = "GameInstallDir.ini";
    private const string InstallDirectoryKey = "GameInstallDir=";
    private const string ResultsFolderName = @"Log\Results";
    private const string RootFolderName = @"My Games\SimBin\RaceRoom Racing Experience";
    private const string SetupsFolderName = "CarSetups";
    private const string TimeAttackFolderName = "TimeAttack";
    private const string UserDataFolderName = "UserData";

    private static R3ePathProvider? singletonInstance;

    public R3ePathProvider(ISteamInfoProvider steamInfoProvider)
    {
        var rootFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            RootFolderName);

        this.GamePath = ResolveGamePath(steamInfoProvider, rootFolderPath);
        this.ExecutablesFolderPath = Path.Combine(this.GamePath, ExecutablesFolderName);
        this.CatalogFilePath = Path.Combine(this.ExecutablesFolderPath, CatalogFolderName, CatalogFileName);

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

    private static string ResolveGamePath(ISteamInfoProvider steamInfoProvider, string rootFolderPath)
    {
        var steamGamePath = steamInfoProvider.GetGamePath(GameName);
        return string.IsNullOrWhiteSpace(steamGamePath)
                   ? ReadGamePathRecordedByGame(rootFolderPath)
                   : steamGamePath;
    }

    // The game records its own install folder on every launch, pointing at the Game subfolder.
    private static string ReadGamePathRecordedByGame(string rootFolderPath)
    {
        var installDirectoryFilePath = Path.Combine(rootFolderPath, InstallDirectoryFileName);
        if(!File.Exists(installDirectoryFilePath))
        {
            return string.Empty;
        }

        var executablesFolderPath = File.ReadLines(installDirectoryFilePath)
                                        .Where(line => line.StartsWith(InstallDirectoryKey,
                                            StringComparison.OrdinalIgnoreCase))
                                        .Select(line => line[InstallDirectoryKey.Length..].Trim())
                                        .FirstOrDefault();

        return string.IsNullOrWhiteSpace(executablesFolderPath)
                   ? string.Empty
                   : Path.GetDirectoryName(executablesFolderPath.TrimEnd('\\')) ?? string.Empty;
    }
}
