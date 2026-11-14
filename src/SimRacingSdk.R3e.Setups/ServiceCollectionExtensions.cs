using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.R3e.Core;
using SimRacingSdk.R3e.Setups.Abstractions;
using SimRacingSdk.R3e.Setups.Services;

namespace SimRacingSdk.R3e.Setups;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseR3eSetupsSdk(this IServiceCollection services)
    {
        services.UseR3eSdk();
        services.TryAddSingleton<IR3eSetupMapProvider, R3eSetupMapProvider>();
        services.TryAddSingleton<IR3eSetupProvider, R3eSetupProvider>();
        return services;
    }
}
