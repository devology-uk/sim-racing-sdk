using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorSessionPhaseChange(SessionPhase OldPhase, SessionPhase NewPhase) : AccMonitorMessageBase { }