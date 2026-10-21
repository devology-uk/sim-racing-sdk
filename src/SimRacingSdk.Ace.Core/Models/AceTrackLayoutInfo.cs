#nullable disable

namespace SimRacingSdk.Ace.Core.Models;

public record AceTrackLayoutInfo
{
    public int Corners { get; init; }
    public int LengthMeters { get; init; }
    public int MaxPitSlot { get; init; }
    public string Name { get; init; }
}
