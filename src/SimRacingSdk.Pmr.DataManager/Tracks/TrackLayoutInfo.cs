namespace SimRacingSdk.Pmr.DataManager.Tracks;

public record TrackLayoutInfo
{
    public required int GridSlots { get; init; }
    public required double LengthMeters { get; init; }
    public required string Name { get; init; }
    public required int Turns { get; init; }
}
