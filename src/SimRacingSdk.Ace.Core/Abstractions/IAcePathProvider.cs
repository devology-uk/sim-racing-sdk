namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAcePathProvider
{
    string AccountFilePath { get; }
    string BroadcastingSettingsFilePath { get; }
    string ConfigFolderPath { get; }
    string CustomCarsFolderPath { get; }
    string CustomDriversFolderPath { get; }
    string CustomLiveriesFolderPath { get; }
    string DocumentsFolderPath { get; }
    string ResultFolderPath { get; }
    string SavedReplaysFolderPath { get; }
    string SetupsFolderPath { get; }
}
