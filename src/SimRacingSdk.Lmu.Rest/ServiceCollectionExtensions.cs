using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Lmu.Rest.Abstractions;

namespace SimRacingSdk.Lmu.Rest;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseLmuRest(this IServiceCollection services)
    {
        services.TryAddSingleton<ILmuRestClientFactory, LmuRestClientFactory>();
        services.TryAddSingleton<ILmuRestEndpointCatalog, LmuRestEndpointCatalog>();
        return services;
    }
}
