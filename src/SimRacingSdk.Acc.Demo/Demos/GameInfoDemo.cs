using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Demo.Abstractions;

namespace SimRacingSdk.Acc.Demo.Demos;

// Shows how to use Acc.Core's providers that need no running game.
public class GameInfoDemo : IGameInfoDemo
{
    private readonly IAccCarInfoProvider carInfoProvider;
    private readonly IAccCompatibilityChecker compatibilityChecker;
    private readonly IConsoleLog consoleLog;
    private readonly IAccLocalConfigProvider localConfigProvider;
    private readonly ILogger<GameInfoDemo> logger;
    private readonly IAccPathProvider pathProvider;
    private readonly IAccTrackInfoProvider trackInfoProvider;

    public GameInfoDemo(ILogger<GameInfoDemo> logger,
        IConsoleLog consoleLog,
        IAccCompatibilityChecker compatibilityChecker,
        IAccPathProvider pathProvider,
        IAccLocalConfigProvider localConfigProvider,
        IAccCarInfoProvider carInfoProvider,
        IAccTrackInfoProvider trackInfoProvider)
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
        if(!this.compatibilityChecker.IsAccInstalled())
        {
            return;
        }

        this.ShowPaths();
        this.ShowLocalConfig();
        this.ShowCatalogs();
    }

    private void ShowCatalogs()
    {
        this.Write("Catalogs:");
        this.Write($"  Cars: {this.carInfoProvider.GetCarInfos().Count}, {this.carInfoProvider.GetCarClasses().Count} classes");
        this.Write($"  Tracks: {this.trackInfoProvider.GetTrackInfos().Count}");
    }

    private void ShowCompatibility()
    {
        this.Write("Compatibility:");
        this.Write($"  Installed: {this.compatibilityChecker.IsAccInstalled()}");
        this.Write($"  Account available: {this.compatibilityChecker.IsAccountAvailable()}");
        this.Write($"  Valid broadcasting settings: {this.compatibilityChecker.HasValidBroadcastingSettings()}");
        this.Write($"  Driven at least one offline session: {this.compatibilityChecker.HasDrivenAtLeastOneOfflineSession()}");
        this.Write($"  Saved a setup: {this.compatibilityChecker.HasSavedSetup()}");
        this.Write($"  Custom cars: {this.compatibilityChecker.HasCustomCars()}");
        this.Write($"  Custom liveries: {this.compatibilityChecker.HasCustomLiveries()}");
    }

    private void ShowLocalConfig()
    {
        var account = this.localConfigProvider.GetAccount();
        this.Write(account is null
            ? "Account: none yet"
            : $"Account: {account.FullName} ({account.NickName}), {account.Country}");

        var broadcastingSettings = this.localConfigProvider.GetBroadcastingSettings();
        this.Write(broadcastingSettings is null
            ? "Broadcasting settings: none yet"
            : $"Broadcasting settings: UDP listener port {broadcastingSettings.UdpListenerPort}");
    }

    private void ShowPaths()
    {
        this.Write("Paths:");
        this.Write($"  Documents: {this.pathProvider.DocumentsFolderPath}");
        this.Write($"  Config: {this.pathProvider.ConfigFolderPath}");
        this.Write($"  Account: {this.pathProvider.AccountFilePath}");
        this.Write($"  Broadcasting settings: {this.pathProvider.BroadcastingSettingsFilePath}");
        this.Write($"  Season settings: {this.pathProvider.SeasonSettingsFilePath}");
        this.Write($"  Results: {this.pathProvider.ResultFolderPath}");
        this.Write($"  Setups: {this.pathProvider.SetupsFolderPath}");
        this.Write($"  Saved replays: {this.pathProvider.SavedReplaysFolderPath}");
        this.Write($"  Custom cars: {this.pathProvider.CustomCarsFolderPath}");
        this.Write($"  Custom drivers: {this.pathProvider.CustomDriversFolderPath}");
        this.Write($"  Custom liveries: {this.pathProvider.CustomLiveriesFolderPath}");
    }

    private void Write(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }
}
