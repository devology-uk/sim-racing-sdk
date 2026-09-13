using SimRacingSdk.Pmr.Core.Models.Config;

namespace SimRacingSdk.Pmr.Core.Abstractions;

public interface IPmrLocalConfigProvider
{
    PmrDriverProfile? GetDriverProfile();
    PmrLocalSettings? GetLocalSettings();
}
