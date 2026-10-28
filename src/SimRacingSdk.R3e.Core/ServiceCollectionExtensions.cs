using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.R3e.Core.Abstractions;

namespace SimRacingSdk.R3e.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseR3eSdk(this IServiceCollection services)
    {
        services.TryAddSingleton<ISteamInfoProvider, SteamInfoProvider>();
        services.TryAddSingleton<IR3eCompatibilityChecker, R3eCompatibilityChecker>();
        services.TryAddSingleton<IR3eGameDetector, R3eGameDetector>();
        services.TryAddSingleton<IR3eGameOptionsReader, R3eGameOptionsReader>();
        services.TryAddSingleton<IR3eLocalConfigProvider, R3eLocalConfigProvider>();
        services.TryAddSingleton<IR3ePathProvider, R3ePathProvider>();
        return services;
    }
}
