using Microsoft.Extensions.Logging;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Demo.Abstractions;

namespace SimRacingSdk.Lmu.Demo.Demos;

// Shows how to use Lmu.Core's providers that need no running game.
public class GameInfoDemo : IGameInfoDemo
{
    private readonly ILmuCarInfoProvider carInfoProvider;
    private readonly IConsoleLog consoleLog;
    private readonly ILmuGameDataProvider gameDataProvider;
    private readonly ILogger<GameInfoDemo> logger;
    private readonly ILmuPathProvider pathProvider;
    private readonly ILmuTrackInfoProvider trackInfoProvider;

    public GameInfoDemo(ILogger<GameInfoDemo> logger,
        IConsoleLog consoleLog,
        ILmuPathProvider pathProvider,
        ILmuGameDataProvider gameDataProvider,
        ILmuCarInfoProvider carInfoProvider,
        ILmuTrackInfoProvider trackInfoProvider)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.pathProvider = pathProvider;
        this.gameDataProvider = gameDataProvider;
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
        this.Write($"  Cars: {this.carInfoProvider.GetCarInfos().Count}, {this.carInfoProvider.GetCarClasses().Count} classes");
        this.Write($"  Tracks: {tracks.Count}, {tracks.Sum(track => track.Layouts.Count)} layouts");
    }

    private void ShowLocalConfig()
    {
        var settings = this.gameDataProvider.GetSettings();
        this.Write($"Driver: {settings.DriverSettings.PlayerName} ({settings.DriverSettings.PlayerNickname}), "
                   + $"{settings.DriverSettings.Nationality}");
        this.Write($"Last car: {settings.DriverSettings.LastVehicleName}");
        this.Write($"External plugins enabled: {settings.GameOptions.EnableExternalPlugins?.ToString() ?? "not set"}");
    }

    private void ShowPaths()
    {
        this.Write("Paths:");
        this.Write($"  User data: {this.pathProvider.UserDataFolder}");
        this.Write($"  Player: {this.pathProvider.PlayerFolder}");
        this.Write($"  Settings: {this.pathProvider.SettingsFilePath}");
        this.Write($"  Results: {this.pathProvider.ResultsFolder}");
        this.Write($"  Setups: {this.pathProvider.SetupsFolderPath}");
        this.Write($"  Logs: {this.pathProvider.LogFolder}");
        this.Write($"  Plugins: {this.pathProvider.PluginsFolder}");
        this.Write($"  Custom plugin variables: {this.pathProvider.CustomPluginVariablesFilePath}");
    }

    private void Write(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }
}
