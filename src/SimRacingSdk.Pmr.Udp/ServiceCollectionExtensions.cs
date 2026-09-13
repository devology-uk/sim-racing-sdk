using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Pmr.Core;
using SimRacingSdk.Pmr.Udp.Abstractions;

namespace SimRacingSdk.Pmr.Udp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UsePmrUdp(this IServiceCollection services)
    {
        services.UsePmrSdk();
        services.TryAddSingleton<IPmrUdpConnectionFactory, PmrUdpConnectionFactory>();
        return services;
    }
}
