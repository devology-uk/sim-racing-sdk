#nullable disable

using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorSession(string EventId, string SessionType, TimeSpan Duration) : AccMonitorMessageBase
{
    public bool IsOnline { get; internal set; }
    public int NumberOfCars { get; internal set; }
}