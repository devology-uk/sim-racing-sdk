#nullable disable

using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccSession(string EventId, string SessionType) : AccMonitorMessageBase { }