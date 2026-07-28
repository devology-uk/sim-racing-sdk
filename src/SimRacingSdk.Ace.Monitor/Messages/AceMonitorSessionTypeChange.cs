#nullable disable

using SimRacingSdk.Ace.Core.Enums;
using SimRacingSdk.Ace.Monitor.Abstractions;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorSessionTypeChange(
    RaceSessionType OldSessionType,
    RaceSessionType NewSessionType) : AceMonitorMessageBase
{
}
