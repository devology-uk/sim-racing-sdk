using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Demo.Abstractions;
using SimRacingSdk.Lmu.Demo.CarExplorer;
using SimRacingSdk.Lmu.Demo.TrackExplorer;
using SimRacingSdk.LogViewer;

namespace SimRacingSdk.Lmu.Demo;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IConsoleLog consoleLog;
    private readonly ILmuGameDetector lmuGameDetector;
    private readonly string logFolderPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\";
    private readonly ILogger<MainWindowViewModel> logger;
    private readonly CompositeDisposable subscriptionSink = new();

    private bool isDemoCancelled = false;
    private bool isGameRunning;

    [ObservableProperty]
    private bool isRunningDemo = false;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger, IConsoleLog consoleLog, ILmuGameDetector lmuGameDetector)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.lmuGameDetector = lmuGameDetector;
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
    private void StopDemo()
    {
        this.isDemoCancelled = true;
        this.StopRunningDemos();
    }

    private void HandleGameDetection(bool isDetected)
    {
        if(isDetected)
        {
            this.Log("LMU game detected successfully.");
            this.isGameRunning = true;
        }
        else
        {
            this.Log("LMU game is not running or has been shutdown.", LogLevel.Warning);
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
        this.Log("Starting LMU game detection...");
        this.subscriptionSink.Add(this.lmuGameDetector.Start()
                                      .Subscribe(this.HandleGameDetection));
    }

    private void StopRunningDemos()
    {
        this.IsRunningDemo = false;
    }

    private async Task WaitForGame()
    {
        this.isDemoCancelled = false;
        this.StartGameDetection();
        while(!this.isGameRunning)
        {
            this.Log("Waiting for LMU...");
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