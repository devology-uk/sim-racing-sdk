using SimRacingSdk.Acc.Core.Abstractions;

namespace SimRacingSdk.Acc.Core;

public class AccPathProvider : IAccPathProvider
{
    private const string AccountFileName = "account.json";
    private const string BroadcastingSettingsFileName = "broadcasting.json";
    private const string ConfigFolderName = "Config";
    private const string CustomCarsFolderName = "Cars";
    private const string CustomDriversFolderName = "Drivers";
    private const string CustomLiveriesFolderName = "Liveries";
    private const string CustomsFolderName = "Customs";
    private const string DocumentsFolderName = "Assetto Corsa Competizione";
    private const string ReplaysFolderName = "Replay";
    private const string ReplaySavedFolderName = "Saved";
    private const string ResultsFolderName = "Results";
    private const string SeasonSettingsFileName = "seasonEntity.json";
    private const string SetupsFolderName = "Setups";

    private static AccPathProvider? singletonInstance;

    public AccPathProvider()
    {
        var myDocumentsFolderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DocumentsFolderName);
        this.AccountFilePath = Path.Combine(myDocumentsFolderPath, ConfigFolderName, AccountFileName);

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
        this.SeasonSettingsFilePath =
            Path.Combine(myDocumentsFolderPath, this.ConfigFolderPath, SeasonSettingsFileName);
        this.SavedReplaysFolderPath = Path.Combine(myDocumentsFolderPath,
            ReplaysFolderName,
            ReplaySavedFolderName);
        this.SetupsFolderPath = Path.Combine(myDocumentsFolderPath, SetupsFolderName);
        this.DocumentsFolderPath = myDocumentsFolderPath;
    }

    public string AccountFilePath { get; }
    public string BroadcastingSettingsFilePath { get; }
    public string ConfigFolderPath { get; }
    public string CustomCarsFolderPath { get; }
    public string CustomDriversFolderPath { get; }
    public string CustomLiveriesFolderPath { get; }
    public string DocumentsFolderPath { get; }
    public static AccPathProvider Instance => singletonInstance ??= new AccPathProvider();
    public string ResultFolderPath { get; }
    public string SavedReplaysFolderPath { get; }
    public string SeasonSettingsFilePath { get; }
    public string SetupsFolderPath { get; }
}