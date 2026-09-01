using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Monitor.Abstractions;
using SimRacingSdk.Lmu.Monitor.Messages;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Models;

namespace SimRacingSdk.Lmu.Monitor;

// Shared-memory-only, mirroring AceMonitor's pattern: session/lap lifecycle comes purely from
// ILmuSharedMemoryConnection's own SessionStarted/SessionEnded/Laps transitions, which it derives from diffing
// LmuScoringInfo/LmuVehicleScoring reads itself (see LmuSharedMemoryConnection.UpdateSession/UpdateLap) - Monitor
// only enriches with car info and republishes as its own Monitor-domain types.
public class LmuMonitor : ILmuMonitor
{
    private const string UnknownCarText = "Unknown";

    private readonly ILmuCarInfoProvider lmuCarInfoProvider;
    private readonly ILmuSharedMemoryConnectionFactory lmuSharedMemoryConnectionFactory;
    private readonly Subject<LmuMonitorLap> lapCompletedSubject = new();
    private readonly LogMessageBroker logMessageBroker = new(nameof(LmuMonitor));
    private readonly Subject<LmuMonitorSession> sessionCompletedSubject = new();
    private readonly Subject<LmuMonitorSession> sessionStartedSubject = new();
    private readonly Subject<LmuTelemetryFrame> telemetrySubject = new();

    private LmuMonitorSession? currentSession;
    private ILmuSharedMemoryConnection? lmuSharedMemoryConnection;
    private CompositeDisposable? subscriptionSink;

    public LmuMonitor(ILmuSharedMemoryConnectionFactory lmuSharedMemoryConnectionFactory,
        ILmuCarInfoProvider lmuCarInfoProvider)
    {
        this.lmuSharedMemoryConnectionFactory = lmuSharedMemoryConnectionFactory;
        this.lmuCarInfoProvider = lmuCarInfoProvider;
    }

    public IObservable<LmuMonitorLap> LapCompleted => this.lapCompletedSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    public IObservable<LmuMonitorSession> SessionCompleted => this.sessionCompletedSubject.AsObservable();
    public IObservable<LmuMonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<LmuTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start()
    {
        this.LogMessage(LoggingLevel.Information, "Starting LMU Monitor...");

        this.lmuSharedMemoryConnection = this.lmuSharedMemoryConnectionFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.lmuSharedMemoryConnection.Laps.Subscribe(this.OnNextLap),
            this.lmuSharedMemoryConnection.LogMessages.Subscribe(this.logMessageBroker.Relay),
            this.lmuSharedMemoryConnection.SessionEnded.Subscribe(this.OnNextSessionEnded),
            this.lmuSharedMemoryConnection.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.lmuSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };

        this.lmuSharedMemoryConnection.Start();
    }

    public void Stop()
    {
        this.subscriptionSink?.Dispose();
        this.lmuSharedMemoryConnection?.Dispose();
        this.lmuSharedMemoryConnection = null;
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

    private void OnNextLap(LmuSharedMemoryLap lmuSharedMemoryLap)
    {
        if(this.currentSession == null)
        {
            return;
        }

        // VehicleModel (e.g. "Porsche 911 GT3 R LMGT3") is the catalog-matchable car identity; VehicleName is the
        // driver's entry/livery string (team name + car number) - confirmed against a real rig log, see CLAUDE.md.
        var car = this.lmuCarInfoProvider.GetCarInfoByDisplayName(lmuSharedMemoryLap.VehicleModel);
        var lmuMonitorLap = new LmuMonitorLap
        {
            CarManufacturer = car?.Manufacturer ?? UnknownCarText,
            CompletedLaps = lmuSharedMemoryLap.CompletedLaps,
            DriverName = lmuSharedMemoryLap.DriverName,
            LastLapTimeMs = lmuSharedMemoryLap.LastLapTimeMs,
            Sector1Ms = lmuSharedMemoryLap.Sector1Ms,
            Sector2Ms = lmuSharedMemoryLap.Sector2Ms,
            Sector3Ms = lmuSharedMemoryLap.Sector3Ms,
            SessionId = this.currentSession.SessionId,
            TrackName = lmuSharedMemoryLap.TrackName,
            VehicleClass = lmuSharedMemoryLap.VehicleClass,
            VehicleClassName = lmuSharedMemoryLap.VehicleClassName,
            VehicleName = lmuSharedMemoryLap.VehicleName
        };

        this.LogMessage(LoggingLevel.Information, $"Lap Completed: {lmuMonitorLap}");
        this.lapCompletedSubject.OnNext(lmuMonitorLap);
    }

    private void OnNextSessionEnded(LmuSharedMemorySession lmuSharedMemorySession)
    {
        if(this.currentSession == null)
        {
            return;
        }

        this.currentSession.IsRunning = false;
        this.LogMessage(LoggingLevel.Information, $"Session Completed: {this.currentSession}");
        this.sessionCompletedSubject.OnNext(this.currentSession);
        this.currentSession = null;
    }

    private void OnNextSessionStarted(LmuSharedMemorySession lmuSharedMemorySession)
    {
        this.currentSession = new LmuMonitorSession
        {
            EndEt = lmuSharedMemorySession.EndEt,
            IsRunning = true,
            MaxLaps = lmuSharedMemorySession.MaxLaps,
            NumberOfCars = lmuSharedMemorySession.NumberOfCars,
            SessionId = lmuSharedMemorySession.SessionId,
            SessionType = lmuSharedMemorySession.SessionType,
            TrackName = lmuSharedMemorySession.TrackName
        };

        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
        this.sessionStartedSubject.OnNext(this.currentSession);
    }

    private void OnNextTelemetryFrame(LmuTelemetryFrame lmuTelemetryFrame)
    {
        this.telemetrySubject.OnNext(lmuTelemetryFrame);
    }
}
