#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Ace.Core.Models.Config;

// Shape assumed identical to Acc's account.json - unverified against a real Ace Evo install.
public class Account
{
    [JsonIgnore()]
    public string DriverDisplayName => $"{this.FirstName[..1]}. {this.LastName}";
    [JsonIgnore()]
    public string FullName => $"{this.FirstName} {this.LastName}";

    [JsonPropertyName("country")]
    public string Country { get; set; }
    [JsonPropertyName("discordUserName")]
    public string DiscordUserName { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("gamePlatformUserId")]
    public string GamePlatformUserId { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("localMachineId")]
    public string LocalMachineId { get; set; }
    [JsonPropertyName("nickName")]
    public string NickName { get; set; }
}
