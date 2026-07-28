#nullable disable

namespace SimRacingSdk.Ace.Core.Models;

// Sourced from the AC Evo Dedicated Server's events_practice.json (track/layout/track_length/
// max_pit_slot fields; event_name deliberately excluded per Mike 2026-07-27). Evo's data has
// nothing equivalent to Acc's AccTrackInfo (Country/CountryCode/Latitude/Longitude/Corners), so
// this is shaped around what's actually available rather than mirroring that model.
public record AceTrackInfo
{
    public string Track { get; init; }
    public string Layout { get; init; }
    public float TrackLengthMeters { get; init; }
    public int MaxPitSlot { get; init; }
}
