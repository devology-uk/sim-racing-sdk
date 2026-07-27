using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.Udp.Abstractions;

namespace SimRacingSdk.Ace.Udp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAceUdp(this IServiceCollection services)
    {
        services.UseAceSdk();
        services.TryAddSingleton<IAceUdpConnectionFactory, AceUdpConnectionFactory>();
        return services;
    }
}
