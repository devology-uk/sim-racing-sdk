using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SimRacingSdk.LogViewer;
using SimRacingSdk.Pmr.Demo.Abstractions;
using SimRacingSdk.Pmr.Demo.CarExplorer;
using SimRacingSdk.Pmr.Demo.Controls.Console;
using SimRacingSdk.Pmr.Demo.Demos;
using SimRacingSdk.Pmr.Demo.Services;
using SimRacingSdk.Pmr.Demo.TrackExplorer;
using SimRacingSdk.Pmr.Monitor;
using SimRacingSdk.Pmr.Udp;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.Pmr.Demo;

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
        this.logger.LogInformation("Sim Racing SDK Demo for PMR has shutdown.");
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.logger.LogInformation("Sim Racing SDK Demo for PMR has started.");

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

        services.UsePmrUdp();
        services.UsePmrMonitor();

        services.AddSingleton<IConsoleLog, ConsoleLog>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ConsoleControlViewModel>();
        services.AddTransient<LogViewerViewModel>();
        services.AddTransient<CarExplorerViewModel>();
        services.AddTransient<TrackExplorerViewModel>();
        services.AddTransient<IUdpDemo, UdpDemo>();
        services.AddTransient<IUdpLog>(sp => new LogMessageSink(sp.GetRequiredService<ILoggerFactory>()
            .CreateLogger("SimRacingSdk.Pmr.Demo.Services.UdpLog")));
        services.AddTransient<IMonitorDemo, MonitorDemo>();
        services.AddTransient<IMonitorLog>(sp => new LogMessageSink(sp.GetRequiredService<ILoggerFactory>()
            .CreateLogger("SimRacingSdk.Pmr.Demo.Services.MonitorLog")));

        return services.BuildServiceProvider();
    }
}
