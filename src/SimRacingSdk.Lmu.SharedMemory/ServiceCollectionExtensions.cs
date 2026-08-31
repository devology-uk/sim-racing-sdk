using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Lmu.Core;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;

namespace SimRacingSdk.Lmu.SharedMemory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseLmuSharedMemory(this IServiceCollection services)
    {
        services.UseLmuSdk();

        services.TryAddSingleton<ILmuSharedMemoryProvider, LmuSharedMemoryProvider>();
        services.TryAddSingleton<ILmuSharedMemoryConnectionFactory, LmuSharedMemoryConnectionFactory>();
        return services;
    }
}
