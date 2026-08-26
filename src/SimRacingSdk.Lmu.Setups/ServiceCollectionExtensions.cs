using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Lmu.Setups.Abstractions;
using SimRacingSdk.Lmu.Setups.Services;

namespace SimRacingSdk.Lmu.Setups;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseLmuSetupsSdk(this IServiceCollection services)
    {
        services.TryAddSingleton<ILmuSetupProvider, LmuSetupProvider>();
        return services;
    }
}
