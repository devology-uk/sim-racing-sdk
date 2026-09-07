using System.Linq;
using SimRacingSdk.Ace.Core.Abstractions;

namespace SimRacingSdk.Ace.Core;

public class AcePathProvider : IAcePathProvider
{
    private const string AccountFileName = "local.driverdescriptor.json";
    private const string BroadcastingSettingsFileName = "broadcasting.json";
    private const string ConfigFolderName = "Config";
    private const string CustomCarsFolderName = "Cars";
    private const string CustomDriversFolderName = "Drivers";
    private const string CustomLiveriesFolderName = "Liveries";
    private const string CustomsFolderName = "Customs";
    private const string DocumentsFolderName = "ACE";
    private const string ReplaysFolderName = "Replay";
    private const string ReplaySavedFolderName = "Saved";
    private const string ResultsFolderName = "Results";
    private const string SavedGamesFolderName = "Saved Games";
    private const string SetupsFolderName = "Car Setups";

    private static AcePathProvider? singletonInstance;

    public AcePathProvider()
    {
        var myDocumentsFolderPath = ResolveDataFolderPath();
        this.AccountFilePath = Path.Combine(myDocumentsFolderPath, AccountFileName);

        this.BroadcastingSettingsFilePath = Path.Combine(myDocumentsFolderPath,
            ConfigFolderName,
            BroadcastingSettingsFileName);
        this.ConfigFolderPath = Path.Combine(myDocumentsFolderPath, ConfigFolderName);
        this.CustomCarsFolderPath =
            Path.Combine(myDocumentsFolderPath, CustomsFolderName, CustomCarsFolderName);
        this.CustomDriversFolderPath =
            Path.Combine(myDocumentsFolderPath, CustomsFolderName, CustomDriversFolderName);

        this.CustomLiveriesFolderPath =
            Path.Combine(myDocumentsFolderPath, CustomsFolderName, CustomLiveriesFolderName);
        this.ResultFolderPath = Path.Combine(myDocumentsFolderPath, ResultsFolderName);
        this.SavedReplaysFolderPath = Path.Combine(myDocumentsFolderPath,
            ReplaysFolderName,
            ReplaySavedFolderName);
        this.SetupsFolderPath = Path.Combine(myDocumentsFolderPath, SetupsFolderName);
        this.DocumentsFolderPath = myDocumentsFolderPath;
    }

    private static string ResolveDataFolderPath()
    {
        var documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            DocumentsFolderName);
        var savedGamesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            SavedGamesFolderName,
            DocumentsFolderName);

        return CountEntries(savedGamesPath) > CountEntries(documentsPath) ? savedGamesPath : documentsPath;
    }

    private static int CountEntries(string path)
    {
        return Directory.Exists(path) ? Directory.EnumerateFileSystemEntries(path).Count() : 0;
    }

    public string AccountFilePath { get; }
    public string BroadcastingSettingsFilePath { get; }
    public string ConfigFolderPath { get; }
    public string CustomCarsFolderPath { get; }
    public string CustomDriversFolderPath { get; }
    public string CustomLiveriesFolderPath { get; }
    public string DocumentsFolderPath { get; }
    public static AcePathProvider Instance => singletonInstance ??= new AcePathProvider();
    public string ResultFolderPath { get; }
    public string SavedReplaysFolderPath { get; }
    public string SetupsFolderPath { get; }
}
