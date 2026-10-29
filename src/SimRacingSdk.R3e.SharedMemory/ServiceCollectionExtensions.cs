using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.R3e.Core;
using SimRacingSdk.R3e.SharedMemory.Abstractions;

namespace SimRacingSdk.R3e.SharedMemory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseR3eSharedMemory(this IServiceCollection services)
    {
        services.UseR3eSdk();

        services.TryAddSingleton<IR3eSharedMemoryProvider, R3eSharedMemoryProvider>();
        services.TryAddSingleton<IR3eSharedMemoryConnectionFactory, R3eSharedMemoryConnectionFactory>();
        return services;
    }
}
