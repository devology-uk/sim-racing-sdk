#nullable disable

using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorSessionTypeChange(
    RaceSessionType OldSessionType,
    RaceSessionType NewSessionType) : AccMonitorMessageBase
{
}