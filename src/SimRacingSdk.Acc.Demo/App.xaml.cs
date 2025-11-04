using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.Demo.Controls.Console;
using SimRacingSdk.Acc.Demo.Demos;
using SimRacingSdk.Acc.Demo.Services;
using SimRacingSdk.Acc.Monitor;
using SimRacingSdk.Acc.SharedMemory;
using SimRacingSdk.Acc.Udp;
using SimRacingSdk.LogViewer;

namespace SimRacingSdk.Acc.Demo;

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
        this.logger.LogInformation("Sim Racing SDK Demo for ACC has shutdown.");
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.logger.LogInformation("Sim Racing SDK Demo for ACC has started.");

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

        services.UseAccUdp();
        services.UseAccSharedMemory();
        services.UseAccMonitor();

        services.AddSingleton<IConsoleLog, ConsoleLog>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ConsoleControlViewModel>();
        services.AddTransient<LogViewerViewModel>();
        services.AddTransient<ISharedMemoryDemo, SharedMemoryDemo>();
        services.AddTransient<IUdpDemo, UdpDemo>();
        services.AddTransient<IMonitorDemo, MonitorDemo>();
        services.AddTransient<IUdpLog, UdpLog>();
        services.AddTransient<ISharedMemoryLog, SharedMemoryLog>();
        services.AddTransient<IMonitorLog, MonitorLog>();

        return services.BuildServiceProvider();
    }
}