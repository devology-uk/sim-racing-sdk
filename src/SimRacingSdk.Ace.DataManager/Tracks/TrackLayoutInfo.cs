namespace SimRacingSdk.Ace.DataManager.Tracks;

public record TrackLayoutInfo
{
    public required int Corners { get; init; }
    public required int LengthMeters { get; init; }

    // Varies by layout (e.g. at the Nurburgring, 5 on the Nordschleife and 50 on the tourist layout).
    public required int MaxPitSlot { get; init; }

    public required string Name { get; init; }
}
