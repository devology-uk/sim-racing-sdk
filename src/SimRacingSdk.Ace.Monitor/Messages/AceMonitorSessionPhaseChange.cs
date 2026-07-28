using SimRacingSdk.Ace.Core.Enums;
using SimRacingSdk.Ace.Monitor.Abstractions;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorSessionPhaseChange(SessionPhase OldPhase, SessionPhase NewPhase) : AceMonitorMessageBase { }
