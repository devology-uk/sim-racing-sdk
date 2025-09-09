using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorSessionPhase(string EventId, string SessionId, string Phase) : AccMonitorMessageBase { }