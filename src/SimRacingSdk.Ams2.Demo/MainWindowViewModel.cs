using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Demo.Abstractions;
using SimRacingSdk.Ams2.Demo.CarExplorer;
using SimRacingSdk.Ams2.Demo.TrackExplorer;
using SimRacingSdk.LogViewer;

namespace SimRacingSdk.Ams2.Demo;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IAms2CompatibilityChecker ams2CompatibilityChecker;
    private readonly IAms2GameDetector ams2GameDetector;
    private readonly IConsoleLog consoleLog;
    private readonly string logFolderPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\";
    private readonly ILogger<MainWindowViewModel> logger;
    private readonly ISharedMemoryDemo sharedMemoryDemo;
    private readonly CompositeDisposable subscriptionSink = new();
    private readonly IUdpDemo udpDemo;
    
    private bool isDemoCancelled;
    private bool isGameRunning;

    [ObservableProperty]
    private bool isRunningDemo = false;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger,
        IConsoleLog consoleLog,
        IAms2CompatibilityChecker ams2CompatibilityChecker,
        IAms2GameDetector ams2GameDetector,
        IUdpDemo udpDemo,
        ISharedMemoryDemo sharedMemoryDemo)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.ams2CompatibilityChecker = ams2CompatibilityChecker;
        this.ams2GameDetector = ams2GameDetector;
        this.udpDemo = udpDemo;
        this.sharedMemoryDemo = sharedMemoryDemo;
    }

    [RelayCommand]
    private void OpenCarExplorer()
    {
        var carViewerViewModel = App.Current.Services.GetRequiredService<CarExplorerViewModel>();
        var carExplorer = new CarExplorerWindow
        {
            DataContext = carViewerViewModel,
            Owner = App.Current.MainWindow
        };
        carExplorer.Show();
        carViewerViewModel.Init();
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
        var trackViewerViewModel = App.Current.Services.GetRequiredService<TrackExplorerViewModel>();
        var trackExplorer = new TrackExplorerWindow
        {
            DataContext = trackViewerViewModel,
            Owner = App.Current.MainWindow
        };
        trackExplorer.Show();
        trackViewerViewModel.Init();
    }

    [RelayCommand]
    private async Task StartSharedMemoryDemo()
    {
        this.consoleLog.Clear();
        this.StopRunningDemos();
        this.IsRunningDemo = true;
        this.CheckCompatibility();

        if(!this.sharedMemoryDemo.Validate())
        {
            return;
        }

        await this.WaitFormGame();
        if(!this.isGameRunning)
        {
            return;
        }

        this.sharedMemoryDemo.Start();
    }

    [RelayCommand]
    private async Task StartUdpDemo()
    {
        this.consoleLog.Clear();
        this.StopRunningDemos();
        this.IsRunningDemo = true;
        this.CheckCompatibility();

        if(!this.udpDemo.Validate())
        {
            return;
        }

        await this.WaitFormGame();
        if(!this.isGameRunning)
        {
            return;
        }

        this.udpDemo.Start();
    }

    [RelayCommand]
    private void StopDemo()
    {
        this.isDemoCancelled = true;
        this.StopRunningDemos();
    }

    private bool CheckCompatibility()
    {
        this.Log("Checking ACC compatibility with this demo...");
        if(!this.ams2CompatibilityChecker.IsAms2Installed())
        {
            this.Log("AMS2 is not installed. Please install AMS2 and run this demo again.", LogLevel.Warning);
            return false;
        }

        this.Log("AMS2 compatibility check was completed successfully.");
        return true;
    }

    private void HandleGameDetection(bool isDetected)
    {
        if(isDetected)
        {
            this.Log("AMS2 game detected successfully.");
            this.isGameRunning = true;
        }
        else
        {
            this.Log("AMS2 game is not running or has been shutdown.", LogLevel.Warning);
            this.isGameRunning = false;
        }
    }

    private void Log(string message, LogLevel logLevel = LogLevel.Information)
    {
        this.logger.Log(logLevel, message);
        this.consoleLog.Write(message);
    }

    private void StartGameDetection()
    {
        this.Log("Starting AMS2 game detection...");
        this.subscriptionSink.Add(this.ams2GameDetector.Start()
                                      .Subscribe(this.HandleGameDetection));
    }

    private void StopRunningDemos()
    {
        this.udpDemo.Stop();
        this.sharedMemoryDemo.Stop();
        this.IsRunningDemo = false;
    }

    private async Task WaitFormGame()
    {
        this.isDemoCancelled = false;
        this.StartGameDetection();
        while(!this.isGameRunning)
        {
            this.Log("Waiting for AMS2...");
            await Task.Delay(5000);
            if(this.isDemoCancelled)
            {
                return;
            }
        }
    }

    ~MainWindowViewModel()
    {
        this.StopRunningDemos();
        this.subscriptionSink.Dispose();
    }
}