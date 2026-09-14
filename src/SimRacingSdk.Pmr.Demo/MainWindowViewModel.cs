using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SimRacingSdk.LogViewer;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Demo.Abstractions;
using SimRacingSdk.Pmr.Demo.CarExplorer;
using SimRacingSdk.Pmr.Demo.TrackExplorer;
using SimRacingSdk.Pmr.Udp;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SimRacingSdk.Pmr.Demo;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IConsoleLog consoleLog;
    private readonly string logFolderPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\";
    private readonly IMonitorDemo monitorDemo;
    private readonly IUdpDemo udpDemo;

    [ObservableProperty]
    private bool isRunningDemo;

    [ObservableProperty]
    private string udpHost = PmrUdpConnection.DefaultHost;

    [ObservableProperty]
    private string udpPort = PmrUdpConnection.DefaultPort.ToString();

    public MainWindowViewModel(IConsoleLog consoleLog,
        IUdpDemo udpDemo,
        IMonitorDemo monitorDemo,
        IPmrLocalConfigProvider pmrLocalConfigProvider)
    {
        this.consoleLog = consoleLog;
        this.udpDemo = udpDemo;
        this.monitorDemo = monitorDemo;

        // Defaults are read once at startup from the game's own settings file, but the fields
        // are editable so a user running other UDP-consuming apps (SimHub, Fanatec App,
        // CrewChief) can point this demo at a relay port instead of the game's actual configured
        // one, since only one process can bind that port.
        var localSettings = pmrLocalConfigProvider.GetLocalSettings();
        if(localSettings != null && !string.IsNullOrWhiteSpace(localSettings.UdpHost) && localSettings.UdpPort > 0)
        {
            this.UdpHost = localSettings.UdpHost;
            this.UdpPort = localSettings.UdpPort.ToString();
        }
    }

    [RelayCommand]
    private void OpenCarExplorer()
    {
        var carExplorerViewModel = App.Current.Services.GetRequiredService<CarExplorerViewModel>();
        var carExplorer = new CarExplorerWindow
        {
            DataContext = carExplorerViewModel,
            Owner = App.Current.MainWindow
        };
        carExplorer.Show();
        carExplorerViewModel.Init();
    }

    [RelayCommand]
    private void OpenLogFolder()
    {
        var currentLogPath = $@"{this.logFolderPath}{DateTime.Now.Date:yyyy-MM-dd}\";
        if(Directory.Exists(currentLogPath))
        {
            Process.Start("explorer.exe", currentLogPath);
        }
    }

    [RelayCommand]
    private void OpenLogViewer()
    {
        var logViewerViewModel = App.Current.Services.GetRequiredService<LogViewerViewModel>();
        var logViewer = new LogViewerWindow
        {
            DataContext = logViewerViewModel,
            Owner = App.Current.MainWindow
        };
        logViewer.Show();
        logViewerViewModel.Init(this.logFolderPath);
    }

    [RelayCommand]
    private void OpenTrackExplorer()
    {
        var trackExplorerViewModel = App.Current.Services.GetRequiredService<TrackExplorerViewModel>();
        var trackExplorer = new TrackExplorerWindow
        {
            DataContext = trackExplorerViewModel,
            Owner = App.Current.MainWindow
        };
        trackExplorer.Show();
        trackExplorerViewModel.Init();
    }

    [RelayCommand]
    private void StartMonitorDemo()
    {
        this.consoleLog.Clear();
        this.StopRunningDemos();

        if(!int.TryParse(this.UdpPort, out var port))
        {
            this.consoleLog.Write($"'{this.UdpPort}' is not a valid port number.");
            return;
        }

        this.IsRunningDemo = true;
        this.monitorDemo.Configure(this.UdpHost, port);

        if(!this.monitorDemo.Validate())
        {
            this.IsRunningDemo = false;
            return;
        }

        this.monitorDemo.Start();
    }

    [RelayCommand]
    private void StartUdpDemo()
    {
        this.consoleLog.Clear();
        this.StopRunningDemos();

        if(!int.TryParse(this.UdpPort, out var port))
        {
            this.consoleLog.Write($"'{this.UdpPort}' is not a valid port number.");
            return;
        }

        this.IsRunningDemo = true;
        this.udpDemo.Configure(this.UdpHost, port);

        if(!this.udpDemo.Validate())
        {
            this.IsRunningDemo = false;
            return;
        }

        this.udpDemo.Start();
    }

    [RelayCommand]
    private void StopDemo()
    {
        this.StopRunningDemos();
    }

    private void StopRunningDemos()
    {
        this.udpDemo.Stop();
        this.monitorDemo.Stop();
        this.IsRunningDemo = false;
    }

    ~MainWindowViewModel()
    {
        this.StopRunningDemos();
    }
}
