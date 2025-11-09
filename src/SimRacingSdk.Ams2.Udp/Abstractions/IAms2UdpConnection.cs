using SimRacingSdk.Ams2.Udp.Messages;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Udp.Abstractions;

public interface IAms2UdpConnection : IDisposable
{
    string ConnectionIdentifier { get; }
    IObservable<ConnectionState> ConnectionStateUpdates { get; }
    IObservable<GameStateUpdate> GameStateUpdates { get; }
    string IpAddress { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<ParticipantsUpdate> ParticipantUpdates { get; }
    int Port { get; }
    IObservable<RaceInfoUpdate> RaceInfoUpdates { get; }
    IObservable<TelemetryUpdate> TelemetryUpdates { get; }
    IObservable<TimeStatsUpdate> TimeStatsUpdates { get; }
    IObservable<TimingsUpdate> TimingsUpdates { get; }
    IObservable<VehicleClassUpdate> VehicleClassUpdates { get; }
    IObservable<VehicleInfoUpdate> VehicleInfoUpdates { get; }
    void Connect(bool autoDetect = true);
    void Stop();
}