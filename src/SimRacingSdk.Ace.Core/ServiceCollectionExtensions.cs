using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ace.Core.Abstractions;

namespace SimRacingSdk.Ace.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAceSdk(this IServiceCollection services)
    {
        services.TryAddSingleton<IAceCarInfoProvider, AceCarInfoProvider>();
        services.TryAddSingleton<IAceCompatibilityChecker, AceCompatibilityChecker>();
        services.TryAddSingleton<IAceLocalConfigProvider, AceLocalConfigProvider>();
        services.TryAddSingleton<IAceNationalityInfoProvider, AceNationalityInfoProvider>();
        services.TryAddSingleton<IAcePathProvider, AcePathProvider>();
        services.TryAddSingleton<IAceTrackInfoProvider, AceTrackInfoProvider>();
        return services;
    }
}
