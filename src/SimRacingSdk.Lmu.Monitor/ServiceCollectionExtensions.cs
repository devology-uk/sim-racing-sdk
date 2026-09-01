using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Lmu.Monitor.Abstractions;
using SimRacingSdk.Lmu.SharedMemory;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;

namespace SimRacingSdk.Lmu.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseLmuMonitor(this IServiceCollection services)
    {
        services.UseLmuSharedMemory();

        services.TryAddSingleton<ILmuSharedMemoryConnectionFactory, LmuSharedMemoryConnectionFactory>();

        services.AddSingleton<ILmuMonitorFactory, LmuMonitorFactory>();
        return services;
    }
}
