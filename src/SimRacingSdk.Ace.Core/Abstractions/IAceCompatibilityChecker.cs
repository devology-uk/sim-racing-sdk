namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAceCompatibilityChecker
{
    bool HasCustomCars();
    bool HasCustomLiveries();
    bool HasDrivenAtLeastOneOfflineSession();
    bool HasSavedSetup();
    bool HasValidBroadcastingSettings();
    bool IsAceInstalled();
    bool IsAccountAvailable();
}
