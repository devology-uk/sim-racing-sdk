using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.SharedMemory.Abstractions;

namespace SimRacingSdk.Ace.SharedMemory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseAceSharedMemory(this IServiceCollection services)
    {
        services.UseAceSdk();

        services.TryAddSingleton<IAceSharedMemoryProvider, AceSharedMemoryProvider>();
        services.TryAddSingleton<IAceSharedMemoryConnectionFactory, AceSharedMemoryConnectionFactory>();
        return services;
    }
}
