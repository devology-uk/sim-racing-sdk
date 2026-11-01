using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.R3e.Monitor.Abstractions;
using SimRacingSdk.R3e.SharedMemory;

namespace SimRacingSdk.R3e.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseR3eMonitor(this IServiceCollection services)
    {
        services.UseR3eSharedMemory();

        services.TryAddSingleton<IR3eMonitorFactory, R3eMonitorFactory>();
        return services;
    }
}
