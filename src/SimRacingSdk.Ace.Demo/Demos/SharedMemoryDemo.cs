/*
 * This demo shows how to use the AceSharedMemoryConnection to read and process data
 * using the Shared Memory interface provided by ACE.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Ace.Demo.Abstractions;
using SimRacingSdk.Ace.SharedMemory.Abstractions;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.Demo.Demos;

public class SharedMemoryDemo : ISharedMemoryDemo
{
    private readonly IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<SharedMemoryDemo> logger;
    private readonly ISharedMemoryLog sharedMemoryLog;
    private IAceSharedMemoryConnection? aceSharedMemoryConnection;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public SharedMemoryDemo(ILogger<SharedMemoryDemo> logger,
        IConsoleLog consoleLog,
        ISharedMemoryLog sharedMemoryLog,
        IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.sharedMemoryLog = sharedMemoryLog;
        this.aceSharedMemoryConnectionFactory = aceSharedMemoryConnectionFactory;
    }

    public void Start()
    {
        this.Stop();

        this.Log("Starting Shared Memory Demo...");
        this.aceSharedMemoryConnection = this.aceSharedMemoryConnectionFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.aceSharedMemoryConnection.LogMessages.Subscribe(this.OnNextLogMessage),
            this.aceSharedMemoryConnection.AppStatusChanges.Subscribe(this.OnNextAppStateChange),
            this.aceSharedMemoryConnection.FlagState.Subscribe(this.OnNextFlagState),
            this.aceSharedMemoryConnection.Laps.Subscribe(this.OnNextNewLap),
            this.aceSharedMemoryConnection.SessionEnded.Subscribe(this.OnNextSessionEnded),
            this.aceSharedMemoryConnection.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.aceSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
        this.aceSharedMemoryConnection.Start();
    }

    public void Stop()
    {
        if(this.aceSharedMemoryConnection == null)
        {
            return;
        }

        this.Log("Stopping Shared Memory Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}");
        this.subscriptionSink?.Dispose();
        this.aceSharedMemoryConnection?.Dispose();
        this.aceSharedMemoryConnection = null!;
    }

    public bool Validate()
    {
        this.Log("Validating Shared Memory Demo...");
        return true;
    }

    private void Log(string message, LogLevel logLevel = LogLevel.Information)
    {
        this.logger.Log(logLevel, message);
        this.consoleLog.Write(message);
    }

    private void OnNextAppStateChange(AceAppStatusChange aceAppStatusChange)
    {
        this.Log(aceAppStatusChange.ToString());
    }

    private void OnNextFlagState(AceFlagState aceFlagState)
    {
        this.Log(aceFlagState.ToString());
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.sharedMemoryLog.Log(logMessage.ToString());
    }

    private void OnNextNewLap(AceSharedMemoryLap aceSharedMemoryLap)
    {
        this.Log(aceSharedMemoryLap.ToString());
    }

    private void OnNextSessionEnded(AceSharedMemorySession sharedMemorySession)
    {
        this.Log(sharedMemorySession.ToString());
    }

    private void OnNextSessionStarted(AceSharedMemorySession sharedMemorySession)
    {
        this.Log(sharedMemorySession.ToString());
    }

    private void OnNextTelemetryFrame(AceTelemetryFrame aceTelemetryFrame)
    {
        // too much information to log telemetry frames, which are logged via log messages
        // just maintaining a count to report at the end
        this.telemetryFrameCount++;
    }
}
