using Microsoft.Extensions.Logging;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Demo.Abstractions;

namespace SimRacingSdk.Pmr.Demo.Demos;

// Shows how to use Pmr.Core's providers that need no running game.
public class GameInfoDemo : IGameInfoDemo
{
    private readonly IPmrCarInfoProvider carInfoProvider;
    private readonly IConsoleLog consoleLog;
    private readonly IPmrLocalConfigProvider localConfigProvider;
    private readonly ILogger<GameInfoDemo> logger;
    private readonly IPmrPathProvider pathProvider;
    private readonly IPmrTrackInfoProvider trackInfoProvider;

    public GameInfoDemo(ILogger<GameInfoDemo> logger,
        IConsoleLog consoleLog,
        IPmrPathProvider pathProvider,
        IPmrLocalConfigProvider localConfigProvider,
        IPmrCarInfoProvider carInfoProvider,
        IPmrTrackInfoProvider trackInfoProvider)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.pathProvider = pathProvider;
        this.localConfigProvider = localConfigProvider;
        this.carInfoProvider = carInfoProvider;
        this.trackInfoProvider = trackInfoProvider;
    }

    public void Show()
    {
        this.ShowPaths();
        this.ShowLocalConfig();
        this.ShowCatalogs();
    }

    private void ShowCatalogs()
    {
        this.Write("Catalogs:");
        var tracks = this.trackInfoProvider.GetTrackInfos();
        this.Write($"  Cars: {this.carInfoProvider.GetCarInfos().Count}, {this.carInfoProvider.GetManufacturers().Count} manufacturers");
        this.Write($"  Tracks: {tracks.Count}, {tracks.Sum(track => track.Layouts.Count)} layouts");
    }

    private void ShowLocalConfig()
    {
        var driverProfile = this.localConfigProvider.GetDriverProfile();
        this.Write(driverProfile is null ? "Driver profile: none yet" : $"Driver nationality: {driverProfile.Nationality}");

        var localSettings = this.localConfigProvider.GetLocalSettings();
        if(localSettings is null)
        {
            this.Write("Settings: none yet");
            return;
        }

        this.Write($"UDP: {(localSettings.UdpEnabled ? "enabled" : "disabled")}, {localSettings.UdpHost}:{localSettings.UdpPort} "
                   + $"at {localSettings.UdpFrequencyHz} Hz");
        this.Write($"Imperial units: distance {localSettings.IsImperialDistance}, fluid {localSettings.IsImperialFluid}, "
                   + $"pressure {localSettings.IsImperialPressure}, weight {localSettings.IsImperialWeight}");
    }

    private void ShowPaths()
    {
        this.Write("Paths:");
        this.Write($"  Game: {this.pathProvider.GamePath}");
        this.Write($"  Data: {this.pathProvider.DataFolderPath}");
        this.Write($"  Vehicles: {this.pathProvider.VehiclesFolderPath}");
        this.Write($"  Tracks: {this.pathProvider.TracksFolderPath}");
        this.Write($"  User data: {this.pathProvider.UserDataFolderPath}");
        this.Write($"  Savegame (setups): {this.pathProvider.SavegameFolderPath}");
        this.Write($"  Settings: {this.pathProvider.SettingsFilePath}");
        this.Write($"  Driver profile: {this.pathProvider.DriverProfileFilePath}");
    }

    private void Write(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }
}
