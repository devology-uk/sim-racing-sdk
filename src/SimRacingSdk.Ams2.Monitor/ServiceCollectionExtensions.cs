using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Ams2.Monitor.Abstractions;
using SimRacingSdk.Ams2.SharedMemory;

namespace SimRacingSdk.Ams2.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAms2Monitor(this IServiceCollection services)
    {
        services.UseAms2SharedMemory();

        services.AddSingleton<IAms2MonitorFactory, Ams2MonitorFactory>();

        return services;
    }
}