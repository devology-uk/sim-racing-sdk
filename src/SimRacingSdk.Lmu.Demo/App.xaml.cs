using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SimRacingSdk.Lmu.Core;
using SimRacingSdk.Lmu.Demo.Abstractions;
using SimRacingSdk.Lmu.Demo.CarExplorer;
using SimRacingSdk.Lmu.Demo.Controls.Console;
using SimRacingSdk.Lmu.Demo.Demos;
using SimRacingSdk.Lmu.Demo.ResultExplorer;
using SimRacingSdk.Lmu.Demo.Services;
using SimRacingSdk.Lmu.Demo.TrackExplorer;
using SimRacingSdk.Lmu.Monitor;
using SimRacingSdk.Lmu.SharedMemory;
using SimRacingSdk.LogViewer;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.Lmu.Demo;

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
        this.logger.LogInformation("Sim Racing SDK Demo for LMU has shutdown.");
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.logger.LogInformation("Sim Racing SDK Demo for LMU has started.");

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

        services.UseLmuSdk();
        services.UseLmuSharedMemory();
        services.UseLmuMonitor();

        services.AddSingleton<IConsoleLog, ConsoleLog>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ConsoleControlViewModel>();
        services.AddTransient<LogViewerViewModel>();
        services.AddTransient<CarExplorerViewModel>();
        services.AddTransient<TrackExplorerViewModel>();
        services.AddTransient<ResultExplorerViewModel>();
        services.AddTransient<ISharedMemoryDemo, SharedMemoryDemo>();
        services.AddTransient<ISharedMemoryLog>(sp => new LogMessageSink(sp.GetRequiredService<ILoggerFactory>()
            .CreateLogger("SimRacingSdk.Lmu.Demo.Services.SharedMemoryLog")));
        services.AddTransient<IMonitorDemo, MonitorDemo>();
        services.AddTransient<IMonitorLog>(sp => new LogMessageSink(sp.GetRequiredService<ILoggerFactory>()
            .CreateLogger("SimRacingSdk.Lmu.Demo.Services.MonitorLog")));

        return services.BuildServiceProvider();
    }
}