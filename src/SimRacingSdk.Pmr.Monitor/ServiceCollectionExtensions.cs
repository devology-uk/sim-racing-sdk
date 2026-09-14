using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Pmr.Monitor.Abstractions;
using SimRacingSdk.Pmr.Udp;

namespace SimRacingSdk.Pmr.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UsePmrMonitor(this IServiceCollection services)
    {
        services.UsePmrUdp();

        services.AddSingleton<IPmrMonitorFactory, PmrMonitorFactory>();
        return services;
    }
}
