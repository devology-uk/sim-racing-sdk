#nullable disable

namespace SimRacingSdk.Ams2.Core.Models;

public record Ams2TrackLayoutInfo
{
    public double LengthKm => this.LengthM / 1000.0;
    public double LengthMi => this.LengthM / 1609.34;
    public int Corners { get; init; }
    public string Grade { get; init; }
    public int LengthM { get; init; }
    public int MaxGridSize { get; init; }
    public string Name { get; init; }
    public string TrackType { get; init; }
}