using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.Monitor.Messages;
using SimRacingSdk.Ace.SharedMemory.Abstractions;
using SimRacingSdk.Ace.SharedMemory.Enums;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;

namespace SimRacingSdk.Ace.Monitor;

// Shared-memory-only, matching Ams2Monitor's pattern: Ace's client has no UDP broadcasting (see
// AceSharedMemoryConnection remarks), so session lifecycle comes purely from
// IAceSharedMemoryConnection's own SessionStarted/SessionEnded transitions - no Event wrapper,
// no UDP-only features (entries, accidents, penalties, green flag, phase/session-type changes)
// since none of those exist without a UDP feed. Those return if/when an opt-in remote
// Dedicated-Server UDP client is built.
public class AceMonitor : IAceMonitor
{
    private const string UnknownCarText = "Unknown";

    private readonly IAceCarInfoProvider aceCarInfoProvider;
    private readonly IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory;
    private readonly Subject<bool> isWhiteFlagActiveSubject = new();
    private readonly Subject<bool> isYellowFlagActiveSubject = new();
    private readonly Subject<AceMonitorLap> lapCompletedSubject = new();
    private readonly LogMessageBroker logMessageBroker = new(nameof(AceMonitor));
    private readonly Subject<AceMonitorSession> sessionCompletedSubject = new();
    private readonly Subject<AceMonitorSession> sessionStartedSubject = new();
    private readonly Subject<AceTelemetryFrame> telemetrySubject = new();

    private IAceSharedMemoryConnection? aceSharedMemoryConnection;
    private AceMonitorSession? currentSession;
    private bool isWhiteFlagActive;
    private bool isYellowFlagActive;
    private CompositeDisposable? subscriptionSink;

    public AceMonitor(IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory,
        IAceCarInfoProvider aceCarInfoProvider)
    {
        this.aceSharedMemoryConnectionFactory = aceSharedMemoryConnectionFactory;
        this.aceCarInfoProvider = aceCarInfoProvider;
    }

    public IObservable<bool> IsWhiteFlagActive => this.isWhiteFlagActiveSubject.AsObservable();
    public IObservable<bool> IsYellowFlagActive => this.isYellowFlagActiveSubject.AsObservable();
    public IObservable<AceMonitorLap> LapCompleted => this.lapCompletedSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    public IObservable<AceMonitorSession> SessionCompleted => this.sessionCompletedSubject.AsObservable();
    public IObservable<AceMonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<AceTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start()
    {
        this.LogMessage(LoggingLevel.Information, "Starting Ace Monitor...");

        this.aceSharedMemoryConnection = this.aceSharedMemoryConnectionFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.aceSharedMemoryConnection.FlagState.Subscribe(this.OnNextFlagState),
            this.aceSharedMemoryConnection.Laps.Subscribe(this.OnNextLap),
            this.aceSharedMemoryConnection.LogMessages.Subscribe(this.logMessageBroker.Relay),
            this.aceSharedMemoryConnection.SessionEnded.Subscribe(this.OnNextSessionEnded),
            this.aceSharedMemoryConnection.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.aceSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };

        this.aceSharedMemoryConnection.Start();
    }

    public void Stop()
    {
        this.subscriptionSink?.Dispose();
        this.aceSharedMemoryConnection?.Dispose();
        this.aceSharedMemoryConnection = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.Stop();
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessageBroker.Log(level, content);
    }

    private void OnNextFlagState(AceFlagState aceFlagState)
    {
        this.LogMessage(LoggingLevel.Information, aceFlagState.ToString());
        this.ProcessWhiteFlagState(aceFlagState);
        this.ProcessYellowFlagState(aceFlagState);
    }

    private void OnNextLap(AceSharedMemoryLap aceSharedMemoryLap)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var car = this.aceCarInfoProvider.FindByAceName(aceSharedMemoryLap.CarModel);
        var aceMonitorLap = new AceMonitorLap
        {
            CarManufacturer = car?.Manufacturer ?? UnknownCarText,
            CarModelName = aceSharedMemoryLap.CarModel,
            CompletedLaps = aceSharedMemoryLap.CompletedLaps,
            DriverName = aceSharedMemoryLap.DriverName,
            LastLapTimeMs = aceSharedMemoryLap.LastLapTimeMs,
            Sector1Ms = aceSharedMemoryLap.Sector1Ms,
            Sector2Ms = aceSharedMemoryLap.Sector2Ms,
            Sector3Ms = aceSharedMemoryLap.Sector3Ms,
            SessionId = this.currentSession.SessionId,
            TrackName = aceSharedMemoryLap.TrackId
        };

        this.LogMessage(LoggingLevel.Information, $"Lap Completed: {aceMonitorLap}");
        this.lapCompletedSubject.OnNext(aceMonitorLap);
    }

    private void OnNextSessionEnded(AceSharedMemorySession aceSharedMemorySession)
    {
        if(this.currentSession == null)
        {
            return;
        }

        this.currentSession.SessionType = aceSharedMemorySession.SessionType;
        this.currentSession.IsRunning = false;
        this.LogMessage(LoggingLevel.Information, $"Session Completed: {this.currentSession}");
        this.sessionCompletedSubject.OnNext(this.currentSession);
        this.currentSession = null;
    }

    private void OnNextSessionStarted(AceSharedMemorySession aceSharedMemorySession)
    {
        this.currentSession = new AceMonitorSession
        {
            Duration = TimeSpan.FromMilliseconds(aceSharedMemorySession.DurationMs),
            IsOnline = aceSharedMemorySession.IsOnline,
            IsRunning = true,
            NumberOfCars = (int)aceSharedMemorySession.NumberOfCars,
            SessionId = aceSharedMemorySession.SessionId,
            SessionType = aceSharedMemorySession.SessionType,
            TrackName = aceSharedMemorySession.TrackName
        };

        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
        this.sessionStartedSubject.OnNext(this.currentSession);
    }

    private void OnNextTelemetryFrame(AceTelemetryFrame aceTelemetryFrame)
    {
        this.telemetrySubject.OnNext(aceTelemetryFrame);
    }

    private void ProcessWhiteFlagState(AceFlagState aceFlagState)
    {
        var isActive = aceFlagState.Flag == AceFlagType.White || aceFlagState.GlobalFlag == AceFlagType.White;
        if(this.isWhiteFlagActive == isActive)
        {
            return;
        }

        this.isWhiteFlagActive = isActive;
        this.isWhiteFlagActiveSubject.OnNext(this.isWhiteFlagActive);
    }

    private void ProcessYellowFlagState(AceFlagState aceFlagState)
    {
        var isActive = aceFlagState.Flag == AceFlagType.Yellow || aceFlagState.GlobalFlag == AceFlagType.Yellow;
        if(this.isYellowFlagActive == isActive)
        {
            return;
        }

        this.isYellowFlagActive = isActive;
        this.isYellowFlagActiveSubject.OnNext(this.isYellowFlagActive);
    }
}
