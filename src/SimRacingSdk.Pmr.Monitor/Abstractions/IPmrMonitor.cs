using SimRacingSdk.Core.Messages;
using SimRacingSdk.Pmr.Monitor.Messages;
using SimRacingSdk.Pmr.Udp;
using SimRacingSdk.Pmr.Udp.Messages;

namespace SimRacingSdk.Pmr.Monitor.Abstractions;

public interface IPmrMonitor : IDisposable
{
    IObservable<PmrMonitorLap> LapCompleted { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<PmrParticipantRaceState> ParticipantUpdates { get; }
    IObservable<PmrMonitorSession> SessionCompleted { get; }
    IObservable<PmrMonitorSession> SessionStarted { get; }
    IObservable<PmrVehicleTelemetry> Telemetry { get; }
    void Start(int port = PmrUdpConnection.DefaultPort,
        bool useMulticast = false,
        string multicastGroup = PmrUdpConnection.DefaultMulticastGroup);
    void Stop();
}
