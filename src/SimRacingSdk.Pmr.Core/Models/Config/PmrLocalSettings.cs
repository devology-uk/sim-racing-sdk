namespace SimRacingSdk.Pmr.Core.Models.Config;

public record PmrLocalSettings
{
    // The game's Preferences set each quantity's units separately.
    public bool IsImperialDistance { get; init; }
    public bool IsImperialFluid { get; init; }
    public bool IsImperialPressure { get; init; }
    public bool IsImperialWeight { get; init; }
    public bool UdpEnabled { get; init; }
    public int UdpFrequencyHz { get; init; }
    public string UdpHost { get; init; } = string.Empty;
    public int UdpPort { get; init; }
}
