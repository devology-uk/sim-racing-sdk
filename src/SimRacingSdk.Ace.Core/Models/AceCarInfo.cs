#nullable disable

namespace SimRacingSdk.Ace.Core.Models;

// Only Name and DisplayName are populated - the AC Evo Dedicated Server's cars.json (the only
// source available) exposes nothing equivalent to Acc's Class/ManufacturerTag/MaxFuel/MaxRpm/Year
// per car, so those fields don't exist here rather than being fabricated.
public record AceCarInfo
{
    public string Name { get; init; }
    public string DisplayName { get; init; }
}
