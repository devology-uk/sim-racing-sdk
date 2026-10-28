using SimRacingSdk.R3e.Core.Models.Config;

namespace SimRacingSdk.R3e.Core.Abstractions;

public interface IR3eLocalConfigProvider
{
    R3eGameOptions? GetGameOptions();
}
