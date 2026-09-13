namespace SimRacingSdk.Pmr.Core.Models.Config;

public record PmrDriverProfile
{
    // Raw value as the game stores it (e.g. "united_kingdom") - same snake_case convention as
    // track Country, but the nationality picker's full option list isn't confirmed yet, so this
    // isn't normalised to a display name/ISO code the way PmrTrackInfo.Country/CountryCode are.
    public string Nationality { get; init; } = string.Empty;
}
