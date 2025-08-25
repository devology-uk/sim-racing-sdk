using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Acc.Core;
using SimRacingSdk.Acc.SharedMemory.Abstractions;

namespace SimRacingSdk.Acc.SharedMemory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccSharedMemory(this IServiceCollection services)
    {
        services.UseAccSdk();

        services.TryAddSingleton<IAccSharedMemoryProvider, AccSharedMemoryProvider>();
        services.TryAddSingleton<IAccSharedMemoryConnectionFactory, AccSharedMemoryConnectionFactory>();
        services.TryAddSingleton<IAccTelemetryConnectionFactory, AccTelemetryConnectionFactory>();
        return services;
    }
}