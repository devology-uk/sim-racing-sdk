using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Messages;
using SimRacingSdk.Lmu.SharedMemory.Models;

namespace SimRacingSdk.Lmu.SharedMemory;

// Reads LMU's native "LMU_Data" shared memory (Studio 397's own SharedMemoryInterface.hpp) directly - no plugin,
// no CustomPluginVariables.JSON, no DMA. The player's Telemetry entry comes straight from PlayerVehicleIdx; the
// matching Scoring entry is found via IsPlayer - both come from the same read, so there's no cross-source
// lap/session-key linking problem to guard against.
public class LmuSharedMemoryConnection : ILmuSharedMemoryConnection
{
    private readonly LogMessageBroker logMessageBroker = new(nameof(LmuSharedMemoryConnection));
    private readonly ILmuSharedMemoryProvider sharedMemoryProvider;
    private readonly Subject<LmuTelemetryFrame> telemetrySubject = new();

    private string? lastLoggedWaitReason;
    private CompositeDisposable? subscriptionSink;
    private IDisposable? updateSubscription;

    public LmuSharedMemoryConnection(ILmuSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
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
        this.Stop();
        this.LogMessage(LoggingLevel.Information, "Starting LMU Shared Memory connection...");
        this.subscriptionSink = new CompositeDisposable
        {
            this.sharedMemoryProvider.LogMessages.Subscribe(this.logMessageBroker.Relay)
        };
        this.updateSubscription = Observable.Interval(TimeSpan.FromMilliseconds(updateIntervalMs))
                                            .Subscribe(this.OnNextUpdate, this.OnError);
    }

    public void Stop()
    {
        this.updateSubscription?.Dispose();
        this.updateSubscription = null;
        this.subscriptionSink?.Dispose();
        this.subscriptionSink = null;
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessageBroker.Log(level, content);
    }

    private void LogWaitReasonOnce(string reason)
    {
        if(this.lastLoggedWaitReason == reason)
        {
            return;
        }

        this.lastLoggedWaitReason = reason;
        this.LogMessage(LoggingLevel.Information, reason);
    }

    private void OnError(Exception exception)
    {
        this.LogMessage(LoggingLevel.Error, exception.Message);
    }

    private void OnNextUpdate(long tick)
    {
        var data = this.sharedMemoryProvider.Read();
        if(data is null)
        {
            return;
        }

        var telemetry = data.Value.Telemetry;
        if(!telemetry.PlayerHasVehicle || telemetry.PlayerVehicleIdx >= telemetry.TelemInfo.Length)
        {
            this.LogWaitReasonOnce(
                "Connected to LMU shared memory, waiting for a player vehicle (join a session/get in a car)...");
            return;
        }

        var playerTelemetry = telemetry.TelemInfo[telemetry.PlayerVehicleIdx];

        var scoring = data.Value.Scoring;
        var numScoredVehicles =
            Math.Clamp(scoring.ScoringInfo.NumVehicles, 0, LmuSharedMemoryScoringData.MaxVehicles);
        var playerScoringIndex =
            Array.FindIndex(scoring.VehScoringInfo, 0, numScoredVehicles, vehicle => vehicle.IsPlayer);
        if(playerScoringIndex < 0)
        {
            this.LogWaitReasonOnce(
                "Connected to LMU shared memory, have player telemetry but no matching scoring entry yet...");
            return;
        }

        this.lastLoggedWaitReason = null;
        var frame = new LmuTelemetryFrame(playerTelemetry, scoring.VehScoringInfo[playerScoringIndex]);
        this.telemetrySubject.OnNext(frame);
    }
}
