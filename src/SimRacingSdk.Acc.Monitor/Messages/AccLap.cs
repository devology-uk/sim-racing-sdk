#nullable disable

using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.Udp.Enums;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccLap : AccMonitorMessageBase
{
    public byte AccCarModelId { get; init; }
    public CupCategory CarCupCategory { get; init; }
    public ushort CarIndex { get; init; }
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public AccDriver CurrentDriver { get; init; }
    public int CurrentDriverIndex { get; init; }
    public string EventId { get; init; }
    public string LapTime { get; init; }
    public int RaceNumber { get; init; }
    public string TeamName { get; init; }
}