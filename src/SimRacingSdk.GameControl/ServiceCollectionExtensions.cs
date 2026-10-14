using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.GameControl.Abstractions;

namespace SimRacingSdk.GameControl;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseGameControl(this IServiceCollection services)
    {
        services.TryAddSingleton<IGameWindowFinder, GameWindowFinder>();
        services.TryAddSingleton<IGameWindowActivator, GameWindowActivator>();
        services.TryAddSingleton<IKeystrokeSender, KeystrokeSender>();
        services.TryAddSingleton<IGameKeyboard, GameKeyboard>();
        return services;
    }
}
