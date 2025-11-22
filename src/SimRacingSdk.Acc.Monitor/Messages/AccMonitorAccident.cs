#nullable disable

using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.Udp.Enums;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorAccident : AccMonitorMessageBase
{
    public byte AccCarModelId { get; init; }
    public CupCategory CarCupCategory { get; set; }
    public int CarIndex { get; init; }
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public int ConnectionId { get; init; }
    public int CurrentDriverIndex { get; init; }
    public AccMonitorDriver CurrentMonitorDriver { get; init; }
    public int RaceNumber { get; init; }
    public string SessionId { get; init; }
    public string TeamName { get; init; }
}