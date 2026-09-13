using SimRacingSdk.Core.Messages;
using SimRacingSdk.Pmr.Udp.Messages;

namespace SimRacingSdk.Pmr.Udp.Abstractions;

public interface IPmrUdpConnection : IDisposable
{
    string ConnectionIdentifier { get; }
    IObservable<LogMessage> LogMessages { get; }
    string MulticastGroup { get; }
    IObservable<PmrParticipantRaceState> ParticipantRaceStates { get; }
    int Port { get; }
    IObservable<PmrRaceInfo> RaceInfoUpdates { get; }
    IObservable<PmrSessionStopped> SessionStoppedUpdates { get; }
    bool UseMulticast { get; }
    IObservable<PmrVehicleTelemetry> VehicleTelemetryUpdates { get; }
    void Connect();
    void Stop();
}
