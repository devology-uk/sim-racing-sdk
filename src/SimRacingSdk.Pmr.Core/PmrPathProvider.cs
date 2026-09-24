using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Pmr.Core.Abstractions;

namespace SimRacingSdk.Pmr.Core;

public class PmrPathProvider : IPmrPathProvider
{
    private const string DataFolderName = "data";
    private const string DriverProfileFileName = "driverProfile.xml";
    private const string GameName = "Project Motor Racing";
    private const string SavegameFolderName = "savegame1";
    private const string SettingsFileName = "settings.xml";
    private const string TracksFolderName = "tracks";
    private const string UserDataFolderName = @"My Games\ProjectMotorRacing";
    private const string VehiclesFolderName = "vehicles";

    private static PmrPathProvider? singletonInstance;

    public PmrPathProvider(ISteamInfoProvider steamInfoProvider)
    {
        this.GamePath = steamInfoProvider.GetGamePath(GameName);
        this.DataFolderPath = Path.Combine(this.GamePath, DataFolderName);
        this.TracksFolderPath = Path.Combine(this.DataFolderPath, TracksFolderName);
        this.VehiclesFolderPath = Path.Combine(this.DataFolderPath, VehiclesFolderName);

        this.UserDataFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            UserDataFolderName);
        this.SavegameFolderPath = Path.Combine(this.UserDataFolderPath, SavegameFolderName);
        this.SettingsFilePath = Path.Combine(this.SavegameFolderPath, SettingsFileName);
        this.DriverProfileFilePath = Path.Combine(this.SavegameFolderPath, DriverProfileFileName);
    }

    public static PmrPathProvider Instance =>
        singletonInstance ??= new PmrPathProvider(SteamInfoProvider.Instance);

    public string DataFolderPath { get; }
    public string DriverProfileFilePath { get; }
    public string GamePath { get; }
    public string SavegameFolderPath { get; }
    public string SettingsFilePath { get; }
    public string TracksFolderPath { get; }
    public string UserDataFolderPath { get; }
    public string VehiclesFolderPath { get; }
}
