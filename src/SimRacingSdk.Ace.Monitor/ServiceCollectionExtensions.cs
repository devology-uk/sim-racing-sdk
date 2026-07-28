using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.SharedMemory;
using SimRacingSdk.Ace.SharedMemory.Abstractions;
using SimRacingSdk.Ace.Udp;
using SimRacingSdk.Ace.Udp.Abstractions;

namespace SimRacingSdk.Ace.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAceMonitor(this IServiceCollection services)
    {
        services.UseAceSdk();

        services.TryAddSingleton<IAceSharedMemoryConnectionFactory, AceSharedMemoryConnectionFactory>();
        services.TryAddSingleton<IAceUdpConnectionFactory, AceUdpConnectionFactory>();

        services.AddSingleton<IAceMonitorFactory, AceMonitorFactory>();
        return services;
    }
}
