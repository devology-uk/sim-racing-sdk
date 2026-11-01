using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.R3e.Demo.Abstractions;
using SimRacingSdk.R3e.Monitor.Abstractions;
using SimRacingSdk.R3e.Monitor.Messages;
using SimRacingSdk.R3e.SharedMemory.Models;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.R3e.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<MonitorDemo> logger;
    private readonly IMonitorLog monitorLog;
    private readonly IR3eMonitorFactory r3eMonitorFactory;

    private IR3eMonitor? r3eMonitor;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public MonitorDemo(ILogger<MonitorDemo> logger,
        IConsoleLog consoleLog,
        IMonitorLog monitorLog,
        IR3eMonitorFactory r3eMonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.monitorLog = monitorLog;
        this.r3eMonitorFactory = r3eMonitorFactory;
    }

    public void Start()
    {
        this.Stop();

        this.telemetryFrameCount = 0;
        this.Log("Starting R3E Monitor Demo...");
        this.r3eMonitor = this.r3eMonitorFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.r3eMonitor.LapCompleted.Subscribe(this.OnNextLapCompleted),
            this.r3eMonitor.LogMessages.Subscribe(this.OnNextLogMessage),
            this.r3eMonitor.SessionCompleted.Subscribe(this.OnNextSessionCompleted),
            this.r3eMonitor.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.r3eMonitor.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
        this.r3eMonitor.Start();
    }

    public void Stop()
    {
        if(this.r3eMonitor == null)
        {
            return;
        }

        this.Log("Stopping R3E Monitor Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}");
        this.r3eMonitor.Dispose();
        this.r3eMonitor = null;
        this.subscriptionSink?.Dispose();
    }

    public bool Validate()
    {
        return true;
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private void OnNextLapCompleted(R3eMonitorLap lap)
    {
        this.logger.LogInformation(lap.ToString());
        if(lap.IsLocalPlayer)
        {
            this.consoleLog.Write($"Lap {lap.CompletedLaps} in the {lap.CarModelName}: {lap.LapTimeMs} ms, valid {lap.IsValid}");
        }
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.monitorLog.Log(logMessage);
    }

    private void OnNextSessionCompleted(R3eMonitorSession session)
    {
        this.Log($"Session Completed: {session}");
    }

    private void OnNextSessionStarted(R3eMonitorSession session)
    {
        this.Log($"Session Started: {session}");
    }

    private void OnNextTelemetryFrame(R3eTelemetryFrame telemetryFrame)
    {
        this.telemetryFrameCount++;
    }
}
