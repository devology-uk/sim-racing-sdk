using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccGreenFlag(string? SessionId) : AccMonitorMessageBase { }