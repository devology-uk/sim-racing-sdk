#nullable disable

using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorSession(string EventId, string SessionType) : AccMonitorMessageBase { }