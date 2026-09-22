using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SimRacingSdk.LogViewer;
using SimRacingSdk.Pmr.DataManager.Abstractions;
using SimRacingSdk.Pmr.DataManager.Cars;
using SimRacingSdk.Pmr.DataManager.Controls.Console;
using SimRacingSdk.Pmr.DataManager.Services;
using SimRacingSdk.Pmr.DataManager.Setups;
using SimRacingSdk.Pmr.DataManager.Storage;
using SimRacingSdk.Pmr.DataManager.Tracks;

namespace SimRacingSdk.Pmr.DataManager;

public partial class App : Application
{
    private readonly ILogger<App> logger;

    public App()
    {
        this.Services = this.ConfigureServices();
        this.logger = this.Services.GetRequiredService<ILogger<App>>();
    }

    public new static App Current => (App)Application.Current;

    public IServiceProvider Services { get; }

    protected override void OnExit(ExitEventArgs e)
    {
        this.logger.LogInformation("Sim Racing SDK Data Manager for PMR has shutdown.");
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.logger.LogInformation("Sim Racing SDK Data Manager for PMR has started.");

        this.MainWindow = new MainWindow
        {
            DataContext = this.Services.GetRequiredService<MainWindowViewModel>()
        };
        this.MainWindow.Show();
    }

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder =>
                             {
                                 builder.ClearProviders();
                                 builder.AddNLog();
                             });

        services.AddSingleton<IConsoleLog, ConsoleLog>();
        services.AddSingleton<IDataPathProvider, DataPathProvider>();
        services.AddSingleton<ICarRepository, CarRepository>();
        services.AddSingleton<IPmrCarProviderGenerator, PmrCarProviderGenerator>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ConsoleControlViewModel>();
        services.AddTransient<LogViewerViewModel>();
        services.AddSingleton<CarsViewModel>();
        services.AddSingleton<TracksViewModel>();
        services.AddSingleton<SetupsViewModel>();

        return services.BuildServiceProvider();
    }
}
