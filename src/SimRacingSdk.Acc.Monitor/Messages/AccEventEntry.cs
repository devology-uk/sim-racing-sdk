#nullable disable

using SimRacingSdk.Acc.Udp.Enums;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccEventEntry()
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();
    public byte AccCarModelId { get; init; }
    public CupCategory CarCupCategory { get; set; }
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public AccDriverEntry CurrentDriver { get; init; }
    public int CurrentDriverIndex { get; init; }
    public List<AccDriverEntry> Drivers { get; init; }
    public string EventId { get; init; }
    public int Index { get; init; }
    public int RaceNumber { get; init; }
    public string TeamName { get; init; }
}