namespace SimRacingSdk.Acc.Core.Abstractions;

public interface IAccCompatibilityChecker
{
    bool HasCustomCars();
    bool HasCustomLiveries();
    bool HasDrivenAtLeastOneOfflineSession();
    bool HasSavedSetup();
    bool HasValidBroadcastingSettings();
    bool IsAccInstalled();
    bool IsAccountAvailable();
}