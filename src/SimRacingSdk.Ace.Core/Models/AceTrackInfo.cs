#nullable disable

namespace SimRacingSdk.Ace.Core.Models;

// Track/layout/length/pit-slot data originally sourced from the AC Evo Dedicated Server's
// events_practice.json; Continent, CountryCode and ShortName added 2026-07-29 from Mike's own
// ace-tracks.csv, hand-gathered from the game's own track list (repo root).
public record AceTrackInfo
{
    public string Continent { get; init; }
    public int Corners { get; init; }
    public string CountryCode { get; init; }
    public string Track { get; init; }
    public string ShortName { get; init; }
    public string Layout { get; init; }
    public float TrackLengthMeters { get; init; }
    public int MaxPitSlot { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}
