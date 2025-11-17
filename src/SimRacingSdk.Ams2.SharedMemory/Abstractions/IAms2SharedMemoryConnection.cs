using SimRacingSdk.Ams2.SharedMemory.Messages;
using SimRacingSdk.Ams2.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.SharedMemory.Abstractions;

public interface IAms2SharedMemoryConnection : IDisposable
{
    IObservable<LogMessage> LogMessages { get; }
    IObservable<Ams2Lap> CompletedLaps { get; }
    IObservable<Ams2GameStatus> GameStatusUpdates { get; }
    IObservable<Ams2Participant> ParticipantUpdates { get; }
    IObservable<Ams2TelemetryFrame> Telemetry { get; }
    void Start(double updateIntervalMs = 300);
    void Stop();
}