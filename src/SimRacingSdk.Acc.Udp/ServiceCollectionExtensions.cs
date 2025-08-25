using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Acc.Core;
using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Udp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccUdp(this IServiceCollection services)
    {
        services.UseAccSdk();
        services.TryAddSingleton<IAccUdpConnectionFactory, AccUdpConnectionFactory>();
        return services;
    }
}