#nullable disable

namespace SimRacingSdk.Acc.Core.Models;

public record AccCarInfo
{
    public string AccName { get; init; }
    public string Class { get; init; }
    public string DisplayName { get; init; }
    public string ManufacturerTag { get; init; }
    public int MaxFuel { get; init; }
    public int MaxRpm { get; init; }
    public byte ModelId { get; init; }
    public int Year { get; init; }
}