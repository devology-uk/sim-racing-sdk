using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ams2.Core.Abstractions;

namespace SimRacingSdk.Ams2.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAms2Sdk(this IServiceCollection services)
    {
        services.TryAddSingleton<IAms2CompatibilityChecker, Ams2CompatibilityChecker>();
        services.TryAddSingleton<IAms2GameDetector, Ams2GameDetector>();
        services.TryAddSingleton<IAms2PathProvider, Ams2PathProvider>();
        services.TryAddSingleton<IAms2CarInfoProvider, Ams2CarInfoProvider>();
        services.TryAddSingleton<IAms2TrackInfoProvider, Ams2TrackInfoProvider>();
        return services;
    }
}