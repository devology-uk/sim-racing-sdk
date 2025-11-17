using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Demo.Abstractions;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;
using SimRacingSdk.Ams2.SharedMemory.Messages;
using SimRacingSdk.Ams2.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Demo.Demos;

public class SharedMemoryDemo : ISharedMemoryDemo
{
    private readonly IAms2CompatibilityChecker ams2CompatibilityChecker;
    private readonly IAms2SharedMemoryConnectionFactory ams2SharedMemoryConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<ISharedMemoryDemo> logger;
    private readonly ISharedMemoryLog sharedMemoryLog;

    private IAms2SharedMemoryConnection? ams2SharedMemoryConnection;
    private CompositeDisposable subscriptionSink = null!;

    public SharedMemoryDemo(ILogger<SharedMemoryDemo> logger,
        IConsoleLog consoleLog,
        ISharedMemoryLog sharedMemoryLog,
        IAms2CompatibilityChecker ams2CompatibilityChecker,
        IAms2SharedMemoryConnectionFactory ams2SharedMemoryConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.sharedMemoryLog = sharedMemoryLog;
        this.ams2CompatibilityChecker = ams2CompatibilityChecker;
        this.ams2SharedMemoryConnectionFactory = ams2SharedMemoryConnectionFactory;
    } 

    public void Start()
    {
        this.Stop();
        this.Log("Starting Shared Memory Demo...");

        this.ams2SharedMemoryConnection = this.ams2SharedMemoryConnectionFactory.Create();
        this.PrepareMessageHandling();
        this.ams2SharedMemoryConnection.Start();
    }

    public void Stop()
    {
        if(this.ams2SharedMemoryConnection == null)
        {
            return;
        }

        this.Log("Stopping Shared Memory Demo...");
        this.subscriptionSink?.Dispose();
        this.ams2SharedMemoryConnection?.Dispose();
        this.ams2SharedMemoryConnection = null;
    }

    public bool Validate()
    {
        return this.ams2CompatibilityChecker.IsAms2Installed();
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private void OnNextCompletedLap(Ams2Lap ams2Lap)
    {
        this.Log(ams2Lap.ToString());
    }

    private void OnNextGameStateChange(Ams2GameStateChange ams2GameStateChange)
    {
        this.Log(ams2GameStateChange.ToString());
    }

    private void OnNextGameStatusUpdate(Ams2GameStatus ams2GameStatus)
    {
        this.Log(ams2GameStatus.ToString());
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.sharedMemoryLog.Log(logMessage.ToString());
        this.consoleLog.Write(logMessage.ToString());
    }

    private void OnNextParticipantUpdate(Ams2Participant ams2Participant)
    {
        this.Log(ams2Participant.ToString());
    }

    private void OnNextRaceStateChange(Ams2RaceStateChange ams2RaceStateChange)
    {
        this.Log(ams2RaceStateChange.ToString());
    }

    private void OnNextSessionStateChange(Ams2SessionStateChange ams2SessionStateChange)
    { }

    private void OnNextTelemetryFrame(Ams2TelemetryFrame ams2TelemetryFrame)
    {
        this.Log(ams2TelemetryFrame.ToString());
    }

    private void PrepareMessageHandling()
    {
        if(this.ams2SharedMemoryConnection == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable
        {
            this.ams2SharedMemoryConnection.LogMessages.Subscribe(this.OnNextLogMessage),
            this.ams2SharedMemoryConnection.CompletedLaps.Subscribe(this.OnNextCompletedLap),
            this.ams2SharedMemoryConnection.GameStateChanges.Subscribe(this.OnNextGameStateChange),
            this.ams2SharedMemoryConnection.GameStatusUpdates.Subscribe(this.OnNextGameStatusUpdate),
            this.ams2SharedMemoryConnection.ParticipantUpdates.Subscribe(this.OnNextParticipantUpdate),
            this.ams2SharedMemoryConnection.RaceStateChanges.Subscribe(this.OnNextRaceStateChange),
            this.ams2SharedMemoryConnection.SessionStateChanges.Subscribe(this.OnNextSessionStateChange),
            this.ams2SharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
    }
}