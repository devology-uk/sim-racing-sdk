using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Udp.Abstractions;

public interface IAccUdpConnection
{
    IObservable<BroadcastingEvent> Accident { get; }
    IObservable<BroadcastingEvent> BestPersonalLap { get; }
    IObservable<BroadcastingEvent> BestSessionLap { get; }
    string CommandPassword { get; }
    string ConnectionIdentifier { get; }
    string ConnectionPassword { get; }
    IObservable<ConnectionState> ConnectionStateChanges { get; }
    string DisplayName { get; }
    IObservable<EntryListUpdate> EntryListUpdates { get; }
    IObservable<BroadcastingEvent> GreenFlag { get; }
    string IpAddress { get; }
    IObservable<BroadcastingEvent> LapCompleted { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<BroadcastingEvent> PenaltyMessage { get; }
    int Port { get; }
    IObservable<RealtimeCarUpdate> RealTimeCarUpdates { get; }
    IObservable<RealtimeUpdate> RealTimeUpdates { get; }
    IObservable<BroadcastingEvent> SessionOver { get; }
    IObservable<TrackDataUpdate> TrackDataUpdates { get; }
    int UpdateInterval { get; }
    void Connect(bool autoDetect = true);
    void Dispose();
    void SetActiveCamera(string cameraSetName, string cameraName);
    void SetActiveCamera(string cameraSetName, string cameraName, int carIndex);
    void SetFocus(int carIndex);
    void SetHudPage(string hudPage);
    void Stop();
}