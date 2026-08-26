using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Acc.Setups.Abstractions;
using SimRacingSdk.Acc.Setups.Services;

namespace SimRacingSdk.Acc.Setups;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccSetupsSdk(this IServiceCollection services)
    {
        services.TryAddSingleton<IAccSetupProvider, AccSetupProvider>();
        return services;
    }
}
