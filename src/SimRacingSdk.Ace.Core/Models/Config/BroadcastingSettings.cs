using System.Text.Json.Serialization;

namespace SimRacingSdk.Ace.Core.Models.Config;

// Shape assumed identical to Acc's broadcasting.json - unverified against a real Ace Evo install.
public class BroadcastingSettings
{
    [JsonPropertyName("commandPassword")]
    public string CommandPassword { get; set; } = string.Empty;
    [JsonPropertyName("connectionPassword")]
    public string ConnectionPassword { get; set; } = string.Empty;
    [JsonPropertyName("updListenerPort")]
    public int UdpListenerPort { get; set; }
}
