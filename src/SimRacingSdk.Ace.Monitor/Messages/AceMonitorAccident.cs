#nullable disable

using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.Udp.Enums;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorAccident : AceMonitorMessageBase
{
    public byte AceCarModelId { get; init; }
    public CupCategory CarCupCategory { get; set; }
    public int CarIndex { get; init; }
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public int CurrentDriverIndex { get; init; }
    public AceMonitorDriver CurrentMonitorDriver { get; init; }
    public int RaceNumber { get; init; }
    public string SessionId { get; init; }
    public string TeamName { get; init; }
}
