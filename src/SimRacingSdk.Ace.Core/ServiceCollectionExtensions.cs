using Microsoft.Extensions.DependencyInjection;

namespace SimRacingSdk.Ace.Core;

public static class ServiceCollectionExtensions
{
    // Populated once Ace.Core grows path/config/car-info providers, mirroring SimRacingSdk.Acc.Core's UseAccSdk.
    public static IServiceCollection UseAceSdk(this IServiceCollection services)
    {
        return services;
    }
}
