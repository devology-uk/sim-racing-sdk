#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Lmu.Core.Models;

public class LmuSettings
{
    [JsonPropertyName("DRIVER")]
    public LmuDriverSettings DriverSettings { get; set; } = new();
}