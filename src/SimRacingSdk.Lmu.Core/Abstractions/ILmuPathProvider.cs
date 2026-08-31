namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuPathProvider
{
    string CustomPluginVariablesFilePath { get; }
    string LogFolder { get; }
    string PlayerFolder { get; }
    string PluginsFolder { get; }
    string ResultsFolder { get; }
    string SettingsFilePath { get; }
    string SetupsFolderPath { get; }
    string UserDataFolder { get; }
}