using SimRacingSdk.Acc.Core.Abstractions;

namespace SimRacingSdk.Acc.Core;

public class AccCompatibilityChecker : IAccCompatibilityChecker
{
    private static AccCompatibilityChecker? singletonInstance;
    private readonly IAccLocalConfigProvider accLocalConfigProvider;
    private readonly IAccPathProvider accPathProvider;

    public AccCompatibilityChecker(IAccPathProvider accPathProvider,
        IAccLocalConfigProvider accLocalConfigProvider)
    {
        this.accPathProvider = accPathProvider;
        this.accLocalConfigProvider = accLocalConfigProvider;
    }

    public static AccCompatibilityChecker Instance =>
        singletonInstance ??=
            new AccCompatibilityChecker(AccPathProvider.Instance, AccLocalConfigProvider.Instance);

    public bool HasCustomCars()
    {
        return this.IsAccInstalled() && Directory
                                        .EnumerateDirectories(this.accPathProvider.CustomCarsFolderPath)
                                        .ToList()
                                        .Any();
    }

    public bool HasCustomLiveries()
    {
        return this.IsAccInstalled() && Directory
                                        .EnumerateDirectories(this.accPathProvider.CustomLiveriesFolderPath)
                                        .ToList()
                                        .Any();
    }

    public bool HasDrivenAtLeastOneOfflineSession()
    {
        return this.IsAccInstalled() && Directory.GetFiles(this.accPathProvider.ResultFolderPath)
                                                 ?.Length > 0;
    }

    public bool HasSavedSetup()
    {
        return this.IsAccInstalled() && Directory.Exists(this.accPathProvider.SetupsFolderPath);
    }

    public bool HasValidBroadcastingSettings()
    {
        var broadcastSettings = this.accLocalConfigProvider.GetBroadcastingSettings();
        return broadcastSettings is
        {
            UdpListenerPort: > 1023
        };
    }

    public bool IsAccInstalled()
    {
        return Directory.Exists(this.accPathProvider.DocumentsFolderPath);
    }

    public bool IsAccountAvailable()
    {
        return this.IsAccInstalled() && File.Exists(this.accPathProvider.AccountFilePath);
    }
}