using SimRacingSdk.Ace.Core.Abstractions;

namespace SimRacingSdk.Ace.Core;

public class AceCompatibilityChecker : IAceCompatibilityChecker
{
    private static AceCompatibilityChecker? singletonInstance;
    private readonly IAceLocalConfigProvider aceLocalConfigProvider;
    private readonly IAcePathProvider acePathProvider;

    public AceCompatibilityChecker(IAcePathProvider acePathProvider,
        IAceLocalConfigProvider aceLocalConfigProvider)
    {
        this.acePathProvider = acePathProvider;
        this.aceLocalConfigProvider = aceLocalConfigProvider;
    }

    public static AceCompatibilityChecker Instance =>
        singletonInstance ??=
            new AceCompatibilityChecker(AcePathProvider.Instance, AceLocalConfigProvider.Instance);

    public bool HasCustomCars()
    {
        return this.IsAceInstalled() && Directory
                                        .EnumerateDirectories(this.acePathProvider.CustomCarsFolderPath)
                                        .ToList()
                                        .Any();
    }

    public bool HasCustomLiveries()
    {
        return this.IsAceInstalled() && Directory
                                        .EnumerateDirectories(this.acePathProvider.CustomLiveriesFolderPath)
                                        .ToList()
                                        .Any();
    }

    public bool HasDrivenAtLeastOneOfflineSession()
    {
        return this.IsAceInstalled() && Directory.GetFiles(this.acePathProvider.ResultFolderPath)
                                                 ?.Length > 0;
    }

    public bool HasSavedSetup()
    {
        return this.IsAceInstalled() && Directory.Exists(this.acePathProvider.SetupsFolderPath);
    }

    public bool HasValidBroadcastingSettings()
    {
        var broadcastSettings = this.aceLocalConfigProvider.GetBroadcastingSettings();
        return broadcastSettings is
        {
            UdpListenerPort: > 1023
        };
    }

    public bool IsAceInstalled()
    {
        return Directory.Exists(this.acePathProvider.DocumentsFolderPath);
    }

    public bool IsAccountAvailable()
    {
        return this.IsAceInstalled() && File.Exists(this.acePathProvider.AccountFilePath);
    }
}
