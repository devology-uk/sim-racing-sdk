namespace SimRacingSdk.Acc.Core.Abstractions;

public interface IAccCompatibilityChecker
{
    bool HasCustomCars();
    bool HasCustomLiveries();
    bool HasDrivenAtLeastOneSession();
    bool HasSavedSetup();
    bool HasValidBroadcastingSettings();
    bool IsAccInstalled();
    bool IsAccountAvailable();
}