using Microsoft.Extensions.Logging;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Demo.Abstractions;

namespace SimRacingSdk.R3e.Demo.Demos;

// Shows how to use R3e.Core's providers that need no running game.
public class GameInfoDemo : IGameInfoDemo
{
    private readonly IR3eCarInfoProvider carInfoProvider;
    private readonly IR3eCompatibilityChecker compatibilityChecker;
    private readonly IConsoleLog consoleLog;
    private readonly IR3eLocalConfigProvider localConfigProvider;
    private readonly ILogger<GameInfoDemo> logger;
    private readonly IR3ePathProvider pathProvider;
    private readonly IR3eTrackInfoProvider trackInfoProvider;

    public GameInfoDemo(ILogger<GameInfoDemo> logger,
        IConsoleLog consoleLog,
        IR3eCompatibilityChecker compatibilityChecker,
        IR3ePathProvider pathProvider,
        IR3eLocalConfigProvider localConfigProvider,
        IR3eCarInfoProvider carInfoProvider,
        IR3eTrackInfoProvider trackInfoProvider)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.compatibilityChecker = compatibilityChecker;
        this.pathProvider = pathProvider;
        this.localConfigProvider = localConfigProvider;
        this.carInfoProvider = carInfoProvider;
        this.trackInfoProvider = trackInfoProvider;
    }

    public void Show()
    {
        this.ShowCompatibility();
        if(!this.compatibilityChecker.IsR3eInstalled())
        {
            return;
        }

        this.ShowPaths();
        this.ShowGameOptions();
        this.ShowCatalogs();
    }

    private void ShowCatalogs()
    {
        var cars = this.carInfoProvider.GetCarInfos();
        var tracks = this.trackInfoProvider.GetTrackInfos();
        this.Write("Catalogs:");
        this.Write($"  Cars: {cars.Count} ({cars.Count(car => !car.NotInGameSelector)} selectable in game), "
                   + $"{this.carInfoProvider.GetCarClasses().Count} classes");
        this.Write($"  Tracks: {tracks.Count} ({tracks.Count(track => !track.NotInGameSelector)} selectable in game), "
                   + $"{tracks.Sum(track => track.Layouts.Count)} layouts");
    }

    private void ShowCompatibility()
    {
        this.Write("Compatibility:");
        this.Write($"  Installed: {this.compatibilityChecker.IsR3eInstalled()}");
        this.Write($"  Launched at least once: {this.compatibilityChecker.HasLaunchedGame()}");
        this.Write($"  Driven at least one session: {this.compatibilityChecker.HasDrivenAtLeastOneSession()}");
        this.Write($"  Saved a setup: {this.compatibilityChecker.HasSavedSetup()}");
    }

    private void ShowGameOptions()
    {
        var gameOptions = this.localConfigProvider.GetGameOptions();
        if(gameOptions is null)
        {
            this.Write("Game options: none yet - the game writes them on first launch");
            return;
        }

        this.Write("Game options:");
        this.Write($"  Units: {gameOptions.UnitSystem}");
        this.Write($"  Language: {gameOptions.LanguageCode}");
    }

    private void ShowPaths()
    {
        this.Write("Paths:");
        this.Write($"  Game: {this.pathProvider.GamePath}");
        this.Write($"  Executables: {this.pathProvider.ExecutablesFolderPath}");
        this.Write($"  Catalog: {this.pathProvider.CatalogFilePath}");
        this.Write($"  User data: {this.pathProvider.UserDataFolderPath}");
        this.Write($"  Game options: {this.pathProvider.GameOptionsFilePath}");
        this.Write($"  Results: {this.pathProvider.ResultsFolderPath}");
        this.Write($"  Setups: {this.pathProvider.SetupsFolderPath}");
        this.Write($"  Time attack: {this.pathProvider.TimeAttackFolderPath}");
    }

    private void Write(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }
}
