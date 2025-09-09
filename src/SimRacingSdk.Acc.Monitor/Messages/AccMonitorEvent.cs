using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorEvent(int TrackId, string TrackName, float TrackMeters) : AccMonitorMessageBase { }