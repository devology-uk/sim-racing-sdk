namespace SimRacingSdk.Pmr.Core.Models.Config;

public record PmrUdpSettings
{
    public int FrequencyHz { get; init; }
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
}
