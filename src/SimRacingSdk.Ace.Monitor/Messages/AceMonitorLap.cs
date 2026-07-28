#nullable disable

using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.Udp.Enums;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorLap : AceMonitorMessageBase
{
    public byte AceCarModelId { get; init; }
    public CupCategory CarCupCategory { get; init; }
    public ushort CarIndex { get; init; }
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public AceMonitorDriver CurrentDriver { get; init; }
    public int CurrentDriverIndex { get; init; }
    public string LapTime { get; init; }
    public int RaceNumber { get; init; }
    public string SessionId { get; init; }
    public string TeamName { get; init; }
}
