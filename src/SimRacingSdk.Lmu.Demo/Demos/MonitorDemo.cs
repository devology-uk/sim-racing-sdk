/*
 * This demo provides an example of how to use the LmuMonitor component, which wraps
 * LmuSharedMemoryConnection to provide enriched, application-friendly data objects.
 *
 * The demo simply logs entities but in a real application you might want to save these entities
 * in a database and display the data.
 *
 * Like SharedMemoryDemo, this only ever reports the local player's own session/laps/telemetry -
 * LMU's shared memory has no entry list or other-car data.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.Demo.Abstractions;
using SimRacingSdk.Lmu.Monitor.Abstractions;
using SimRacingSdk.Lmu.Monitor.Messages;
using SimRacingSdk.Lmu.SharedMemory.Models;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.Lmu.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IConsoleLog consoleLog;
    private readonly ILmuMonitorFactory lmuMonitorFactory;
    private readonly ILogger<MonitorDemo> logger;
    private readonly IMonitorLog monitorLog;
    private ILmuMonitor? lmuMonitor;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public MonitorDemo(ILogger<MonitorDemo> logger,
        IConsoleLog consoleLog,
        IMonitorLog monitorLog,
        ILmuMonitorFactory lmuMonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.monitorLog = monitorLog;
        this.lmuMonitorFactory = lmuMonitorFactory;
    }

    public void Start()
    {
        this.Stop();
        this.telemetryFrameCount = 0;
        this.Log("Starting LMU Monitor Demo...");

        this.lmuMonitor = this.lmuMonitorFactory.Create();
        this.PrepareMessageHandling();
        this.lmuMonitor.Start();
    }

    public void Stop()
    {
        if(this.lmuMonitor == null)
        {
            return;
        }

        this.Log("Stopping LMU Monitor Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}");
        this.subscriptionSink?.Dispose();
        this.lmuMonitor?.Dispose();
        this.lmuMonitor = null;
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

    private void OnNextCompletedLap(LmuMonitorLap lmuMonitorLap)
    {
        this.Log(lmuMonitorLap.ToString());
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.monitorLog.Log(logMessage);
    }

    private void OnNextSessionCompleted(LmuMonitorSession lmuMonitorSession)
    {
        this.Log($"Session Completed: {lmuMonitorSession}");
    }

    private void OnNextSessionStarted(LmuMonitorSession lmuMonitorSession)
    {
        this.Log($"Session Started: {lmuMonitorSession}");
    }

    private void OnNextTelemetryFrame(LmuTelemetryFrame lmuTelemetryFrame)
    {
        // too much information to log telemetry frames, which are logged via log messages
        // just maintaining a count to report at the end
        this.telemetryFrameCount++;
    }

    private void PrepareMessageHandling()
    {
        if(this.lmuMonitor == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable
        {
            this.lmuMonitor.LapCompleted.Subscribe(this.OnNextCompletedLap),
            this.lmuMonitor.LogMessages.Subscribe(this.OnNextLogMessage),
            this.lmuMonitor.SessionCompleted.Subscribe(this.OnNextSessionCompleted),
            this.lmuMonitor.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.lmuMonitor.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
    }
}
