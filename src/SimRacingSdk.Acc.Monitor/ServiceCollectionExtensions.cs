using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.SharedMemory;
using SimRacingSdk.Acc.Udp;

namespace SimRacingSdk.Acc.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccMonitor(this IServiceCollection services)
    {
        services.UseAccSharedMemory();
        services.UseAccUdp();

        services.AddSingleton<IAccMonitorFactory, AccMonitorFactory>();
        return services;
    }
}