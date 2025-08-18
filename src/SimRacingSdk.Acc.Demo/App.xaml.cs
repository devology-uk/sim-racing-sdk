using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SimRacingSdk.Acc.Core;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.Demo.Controls.Console;
using SimRacingSdk.Acc.Demo.Services;
using SimRacingSdk.Acc.SharedMemory;
using SimRacingSdk.Acc.Udp;

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
        this.logger.LogInformation("Sim Racing SDL Demo for ACC.");
        LogManager.Shutdown();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.logger.LogInformation("Sim Racing SDL Demo for ACC is starting up.");

        this.MainWindow = new MainWindow
        {
            DataContext = this.Services.GetRequiredService<MainWindowViewModel>()
        };
        this.MainWindow.Show();
    }

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.UseAccSdk();
        services.UseAccUdp();
        services.UseAccSharedMemory();

        services.AddSingleton<IConsoleLog, ConsoleLog>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ConsoleControlViewModel>();

        services.AddLogging(builder =>
                            {
                                builder.ClearProviders();
                                builder.AddNLog();
                            });

        return services.BuildServiceProvider();
    }
}