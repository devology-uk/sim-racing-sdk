using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Acc.Core.Abstractions;

namespace SimRacingSdk.Acc.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccSdk(this IServiceCollection services)
    {
        services.AddSingleton<IAccCarInfoProvider, AccCarInfoProvider>();
        services.AddSingleton<IAccCompatibilityChecker, AccCompatibilityChecker>();
        services.AddSingleton<IAccGameDetector, AccGameDetector>();
        services.AddSingleton<IAccLocalConfigProvider, AccLocalConfigProvider>();
        services.AddSingleton<IAccNationalityInfoProvider, AccNationalityInfoProvider>();
        services.AddSingleton<IAccPathProvider, AccPathProvider>();
        return services;
    }
}