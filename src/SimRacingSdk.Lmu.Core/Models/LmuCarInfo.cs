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
    // Litres, whole number - real-world regulated fuel cell/tank capacity where a specific figure was found for
    // this exact car, otherwise the closest Class/Category match's figure (LMU gives no way to read this from
    // the game itself, so it's sourced entirely from real-world research - see sim-racer-tools' own CLAUDE.md,
    // "Race Engineer feature" section, for the full per-car sourcing/confidence trail, added 2026-09-02).
    public int MaxFuel { get; init; }
    // Engine rev limiter RPM - same sourcing/fallback rule as MaxFuel.
    public int MaxRpm { get; init; }
    public string ModelId { get; init; }
    public int PowerBhp { get; init; }
    public int PowerKw { get; init; }
    public string ResultCarType { get; init; }
    public string Transmission { get; init; }
    public int WeightKg { get; init; }
    public int WidthMm { get; init; }
    // The model year as this catalog/game represents the car, not necessarily its real-world homologation year -
    // taken from ModelId's year suffix where present (the catalog's own authority on "which year variant this
    // row is"), falling back to real-world research only for the handful of rows with no ModelId yet.
    public int Year { get; init; }
}