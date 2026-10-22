#nullable disable

namespace SimRacingSdk.Lmu.Core.Models;

public record LmuTrackLayoutInfo
{
    public double LengthKm => this.LengthM / 1000.0;
    public double LengthMi => this.LengthM / 1609.34;
    public int Corners { get; init; }
    public int LengthM { get; init; }
    public string Name { get; init; }

    // The exact track names LMU reports for this layout, where known.
    public IReadOnlyList<string> RawTrackNames { get; init; } = [];
}
