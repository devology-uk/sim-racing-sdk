using SimRacingSdk.Ace.Core.Abstractions;

namespace SimRacingSdk.Ace.Core;

// Folder/file layout mirrors Acc's Documents structure with the confirmed "ACE" Documents folder
// name substituted in. Whether Evo keeps the same Config/account.json/broadcasting.json layout as
// Acc is still an unverified assumption pending a check against a real installation.
public class AcePathProvider : IAcePathProvider
{
    private const string AccountFileName = "account.json";
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
    private const string SetupsFolderName = "Setups";

    private static AcePathProvider? singletonInstance;

    public AcePathProvider()
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
    public static AcePathProvider Instance => singletonInstance ??= new AcePathProvider();
    public string ResultFolderPath { get; }
    public string SavedReplaysFolderPath { get; }
    public string SetupsFolderPath { get; }
}
