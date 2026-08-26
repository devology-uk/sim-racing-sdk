using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ace.Setups.Abstractions;
using SimRacingSdk.Ace.Setups.Services;

namespace SimRacingSdk.Ace.Setups;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAceSetupsSdk(this IServiceCollection services)
    {
        services.TryAddSingleton<IAceSetupProvider, AceSetupProvider>();
        return services;
    }
}
