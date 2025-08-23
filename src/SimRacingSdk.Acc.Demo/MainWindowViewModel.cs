// ReSharper disable AsyncVoidMethod

using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Demo.Abstractions;

namespace SimRacingSdk.Acc.Demo;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IAccCompatibilityChecker accCompatibilityChecker;
    private readonly IAccGameDetector accGameDetector;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<MainWindowViewModel> logger;
    private readonly ISharedMemoryDemo sharedMemoryDemo;
    private readonly CompositeDisposable subscriptionSink = new();
    private readonly ITelemetryOnlyDemo telemetryOnlyDemo;
    private readonly IUdpDemo udpDemo;

    private bool isGameRunning;
    private bool isDemoCancelled = false;

    [ObservableProperty]
    private bool isRunningDemo = false;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger,
        IConsoleLog consoleLog,
        IAccCompatibilityChecker accCompatibilityChecker,
        IAccGameDetector accGameDetector,
        IUdpDemo udpDemo,
        ISharedMemoryDemo sharedMemoryDemo,
        ITelemetryOnlyDemo telemetryOnlyDemo)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accGameDetector = accGameDetector;
        this.udpDemo = udpDemo;
        this.sharedMemoryDemo = sharedMemoryDemo;
        this.telemetryOnlyDemo = telemetryOnlyDemo;
    }

    [RelayCommand]
    private void OpenLogFolder()
    {
        var currentLogPath =
            $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\{DateTime.Now.Date:yyyy-MM-dd}\";
        if(Directory.Exists(currentLogPath))
        {
            Process.Start("explorer.exe", currentLogPath);
        }
    }

    [RelayCommand]
    private async void StartSharedMemoryDemo()
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
    private async Task StartTelemetryOnlySharedDemo()
    {
        this.consoleLog.Clear();
        this.StopRunningDemos();
        this.IsRunningDemo = true;
        this.CheckCompatibility();

        if(!this.telemetryOnlyDemo.Validate())
        {
            return;
        }

        await this.WaitFormGame();
        if(!this.isGameRunning)
        {
            return;
        }
        this.telemetryOnlyDemo.Start();
    }

    [RelayCommand]
    private async void StartUdpDemo()
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
        if(!this.accCompatibilityChecker.IsAccInstalled())
        {
            this.Log("ACC is not installed. Please install ACC and run this demo again.", LogLevel.Warning);
            return false;
        }

        this.Log("Checking ACC Account is available...");
        if(!this.accCompatibilityChecker.IsAccountAvailable())
        {
            this.Log(
                "No account profile found. Please run ACC and complete the initial configuration then run this demo again.",
                LogLevel.Warning);
            return false;
        }

        this.Log("ACC compatibility check was completed successfully.");
        return true;
    }

    private void HandleGameDetection(bool isDetected)
    {
        if(isDetected)
        {
            this.Log("ACC game detected successfully.");
            this.isGameRunning = true;
        }
        else
        {
            this.Log("ACC game is not running or has been shutdown.", LogLevel.Warning);
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
        this.Log("Starting ACC game detection...");
        this.subscriptionSink.Add(this.accGameDetector.Start()
                                      .Subscribe(this.HandleGameDetection));
    }

    private void StopRunningDemos()
    {
        this.sharedMemoryDemo.Stop();
        this.telemetryOnlyDemo.Stop();
        this.udpDemo.Stop();
        this.IsRunningDemo = false;
    }

    private async Task WaitFormGame()
    {
        this.isDemoCancelled = false;
        this.StartGameDetection();
        while(!this.isGameRunning)
        {
            this.Log("Waiting for ACC...");
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