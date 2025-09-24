#nullable disable

namespace SimRacingSdk.Lmu.Core.Models;

public record LmuCarInfo
{
    public string Category { get; init; }
    public string Class { get; init; }
    public string DisplayName { get; init; }
    public string Engine { get; init; }
    public int HeightMm { get; init; }
    public int LengthMm { get; init; }
    public string Manufacturer { get; init; }
    public int PowerBhp { get; init; }
    public int PowerKw { get; init; }
    public string ResultCarType { get; init; }
    public string Transmission { get; init; }
    public int WeightKg { get; init; }
    public int WidthMm { get; init; }
}