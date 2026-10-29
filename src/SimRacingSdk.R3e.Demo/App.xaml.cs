using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SimRacingSdk.LogViewer;
using SimRacingSdk.R3e.Core;
using SimRacingSdk.R3e.Demo.Abstractions;
using SimRacingSdk.R3e.Demo.CarExplorer;
using SimRacingSdk.R3e.Demo.Controls.Console;
using SimRacingSdk.R3e.Demo.Demos;
using SimRacingSdk.R3e.Demo.Services;
using SimRacingSdk.R3e.Demo.TrackExplorer;
using SimRacingSdk.R3e.SharedMemory;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.R3e.Demo;

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
        this.logger.LogInformation("Sim Racing SDK Demo for R3E has shutdown.");
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.logger.LogInformation("Sim Racing SDK Demo for R3E has started.");

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

        services.UseR3eSdk();
        services.UseR3eSharedMemory();

        services.AddSingleton<IConsoleLog, ConsoleLog>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ConsoleControlViewModel>();
        services.AddTransient<LogViewerViewModel>();
        services.AddTransient<CarExplorerViewModel>();
        services.AddTransient<TrackExplorerViewModel>();
        services.AddTransient<IGameInfoDemo, GameInfoDemo>();
        services.AddTransient<ISharedMemoryDemo, SharedMemoryDemo>();
        services.AddTransient<ISharedMemoryLog>(sp => new LogMessageSink(sp.GetRequiredService<ILoggerFactory>()
            .CreateLogger("SimRacingSdk.R3e.Demo.Services.SharedMemoryLog")));

        return services.BuildServiceProvider();
    }
}
