#nullable disable

using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorSessionChange(
    RaceSessionType OldSessionType,
    RaceSessionType NewSessionType) : AccMonitorMessageBase
{
}