using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using System.Windows;
using NLog.Extensions.Logging;
using SimRacingSdk.Ams2.Demo.Controls.Console;
using SimRacingSdk.Ams2.Demo.Abstractions;
using SimRacingSdk.Ams2.Demo.CarExplorer;
using SimRacingSdk.Ams2.Demo.Demos;
using SimRacingSdk.LogViewer;
using SimRacingSdk.Ams2.Demo.Services;
using SimRacingSdk.Ams2.Demo.TrackExplorer;
using SimRacingSdk.Ams2.SharedMemory;
using SimRacingSdk.Ams2.Udp;

namespace SimRacingSdk.Ams2.Demo;

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
        this.logger.LogInformation("Sim Racing SDK Demo for AMS2 has shutdown.");
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.logger.LogInformation("Sim Racing SDK Demo for AMS2 has started.");

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

        services.UseAms2Udp();
        services.UseAms2SharedMemory();

        services.AddSingleton<IConsoleLog, ConsoleLog>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ConsoleControlViewModel>();
        services.AddTransient<LogViewerViewModel>();
        services.AddTransient<CarExplorerViewModel>();
        services.AddTransient<TrackExplorerViewModel>();
        services.AddTransient<IUdpDemo, UdpDemo>();
        services.AddTransient<IUdpLog, UdpLog>();
        services.AddTransient<ISharedMemoryDemo, SharedMemoryDemo>();
        services.AddTransient<ISharedMemoryLog, SharedMemoryLog>();

        return services.BuildServiceProvider();
    }
}