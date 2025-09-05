using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccSessionPhase(string EventId, string SessionId, string Phase) : AccMonitorMessageBase { }