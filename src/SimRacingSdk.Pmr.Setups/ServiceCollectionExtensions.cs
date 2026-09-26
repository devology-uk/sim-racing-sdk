using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Pmr.Core;
using SimRacingSdk.Pmr.Setups.Abstractions;
using SimRacingSdk.Pmr.Setups.Services;

namespace SimRacingSdk.Pmr.Setups;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UsePmrSetupsSdk(this IServiceCollection services)
    {
        services.UsePmrSdk();
        services.TryAddSingleton<IPmrSetupMapProvider, PmrSetupMapProvider>();
        services.TryAddSingleton<IPmrSetupPresenter, PmrSetupPresenter>();
        services.TryAddSingleton<IPmrSetupProvider, PmrSetupProvider>();
        return services;
    }
}
