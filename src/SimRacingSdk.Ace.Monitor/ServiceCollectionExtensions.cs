using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.SharedMemory;

namespace SimRacingSdk.Ace.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAceMonitor(this IServiceCollection services)
    {
        services.UseAceSharedMemory();

        services.AddSingleton<IAceMonitorFactory, AceMonitorFactory>();
        return services;
    }
}
