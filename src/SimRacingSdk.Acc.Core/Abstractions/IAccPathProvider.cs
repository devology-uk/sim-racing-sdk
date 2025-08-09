namespace SimRacingSdk.Acc.Core.Abstractions;

public interface IAccPathProvider
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
    string SeasonSettingsFilePath { get; }
    string SetupsFolderPath { get; }
}