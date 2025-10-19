using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ams2.Core;
using SimRacingSdk.Ams2.Monitor.Abstractions;
using SimRacingSdk.Ams2.SharedMemory;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;

namespace SimRacingSdk.Ams2.Monitor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAms2Monitor(this IServiceCollection services)
    {
        services.UseAms2Sdk();

        services.TryAddSingleton<IAms2SharedMemoryConnectionFactory, Ams2SharedMemoryConnectionFactory>();

        services.AddSingleton<IAms2MonitorFactory, Ams2MonitorFactory>();

        return services;
    }
}