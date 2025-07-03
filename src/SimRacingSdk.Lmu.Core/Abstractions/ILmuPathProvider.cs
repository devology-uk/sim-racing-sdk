namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuPathProvider
{
    string LogFolder { get; }
    string PlayerFolder { get; }
    string ResultsFolder { get; }
    string SettingsFilePath { get; }
    string UserDataFolder { get; }
}