using SimRacingSdk.Acc.Core.Models.Config;

namespace SimRacingSdk.Acc.Core.Abstractions;

public interface IAccLocalConfigProvider
{
    Account? GetAccount();
    BroadcastingSettings? GetBroadcastingSettings();
    void SaveBroadcastingSettings(BroadcastingSettings settings);
}