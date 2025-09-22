#nullable disable

using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.Udp.Enums;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorEventEntry : AccMonitorMessageBase
{
    public byte AccCarModelId { get; init; }
    public CupCategory CarCupCategory { get; set; }
    public int CarIndex { get; init; }
    public CarLocation CarLocation { get; internal set; }
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public int CurrentDriverIndex { get; init; }
    public AccMonitorDriver CurrentMonitorDriver { get; init; }
    public List<AccMonitorDriver> Drivers { get; init; }
    public string EventId { get; init; }
    public int RaceNumber { get; init; }
    public string TeamName { get; init; }
}