using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Udp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAccUdp(this IServiceCollection services)
    {
        services.AddSingleton<IAccUdpConnectionFactory, AccUdpConnectionFactory>();
        return services;
    }
}