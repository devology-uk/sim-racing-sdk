using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Acc.Core;
using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.SharedMemory;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.Udp;
using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccMonitor(this IServiceCollection services)
    {
        services.UseAccSdk();

        services.TryAddSingleton<IAccSharedMemoryConnectionFactory, AccSharedMemoryConnectionFactory>();
        services.TryAddSingleton<IAccUdpConnectionFactory, AccUdpConnectionFactory>();

        services.AddSingleton<IAccMonitorFactory, AccMonitorFactory>();
        return services;
    }
}