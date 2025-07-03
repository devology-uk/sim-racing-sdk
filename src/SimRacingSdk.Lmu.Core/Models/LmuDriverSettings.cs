using System.Text.Json.Serialization;

namespace SimRacingSdk.Lmu.Core.Models;

#nullable disable
public class LmuDriverSettings
{
    [JsonPropertyName("Birth Date")]
    public string DateOfBirth { get; set; }

    [JsonPropertyName("Team")]
    public string LastTeamName { get; set; }

    [JsonPropertyName("Vehicle")]
    public string LastVehicleName { get; set; }

    public string Location { get; set; }

    public string Nationality { get; set; }

    [JsonPropertyName("Player Name")]
    public string PlayerName { get; set; } = "Not Set";
        
    [JsonPropertyName("Player Nick")]
    public string PlayerNickname { get; set; } = "Not Set";
}