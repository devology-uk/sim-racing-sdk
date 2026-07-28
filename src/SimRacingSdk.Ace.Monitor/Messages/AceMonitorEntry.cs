#nullable disable

using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.Udp.Enums;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorEntry : AceMonitorMessageBase
{
    public byte AceCarModelId { get; init; }
    public CupCategory CarCupCategory { get; set; }
    public int CarIndex { get; init; }
    public CarLocation CarLocation { get; internal set; }
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public string ConnectionId { get; init; }
    public int CurrentDriverIndex { get; init; }
    public AceMonitorDriver CurrentMonitorDriver { get; init; }
    public List<AceMonitorDriver> Drivers { get; init; }
    public int RaceNumber { get; init; }
    public string TeamName { get; init; }
}
