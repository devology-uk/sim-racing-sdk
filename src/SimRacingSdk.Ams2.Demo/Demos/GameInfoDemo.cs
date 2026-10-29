using Microsoft.Extensions.Logging;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Demo.Abstractions;

namespace SimRacingSdk.Ams2.Demo.Demos;

// Shows how to use Ams2.Core's providers that need no running game.
public class GameInfoDemo : IGameInfoDemo
{
    private readonly IAms2CarInfoProvider carInfoProvider;
    private readonly IAms2CompatibilityChecker compatibilityChecker;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<GameInfoDemo> logger;
    private readonly IAms2PathProvider pathProvider;
    private readonly IAms2TrackInfoProvider trackInfoProvider;

    public GameInfoDemo(ILogger<GameInfoDemo> logger,
        IConsoleLog consoleLog,
        IAms2CompatibilityChecker compatibilityChecker,
        IAms2PathProvider pathProvider,
        IAms2CarInfoProvider carInfoProvider,
        IAms2TrackInfoProvider trackInfoProvider)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.compatibilityChecker = compatibilityChecker;
        this.pathProvider = pathProvider;
        this.carInfoProvider = carInfoProvider;
        this.trackInfoProvider = trackInfoProvider;
    }

    public void Show()
    {
        this.ShowCompatibility();
        if(!this.compatibilityChecker.IsAms2Installed())
        {
            return;
        }

        this.ShowPaths();
        this.ShowCatalogs();
    }

    private void ShowCatalogs()
    {
        this.Write("Catalogs:");
        var tracks = this.trackInfoProvider.GetTrackInfos();
        this.Write($"  Cars: {this.carInfoProvider.GetCarInfos().Count}, {this.carInfoProvider.GetCarClasses().Count} classes");
        this.Write($"  Tracks: {tracks.Count}, {tracks.Sum(track => track.Layouts.Count)} layouts");
    }

    private void ShowCompatibility()
    {
        this.Write("Compatibility:");
        this.Write($"  Installed: {this.compatibilityChecker.IsAms2Installed()}");
    }

    private void ShowPaths()
    {
        this.Write("Paths:");
        this.Write($"  Documents: {this.pathProvider.DocumentsFolderPath}");
    }

    private void Write(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }
}
