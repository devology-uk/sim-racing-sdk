using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ams2.Core;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;

namespace SimRacingSdk.Ams2.SharedMemory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAms2SharedMemory(this IServiceCollection services)
    {
        services.UseAms2Sdk();

        services.TryAddSingleton<IAms2SharedMemoryProvider, Ams2SharedMemoryProvider>();
        services.TryAddSingleton<IAms2SharedMemoryConnectionFactory, Ams2SharedMemoryConnectionFactory>();

        return services;
    }
}