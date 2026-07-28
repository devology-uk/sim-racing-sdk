using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SimRacingSdk.Ace.Demo.Abstractions;
using SimRacingSdk.Ace.Demo.CarExplorer;
using SimRacingSdk.Ace.Demo.Controls.Console;
using SimRacingSdk.Ace.Demo.Demos;
using SimRacingSdk.Ace.Demo.Services;
using SimRacingSdk.Ace.Demo.TrackExplorer;
using SimRacingSdk.Ace.Monitor;
using SimRacingSdk.Ace.SharedMemory;
using SimRacingSdk.Ace.Udp;
using SimRacingSdk.LogViewer;

namespace SimRacingSdk.Ace.Demo;

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
        this.logger.LogInformation("Sim Racing SDK Demo for ACE has shutdown.");
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.logger.LogInformation("Sim Racing SDK Demo for ACE has started.");

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

        services.UseAceUdp();
        services.UseAceSharedMemory();
        services.UseAceMonitor();

        services.AddSingleton<IConsoleLog, ConsoleLog>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ConsoleControlViewModel>();
        services.AddTransient<LogViewerViewModel>();
        services.AddTransient<CarExplorerViewModel>();
        services.AddTransient<TrackExplorerViewModel>();
        services.AddTransient<ISharedMemoryDemo, SharedMemoryDemo>();
        services.AddTransient<IUdpDemo, UdpDemo>();
        services.AddTransient<IMonitorDemo, MonitorDemo>();
        services.AddTransient<IUdpLog, UdpLog>();
        services.AddTransient<ISharedMemoryLog, SharedMemoryLog>();
        services.AddTransient<IMonitorLog, MonitorLog>();

        return services.BuildServiceProvider();
    }
}
