using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Exceptions;
using SimRacingSdk.Lmu.SharedMemory.Messages;
using SimRacingSdk.Lmu.SharedMemory.Models;

namespace SimRacingSdk.Lmu.SharedMemory;

// Combines rFactor2SharedMemoryMapPlugin64's Telemetry+Scoring buffers with LMU_SharedMemoryMapPlugin64's Extended
// buffer into one LmuTelemetryFrame per poll - both plugins are required (Start throws if either isn't installed
// and configured), since the Extended plugin alone carries no telemetry/scoring at all (see the Lmu shared-memory
// scoping discussion this session) and the standard plugin alone is missing LMU's fuel/energy/penalty/TC fields.
public class LmuSharedMemoryConnection : ILmuSharedMemoryConnection
{
    private readonly LogMessageBroker logMessageBroker = new(nameof(LmuSharedMemoryConnection));
    private readonly ILmuSharedMemoryPluginInstaller lmuSharedMemoryPluginInstaller;
    private readonly IRfactor2SharedMemoryPluginInstaller rfactor2SharedMemoryPluginInstaller;
    private readonly ILmuSharedMemoryProvider sharedMemoryProvider;
    private readonly Subject<LmuTelemetryFrame> telemetrySubject = new();

    private IDisposable? updateSubscription;

    public LmuSharedMemoryConnection(ILmuSharedMemoryProvider sharedMemoryProvider,
        ILmuSharedMemoryPluginInstaller lmuSharedMemoryPluginInstaller,
        IRfactor2SharedMemoryPluginInstaller rfactor2SharedMemoryPluginInstaller)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
        this.lmuSharedMemoryPluginInstaller = lmuSharedMemoryPluginInstaller;
        this.rfactor2SharedMemoryPluginInstaller = rfactor2SharedMemoryPluginInstaller;
    }

    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    public IObservable<LmuTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Stop();
        GC.SuppressFinalize(this);
    }

    public void Start(double updateIntervalMs = 20)
    {
        if(!this.lmuSharedMemoryPluginInstaller.IsInstalled ||
           !this.rfactor2SharedMemoryPluginInstaller.IsInstalled)
        {
            throw new LmuSharedMemoryPluginsNotInstalledException();
        }

        this.Stop();
        this.LogMessage(LoggingLevel.Information, "Starting LMU Shared Memory connection...");
        this.updateSubscription = Observable.Interval(TimeSpan.FromMilliseconds(updateIntervalMs))
                                            .Subscribe(this.OnNextUpdate, this.OnError);
    }

    public void Stop()
    {
        this.updateSubscription?.Dispose();
        this.updateSubscription = null;
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessageBroker.Log(level, content);
    }

    private void OnError(Exception exception)
    {
        this.LogMessage(LoggingLevel.Error, exception.Message);
    }

    private void OnNextUpdate(long tick)
    {
        var scoringBuffer = this.sharedMemoryProvider.ReadScoring();
        var telemetryBuffer = this.sharedMemoryProvider.ReadTelemetry();
        var extendedBuffer = this.sharedMemoryProvider.ReadExtended();

        if(scoringBuffer is null || telemetryBuffer is null || extendedBuffer is null)
        {
            return;
        }

        var scoring = scoringBuffer.Value;
        var telemetry = telemetryBuffer.Value;

        var numScoredVehicles = Math.Clamp(scoring.ScoringInfo.NumVehicles, 0, Rf2TelemetryBuffer.MaxMappedVehicles);
        var playerScoringIndex =
            Array.FindIndex(scoring.Vehicles, 0, numScoredVehicles, vehicle => vehicle.IsPlayer);
        if(playerScoringIndex < 0)
        {
            return;
        }

        var playerScoring = scoring.Vehicles[playerScoringIndex];

        var numTelemetryVehicles = Math.Clamp(telemetry.NumVehicles, 0, Rf2TelemetryBuffer.MaxMappedVehicles);
        var playerTelemetryIndex = Array.FindIndex(telemetry.Vehicles,
            0,
            numTelemetryVehicles,
            vehicle => vehicle.Id == playerScoring.Id);
        if(playerTelemetryIndex < 0)
        {
            return;
        }

        var frame = new LmuTelemetryFrame(telemetry.Vehicles[playerTelemetryIndex],
            playerScoring,
            extendedBuffer.Value);
        this.telemetrySubject.OnNext(frame);
    }
}
