using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SimRacingSdk.DataManager.Shared.Shell;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SimRacingSdk.DataManager.Shared;

// The base for each game's App - its App.xaml supplies the page DataTemplates, and the code-behind
// supplies the options and registers the game's pages.
public abstract class DataManagerApplication : Application
{
    private ILogger? logger;

    protected abstract DataManagerOptions Options { get; }

    protected abstract void ConfigureGameServices(IServiceCollection services);

    protected override void OnExit(ExitEventArgs e)
    {
        this.logger?.LogInformation("{Title} has shutdown.", this.Options.Title);
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = this.BuildServices();
        this.logger = services.GetRequiredService<ILogger<DataManagerApplication>>();
        this.logger.LogInformation("{Title} has started.", this.Options.Title);

        this.MainWindow = services.GetRequiredService<ShellWindow>();
        this.MainWindow.Show();
    }

    private IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder =>
                             {
                                 builder.ClearProviders();
                                 builder.AddNLog();
                             });

        this.ConfigureGameServices(services);
        services.UseDataManagerShell(this.Options);

        return services.BuildServiceProvider();
    }
}
