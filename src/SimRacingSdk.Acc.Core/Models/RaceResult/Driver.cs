#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record Driver
{
    [JsonIgnore]
    public string FullName => $"{this.FirstName} {this.LastName}";
    [JsonPropertyName("firstName")]
    public string FirstName { get; init; }
    [JsonPropertyName("lastName")]
    public string LastName { get; init; }
    [JsonPropertyName("playerId")]
    public string PlayerId { get; init; }
    [JsonPropertyName("shortName")]
    public string ShortName { get; init; }

    public string GetDisplayName()
    {
        if(string.IsNullOrWhiteSpace(this.FirstName) && string.IsNullOrWhiteSpace(this.LastName))
        {
            return string.IsNullOrWhiteSpace(this.ShortName)? "Unknown": this.ShortName;
        }

        if(string.IsNullOrWhiteSpace(this.FirstName) && !string.IsNullOrWhiteSpace(this.LastName))
        {
            return this.LastName;
        }

        if(!string.IsNullOrWhiteSpace(this.FirstName) && string.IsNullOrWhiteSpace(this.LastName))
        {
            return this.FirstName;
        }

        return $"{this.FirstName[..1]}. {this.LastName}";
    }
}