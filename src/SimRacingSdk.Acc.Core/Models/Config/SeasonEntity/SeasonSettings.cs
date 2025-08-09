#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record SeasonSettings
{
    [JsonPropertyName("assists")]
    public Assists Assists { get; init; }
    [JsonPropertyName("eventOnlineReference")]
    public int EventOnlineReference { get; init; }
    [JsonPropertyName("events")]
    public IList<Event> Events { get; init; }
    [JsonPropertyName("gameplay")]
    public Gameplay Gameplay { get; init; }
    [JsonPropertyName("graphics")]
    public Graphics Graphics { get; init; }
    [JsonPropertyName("sessionGamePlay")]
    public SessionGameplay SessionGameplay { get; init; }
    [JsonPropertyName("sessionGameplayOverride")]
    public IList<SessionGameplay> SessionGameplayOverride { get; init; }
    [JsonPropertyName("sessionOnline")]
    public SessionOnline SessionOnline { get; init; }
    [JsonPropertyName("sessionOnlineOverride")]
    public IList<SessionOnline> SessionOnlineOverride { get; init; }
    [JsonPropertyName("sessionRealism")]
    public SessionRealism SessionRealism { get; init; }
    [JsonPropertyName("sessionRealismOverride")]
    public IList<SessionRealism> SessionRealismOverride { get; init; }
}