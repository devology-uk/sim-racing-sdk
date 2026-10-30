using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.R3e.Demo.Abstractions;
using SimRacingSdk.R3e.SharedMemory.Abstractions;
using SimRacingSdk.R3e.SharedMemory.Models;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.R3e.Demo.Demos;

public class SharedMemoryDemo : ISharedMemoryDemo
{
    private const int FrameLogInterval = 25;

    private readonly IConsoleLog consoleLog;
    private readonly ILogger<SharedMemoryDemo> logger;
    private readonly IR3eSharedMemoryConnectionFactory r3eSharedMemoryConnectionFactory;
    private readonly ISharedMemoryLog sharedMemoryLog;

    private IR3eSharedMemoryConnection? r3eSharedMemoryConnection;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public SharedMemoryDemo(ILogger<SharedMemoryDemo> logger,
        IConsoleLog consoleLog,
        ISharedMemoryLog sharedMemoryLog,
        IR3eSharedMemoryConnectionFactory r3eSharedMemoryConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.sharedMemoryLog = sharedMemoryLog;
        this.r3eSharedMemoryConnectionFactory = r3eSharedMemoryConnectionFactory;
    }

    public void Start()
    {
        this.Stop();

        this.telemetryFrameCount = 0;
        this.Log("Starting Shared Memory Demo...");
        this.r3eSharedMemoryConnection = this.r3eSharedMemoryConnectionFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.r3eSharedMemoryConnection.LogMessages.Subscribe(this.OnNextLogMessage),
            this.r3eSharedMemoryConnection.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.r3eSharedMemoryConnection.SessionEnded.Subscribe(this.OnNextSessionEnded),
            this.r3eSharedMemoryConnection.Laps.Subscribe(this.OnNextLap),
            this.r3eSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
        this.r3eSharedMemoryConnection.Start();
    }

    public void Stop()
    {
        if(this.r3eSharedMemoryConnection == null)
        {
            return;
        }

        this.Log("Stopping Shared Memory Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}");
        this.r3eSharedMemoryConnection?.Dispose();
        this.r3eSharedMemoryConnection = null;
        this.subscriptionSink?.Dispose();
    }

    public bool Validate()
    {
        this.Log("Validating Shared Memory Demo...");
        this.Log("R3E's shared memory is always on, so there is nothing to set up in the game.");
        return true;
    }

    private void Log(string message, LogLevel logLevel = LogLevel.Information)
    {
        this.logger.Log(logLevel, message);
        this.consoleLog.Write(message);
    }

    private void OnNextLap(R3eSharedMemoryLap lap)
    {
        if(lap.IsLocalPlayer)
        {
            this.consoleLog.Write($"Lap {lap.CompletedLaps}: {lap.LapTimeMs} ms, valid {lap.IsValid}");
        }
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.sharedMemoryLog.Log(logMessage);
    }

    private void OnNextSessionEnded(R3eSharedMemorySession session)
    {
        this.consoleLog.Write($"Session Ended: {session.SessionType} at {session.TrackName} {session.LayoutName}");
    }

    private void OnNextSessionStarted(R3eSharedMemorySession session)
    {
        this.consoleLog.Write(
            $"Session Started: {session.SessionType} {session.SessionIteration} at {session.TrackName} {session.LayoutName}");
    }

    private void OnNextTelemetryFrame(R3eTelemetryFrame telemetryFrame)
    {
        this.telemetryFrameCount++;
        if(this.telemetryFrameCount % FrameLogInterval == 0)
        {
            this.logger.LogInformation(LogValueFormatter.Format(telemetryFrame));
        }
    }
}
