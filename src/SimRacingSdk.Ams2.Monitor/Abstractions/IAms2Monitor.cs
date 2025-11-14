using SimRacingSdk.Ams2.Monitor.Messages;
using SimRacingSdk.Ams2.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Monitor.Abstractions;

public interface IAms2Monitor : IDisposable
{
    void Start();
    void Stop();
    IObservable<LogMessage> LogMessages { get; }
    IObservable<Ams2MonitorSession> SessionCompleted { get; }
    IObservable<Ams2MonitorSession> SessionStarted { get; }
    IObservable<Ams2TelemetryFrame> Telemetry { get; }
    IObservable<Ams2Lap> CompletedLaps { get; }
    IObservable<Ams2MonitorParticipant> ParticipantUpdates { get; }
}