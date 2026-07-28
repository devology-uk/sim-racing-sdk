using SimRacingSdk.Ace.Monitor.Abstractions;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorGreenFlag(string? SessionId) : AceMonitorMessageBase { }
