using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorGreenFlag(string? SessionId) : AccMonitorMessageBase { }