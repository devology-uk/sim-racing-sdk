using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Acc.Core.Abstractions;

namespace SimRacingSdk.Acc.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccSdk(this IServiceCollection services)
    {
        services.TryAddSingleton<IAccCarInfoProvider, AccCarInfoProvider>();
        services.TryAddSingleton<IAccTrackInfoProvider, AccTrackInfoProvider>();
        services.TryAddSingleton<IAccCompatibilityChecker, AccCompatibilityChecker>();
        services.TryAddSingleton<IAccGameDetector, AccGameDetector>();
        services.TryAddSingleton<IAccLocalConfigProvider, AccLocalConfigProvider>();
        services.TryAddSingleton<IAccNationalityInfoProvider, AccNationalityInfoProvider>();
        services.TryAddSingleton<IAccPathProvider, AccPathProvider>();
        return services;
    }
}