using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Udp.Abstractions;

public interface IAccUdpConnection : IDisposable
{
    IObservable<BroadcastingEvent> BroadcastingEvents { get; }
    string CommandPassword { get; }
    string ConnectionIdentifier { get; }
    string ConnectionPassword { get; }
    IObservable<ConnectionState> ConnectionStateChanges { get; }
    string DisplayName { get; }
    IObservable<EntryListUpdate> EntryListUpdates { get; }
    string IpAddress { get; }
    IObservable<LogMessage> LogMessages { get; }
    int Port { get; }
    IObservable<RealtimeCarUpdate> RealTimeCarUpdates { get; }
    IObservable<RealtimeUpdate> RealTimeUpdates { get; }
    IObservable<TrackDataUpdate> TrackDataUpdates { get; }
    int UpdateInterval { get; }
    void Connect(bool autoDetect = true);
    void RequestEntryList();
    void SetActiveCamera(string cameraSetName, string cameraName);
    void SetActiveCamera(string cameraSetName, string cameraName, int carIndex);
    void SetFocus(int carIndex);
    void SetHudPage(string hudPage);
    void Stop();
}