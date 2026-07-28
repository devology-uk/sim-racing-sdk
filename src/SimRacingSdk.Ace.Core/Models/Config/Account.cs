#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Ace.Core.Models.Config;

// Shape confirmed 2026-07-28 from a real local.driverdescriptor.json (Evo's account.json
// equivalent) - much thinner than Acc's Account: no email, discord username, game platform user
// id or local machine id. player_id is the driver's SteamID64.
public class Account
{
    [JsonIgnore()]
    public string DriverDisplayName => $"{this.FirstName[..1]}. {this.LastName}";
    [JsonIgnore()]
    public string FullName => $"{this.FirstName} {this.LastName}";

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    [JsonPropertyName("guid")]
    public AccountGuid Guid { get; set; }
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
    [JsonPropertyName("nation")]
    public string Nation { get; set; }
    [JsonPropertyName("nickname")]
    public string NickName { get; set; }
    [JsonPropertyName("player_id")]
    public string PlayerId { get; set; }
}

public class AccountGuid
{
    [JsonPropertyName("a")]
    public string A { get; set; }
    [JsonPropertyName("b")]
    public string B { get; set; }
}
