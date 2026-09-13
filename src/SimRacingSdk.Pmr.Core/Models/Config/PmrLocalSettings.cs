namespace SimRacingSdk.Pmr.Core.Models.Config;

public record PmrLocalSettings
{
    public bool UdpEnabled { get; init; }
    public int UdpFrequencyHz { get; init; }
    public string UdpHost { get; init; } = string.Empty;
    public int UdpPort { get; init; }
}
