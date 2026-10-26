using System.Text.Json.Serialization;

namespace SimRacingSdk.Lmu.Core.Models;

public class LmuGameOptions
{
    // Null when the key is missing, as in settings files from older versions of the game.
    [JsonPropertyName("Enable external plugins")]
    public bool? EnableExternalPlugins { get; set; }
}
