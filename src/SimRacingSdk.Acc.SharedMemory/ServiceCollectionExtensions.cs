using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Acc.SharedMemory.Abstractions;

namespace SimRacingSdk.Acc.SharedMemory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccSharedMemory(this IServiceCollection services)
    {
        services.AddSingleton<IAccSharedMemoryProvider, AccSharedMemoryProvider>();
        services.AddSingleton<IAccSharedMemoryConnectionFactory, AccSharedMemoryConnectionFactory>();
        services.AddSingleton<IAccTelemetryConnectionFactory, AccTelemetryConnectionFactory>();
        return services;
    }
}