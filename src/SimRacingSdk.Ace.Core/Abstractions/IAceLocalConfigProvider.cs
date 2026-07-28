using SimRacingSdk.Ace.Core.Models.Config;

namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAceLocalConfigProvider
{
    Account? GetAccount();
    BroadcastingSettings? GetBroadcastingSettings();
    void SaveBroadcastingSettings(BroadcastingSettings settings);
}
