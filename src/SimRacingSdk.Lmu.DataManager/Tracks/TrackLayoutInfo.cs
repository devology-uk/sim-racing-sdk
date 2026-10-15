namespace SimRacingSdk.Lmu.DataManager.Tracks;

public record TrackLayoutInfo
{
    public required int Corners { get; init; }
    public required int LengthM { get; init; }
    public required string Name { get; init; }

    // The exact track names LMU reports for this layout (shared memory and result files), e.g.
    // "Bahrain Outer Circuit" - they don't reliably contain the layout's own Name, so they're
    // captured rather than guessed. Matched before any fallback.
    public List<string> RawTrackNames { get; init; } = [];
}
