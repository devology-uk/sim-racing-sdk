using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.Udp.Abstractions;
using SimRacingSdk.Acc.Udp.Messages;

// ReSharper disable AsyncVoidMethod

namespace SimRacingSdk.Acc.Demo;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IAccCompatibilityChecker accCompatibilityChecker;
    private readonly IAccGameDetector accGameDetector;
    private readonly IAccLocalConfigProvider accLocalConfigProvider;
    private readonly IAccPathProvider accPathProvider;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<MainWindowViewModel> logger;
    private readonly CompositeDisposable subscriptionSink = new();
    private IAccUdpConnection accUdpConnection = null!;
    private bool isGameRunning;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger,
        IConsoleLog consoleLog,
        IAccCompatibilityChecker accCompatibilityChecker,
        IAccLocalConfigProvider accLocalConfigProvider,
        IAccPathProvider accPathProvider,
        IAccGameDetector accGameDetector,
        IAccUdpConnectionFactory accUdpConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accLocalConfigProvider = accLocalConfigProvider;
        this.accPathProvider = accPathProvider;
        this.accGameDetector = accGameDetector;
        this.accUdpConnectionFactory = accUdpConnectionFactory;
    }

    [RelayCommand]
    private void OpenFileLog()
    {
        var currentLogPath =
            $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\logs\{DateTime.Now.Date:yyyy-MM-dd}\";
        if(Directory.Exists(currentLogPath))
        {
            Process.Start("explorer.exe", currentLogPath);
        }
    }

    [RelayCommand]
    private async void StartBroadcastingDemo()
    {
        if(!this.accCompatibilityChecker.HasValidBroadcastingSettings())
        {
            this.Log(
                $"ACC broadcasting has not been configured.  Please ensure at least the updListenerPort in {this.accPathProvider.BroadcastingSettingsFilePath} has been set to a value > 1023 and try again.");
            return;
        }

        await this.WaitFormGame();

        var broadcastSettings = this.accLocalConfigProvider.GetBroadcastingSettings()!;
        this.accUdpConnection = this.accUdpConnectionFactory.Create("127.0.0.1",
            broadcastSettings.UdpListenerPort,
            "Sim Racing SDK ACC UDP Demo",
            broadcastSettings.ConnectionPassword,
            broadcastSettings.CommandPassword);

        this.PrepareBroadcastMessageHandling();
        this.accUdpConnection.Connect();
    }

    [RelayCommand]
    private async void StartSharedMemoryDemo()
    {
        await this.WaitFormGame();
    }

    [RelayCommand]
    private async Task StartTelemetryOnlySharedDemo()
    {
        await this.WaitFormGame();
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

    private void LogBroadcastingEvent(BroadcastingEvent broadcastingEvent)
    {
        this.Log(broadcastingEvent.ToString());
    }

    private void OnNexLogMessage(string message)
    {
        this.Log(message);
    }

    private void OnNextConnectionStateChange(ConnectionState connectionState)
    {
        this.Log(connectionState.ToString());
    }

    private void OnNextEntryListUpdate(EntryListUpdate entryListUpdate)
    {
        this.Log(entryListUpdate.ToString());
    }

    private void OnNextRealtimeCarUpdate(RealtimeCarUpdate realtimeCarUpdate)
    {
        this.Log(realtimeCarUpdate.ToString());
    }

    private void OnNextRealtimeUpdate(RealtimeUpdate realtimeUpdate)
    {
        this.Log(realtimeUpdate.ToString());
    }

    private void OnNextTrackDataUpdate(TrackDataUpdate trackDataUpdate)
    {
        this.Log(trackDataUpdate.ToString());
    }

    private void PrepareBroadcastMessageHandling()
    {
        this.subscriptionSink.Add(this.accUdpConnection.BestPersonalLap.Subscribe(this.LogBroadcastingEvent));
        this.subscriptionSink.Add(this.accUdpConnection.BestSessionLap.Subscribe(this.LogBroadcastingEvent));
        this.subscriptionSink.Add(
            this.accUdpConnection.ConnectionStateChanges.Subscribe(this.OnNextConnectionStateChange));
        this.subscriptionSink.Add(
            this.accUdpConnection.EntryListUpdates.Subscribe(this.OnNextEntryListUpdate));
        this.subscriptionSink.Add(this.accUdpConnection.GreenFlag.Subscribe(this.LogBroadcastingEvent));
        this.subscriptionSink.Add(this.accUdpConnection.LapCompleted.Subscribe(this.LogBroadcastingEvent));
        this.subscriptionSink.Add(this.accUdpConnection.PenaltyMessage.Subscribe(this.LogBroadcastingEvent));
        this.subscriptionSink.Add(
            this.accUdpConnection.RealTimeCarUpdates.Subscribe(this.OnNextRealtimeCarUpdate));
        this.subscriptionSink.Add(this.accUdpConnection.RealTimeUpdates.Subscribe(this.OnNextRealtimeUpdate));
        this.subscriptionSink.Add(
            this.accUdpConnection.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate));
        this.subscriptionSink.Add(this.accUdpConnection.LogMessages.Subscribe(this.OnNexLogMessage));
    }

    private void StartGameDetection()
    {
        this.Log("Starting ACC game detection...");
        this.subscriptionSink.Add(this.accGameDetector.Start()
                                      .Subscribe(this.HandleGameDetection));
    }

    private async Task WaitFormGame()
    {
        if(!this.CheckCompatibility())
        {
            return;
        }

        this.StartGameDetection();
        while(!this.isGameRunning)
        {
            this.Log("Waiting for ACC...");
            await Task.Delay(5000);
        }
    }

    ~MainWindowViewModel()
    {
        this.subscriptionSink.Dispose();
    }
}