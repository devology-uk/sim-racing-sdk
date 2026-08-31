using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Services;

namespace SimRacingSdk.Lmu.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseLmuSdk(this IServiceCollection services)
    {
        services.TryAddSingleton<ISteamInfoProvider, SteamInfoProvider>();
        services.TryAddSingleton<ILmuCarInfoProvider, LmuCarInfoProvider>();
        services.TryAddSingleton<ILmuGameDataProvider, LmuGameDataProvider>();
        services.TryAddSingleton<ILmuGameDetector, LmuGameDetector>();
        services.TryAddSingleton<ILmuPathProvider, LmuPathProvider>();
        services.TryAddSingleton<ILmuTrackInfoProvider, LmuTrackInfoProvider>();
        return services;
    }
}