using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Pmr.Core.Abstractions;

namespace SimRacingSdk.Pmr.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UsePmrSdk(this IServiceCollection services)
    {
        services.TryAddSingleton<ISteamInfoProvider, SteamInfoProvider>();
        services.TryAddSingleton<IPmrCarInfoProvider, PmrCarInfoProvider>();
        services.TryAddSingleton<IPmrGameDetector, PmrGameDetector>();
        services.TryAddSingleton<IPmrLocalConfigProvider, PmrLocalConfigProvider>();
        services.TryAddSingleton<IPmrPathProvider, PmrPathProvider>();
        services.TryAddSingleton<IPmrTrackInfoProvider, PmrTrackInfoProvider>();
        return services;
    }
}
