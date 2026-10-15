using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SimRacingSdk.DataManager.Shared.Navigation;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Shell;
using SimRacingSdk.DataManager.Shared.Storage;
using SimRacingSdk.LogViewer;

namespace SimRacingSdk.DataManager.Shared;

public static class ServiceCollectionExtensions
{
    // Registered after the game's own services, so a game can supply its own session state type or
    // unsaved-changes saver first and these TryAdds leave them alone.
    public static IServiceCollection UseDataManagerShell(this IServiceCollection services, DataManagerOptions options)
    {
        services.AddSingleton(options);
        services.TryAddSingleton<IDataPathProvider, DataPathProvider>();
        services.TryAddSingleton<ISessionStateStore, SessionStateStore<SessionState>>();
        services.TryAddSingleton<IUserConfirmation, MessageBoxUserConfirmation>();
        services.TryAddSingleton<IUnsavedChangesSaver, NoUnsavedChanges>();
        services.AddTransient<LogViewerViewModel>();
        services.AddSingleton<Func<LogViewerViewModel>>(provider => () => provider.GetRequiredService<LogViewerViewModel>());
        services.AddTransient<ShellViewModel>();
        services.AddTransient<ShellWindow>();
        return services;
    }

    public static IServiceCollection UseCatalogRepository<TEntity>(this IServiceCollection services, string entityName)
        where TEntity : ICatalogEntity
    {
        return services.AddSingleton<IEntityRepository<TEntity>>(provider =>
            new JsonEntityRepository<TEntity>(provider.GetRequiredService<IDataPathProvider>(), entityName));
    }

    // Registers the page view model and its nav entry; pages appear in registration order.
    public static IServiceCollection UsePage<TPageViewModel>(this IServiceCollection services, string title)
        where TPageViewModel : class
    {
        services.AddSingleton<TPageViewModel>();
        return services.AddSingleton(provider => new NavigationItem
        {
            Title = title,
            PageViewModel = provider.GetRequiredService<TPageViewModel>()
        });
    }
}
