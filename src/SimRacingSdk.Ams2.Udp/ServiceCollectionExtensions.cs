using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ams2.Core;
using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAms2Udp(this IServiceCollection services)
    {
        services.UseAms2Sdk();
        services.TryAddSingleton<IAms2UdpConnectionFactory, Ams2UdpConnectionFactory>();
        return services;
    }
}