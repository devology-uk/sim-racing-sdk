using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimRacingSdk.LogViewer;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Demo.Abstractions;
using SimRacingSdk.R3e.Demo.CarExplorer;
using SimRacingSdk.R3e.Demo.TrackExplorer;

namespace SimRacingSdk.R3e.Demo;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IConsoleLog consoleLog;
    private readonly IGameInfoDemo gameInfoDemo;
    private readonly string logFolderPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\";
    private readonly ILogger<MainWindowViewModel> logger;
    private readonly IR3eGameDetector r3eGameDetector;
    private readonly ISharedMemoryDemo sharedMemoryDemo;
    private readonly CompositeDisposable subscriptionSink = new();

    private bool isDemoCancelled;
    private bool isGameRunning;

    [ObservableProperty]
    private bool isRunningDemo;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger,
        IConsoleLog consoleLog,
        IGameInfoDemo gameInfoDemo,
        IR3eGameDetector r3eGameDetector,
        ISharedMemoryDemo sharedMemoryDemo)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.gameInfoDemo = gameInfoDemo;
        this.r3eGameDetector = r3eGameDetector;
        this.sharedMemoryDemo = sharedMemoryDemo;
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
    private void ShowGameInfo()
    {
        this.consoleLog.Clear();
        this.gameInfoDemo.Show();
    }

    [RelayCommand]
    private async Task StartSharedMemoryDemo()
    {
        this.consoleLog.Clear();
        this.StopRunningDemos();
        this.IsRunningDemo = true;

        if(!this.sharedMemoryDemo.Validate())
        {
            return;
        }

        await this.WaitForGame();
        if(!this.isGameRunning)
        {
            return;
        }

        this.sharedMemoryDemo.Start();
    }

    [RelayCommand]
    private void StopDemo()
    {
        this.isDemoCancelled = true;
        this.StopRunningDemos();
    }

    private void HandleGameDetection(bool isDetected)
    {
        if(isDetected)
        {
            this.Log("R3E game detected successfully.");
            this.isGameRunning = true;
        }
        else
        {
            this.Log("R3E game is not running or has been shutdown.", LogLevel.Warning);
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
        this.Log("Starting R3E game detection...");
        this.subscriptionSink.Add(this.r3eGameDetector.Start()
                                      .Subscribe(this.HandleGameDetection));
    }

    private void StopRunningDemos()
    {
        this.sharedMemoryDemo.Stop();
        this.IsRunningDemo = false;
    }

    private async Task WaitForGame()
    {
        this.isDemoCancelled = false;
        this.StartGameDetection();
        while(!this.isGameRunning)
        {
            this.Log("Waiting for R3E...");
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
