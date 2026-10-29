using SimRacingSdk.R3e.Core.Enums;

namespace SimRacingSdk.R3e.Core.Models.Config;

public record R3eGameOptions
{
    public string LanguageCode { get; init; } = string.Empty;
    public R3eUnitSystem UnitSystem { get; init; }
}
