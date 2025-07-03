using System.Text.Json.Serialization;

namespace SimRacingSdk.Lmu.Core.Models
{
    #nullable disable
    public class LmuSettings
    {
        [JsonPropertyName("DRIVER")]
        public LmuDriverSettings DriverSettings { get; set; } = new();
    }
}