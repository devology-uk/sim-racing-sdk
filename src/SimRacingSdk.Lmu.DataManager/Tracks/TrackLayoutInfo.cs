namespace SimRacingSdk.Lmu.DataManager.Tracks;

public record TrackLayoutInfo
{
    public required int Corners { get; init; }
    public required int LengthM { get; init; }
    public required string Name { get; init; }
}
