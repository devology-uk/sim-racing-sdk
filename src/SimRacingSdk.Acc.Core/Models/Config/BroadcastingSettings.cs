using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config;

public class BroadcastingSettings
{
    [JsonPropertyName("commandPassword")]
    public string CommandPassword { get; set; } = string.Empty;
    [JsonPropertyName("connectionPassword")]
    public string ConnectionPassword { get; set; } = string.Empty;
    [JsonPropertyName("updListenerPort")]
    public int UdpListenerPort { get; set; }
}