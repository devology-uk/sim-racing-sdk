/*
 * This demo provides an example of how to use the AceMonitor component, which wraps
 * AceSharedMemoryConnection to provide enriched, application-friendly data objects.
 *
 * The demo simply logs entities but in a real application you might want to save these entities
 * in a database and display the data.
 *
 * Ace Evo's running game client has no UDP broadcasting feed (only a separate Dedicated Server
 * package exposes one), so AceMonitor is shared-memory-only and only ever reports the local
 * player's own session/laps/telemetry - there is no entry list or other-car data here.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Ace.Demo.Abstractions;
using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.Monitor.Messages;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IAceMonitorFactory aceMonitorFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<MonitorDemo> logger;
    private readonly IMonitorLog monitorLog;
    private IAceMonitor? aceMonitor;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public MonitorDemo(ILogger<MonitorDemo> logger,
        IConsoleLog consoleLog,
        IMonitorLog monitorLog,
        IAceMonitorFactory aceMonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.monitorLog = monitorLog;
        this.aceMonitorFactory = aceMonitorFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting ACE Monitor Demo...");

        this.aceMonitor = this.aceMonitorFactory.Create();
        this.PrepareMessageHandling();
        this.aceMonitor.Start();
    }

    public void Stop()
    {
        if(this.aceMonitor == null)
        {
            return;
        }

        this.Log("Stopping ACE Monitor Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}");
        this.subscriptionSink?.Dispose();
        this.aceMonitor?.Dispose();
        this.aceMonitor = null;
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

    private void OnNextCompletedLap(AceMonitorLap aceMonitorLap)
    {
        this.Log(aceMonitorLap.ToString());
    }

    private void OnNextIsWhiteFlagActive(bool isWhiteFlagActive)
    {
        this.Log($"White Flag Is Active: {isWhiteFlagActive}");
    }

    private void OnNextIsYellowFlagActive(bool isYellowFlagActive)
    {
        this.Log($"Yellow Flag Is Active: {isYellowFlagActive}");
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.monitorLog.Log(logMessage.ToString());
    }

    private void OnNextSessionCompleted(AceMonitorSession aceMonitorSession)
    {
        this.Log($"Session Completed: {aceMonitorSession}");
    }

    private void OnNextSessionStarted(AceMonitorSession aceMonitorSession)
    {
        this.Log($"Session Started: {aceMonitorSession}");
    }

    private void OnNextTelemetryFrame(AceTelemetryFrame aceTelemetryFrame)
    {
        // too much information to log telemetry frames, which are logged via log messages
        // just maintaining a count to report at the end
        this.telemetryFrameCount++;
    }

    private void PrepareMessageHandling()
    {
        if(this.aceMonitor == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable
        {
            this.aceMonitor.IsWhiteFlagActive.Subscribe(this.OnNextIsWhiteFlagActive),
            this.aceMonitor.IsYellowFlagActive.Subscribe(this.OnNextIsYellowFlagActive),
            this.aceMonitor.LapCompleted.Subscribe(this.OnNextCompletedLap),
            this.aceMonitor.LogMessages.Subscribe(this.OnNextLogMessage),
            this.aceMonitor.SessionCompleted.Subscribe(this.OnNextSessionCompleted),
            this.aceMonitor.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.aceMonitor.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
    }
}
