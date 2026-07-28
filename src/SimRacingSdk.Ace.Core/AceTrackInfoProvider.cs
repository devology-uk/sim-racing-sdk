using System.Collections.ObjectModel;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core;

public class AceTrackInfoProvider : IAceTrackInfoProvider
{
    private static AceTrackInfoProvider? singletonInstance;

    private readonly List<AceTrackInfo> tracks =
    [
        new() { Track = "Brands Hatch", Layout = "GP", TrackLengthMeters = 3916, MaxPitSlot = 32 },
        new() { Track = "Brands Hatch", Layout = "Indy", TrackLengthMeters = 1944, MaxPitSlot = 32 },
        new() { Track = "Circuit Of The Americas", Layout = "GP", TrackLengthMeters = 5515, MaxPitSlot = 34 },
        new() { Track = "Circuit Of The Americas", Layout = "National", TrackLengthMeters = 3702, MaxPitSlot = 34 },
        new() { Track = "Circuit de Spa Francorchamps", Layout = "GP", TrackLengthMeters = 7004, MaxPitSlot = 36 },
        new() { Track = "Donington Park", Layout = "GP", TrackLengthMeters = 4020, MaxPitSlot = 19 },
        new() { Track = "Donington Park", Layout = "National", TrackLengthMeters = 3149, MaxPitSlot = 19 },
        new() { Track = "Fuji Speedway", Layout = "GP", TrackLengthMeters = 4549, MaxPitSlot = 34 },
        new() { Track = "Fuji Speedway", Layout = "GP Short", TrackLengthMeters = 4526, MaxPitSlot = 34 },
        new() { Track = "Imola", Layout = "GP", TrackLengthMeters = 4909, MaxPitSlot = 29 },
        new() { Track = "Kyalami", Layout = "GP", TrackLengthMeters = 4522, MaxPitSlot = 20 },
        new() { Track = "Laguna Seca", Layout = "GP", TrackLengthMeters = 3602, MaxPitSlot = 24 },
        new() { Track = "Monza", Layout = "GP", TrackLengthMeters = 5793, MaxPitSlot = 30 },
        new() { Track = "Mount Panorama", Layout = "GP", TrackLengthMeters = 6213, MaxPitSlot = 35 },
        new() { Track = "Nurburgring", Layout = "24h", TrackLengthMeters = 25947, MaxPitSlot = 30 },
        new() { Track = "Nurburgring", Layout = "Gp Strecke", TrackLengthMeters = 5148, MaxPitSlot = 30 },
        new() { Track = "Nurburgring", Layout = "Nordschleife", TrackLengthMeters = 20832, MaxPitSlot = 5 },
        new() { Track = "Nurburgring", Layout = "Sprint", TrackLengthMeters = 3629, MaxPitSlot = 30 },
        new() { Track = "Nurburgring", Layout = "Touristenfahrten", TrackLengthMeters = 19300, MaxPitSlot = 50 },
        new() { Track = "Oulton Park", Layout = "International", TrackLengthMeters = 4333, MaxPitSlot = 16 },
        new() { Track = "Oulton Park", Layout = "Fosters", TrackLengthMeters = 2662, MaxPitSlot = 16 },
        new() { Track = "Paul Ricard", Layout = "Layout 1A-V2", TrackLengthMeters = 5770, MaxPitSlot = 34 },
        new() { Track = "Paul Ricard", Layout = "Layout 1C-V2", TrackLengthMeters = 5842, MaxPitSlot = 34 },
        new() { Track = "Paul Ricard", Layout = "Layout 3A", TrackLengthMeters = 3793, MaxPitSlot = 34 },
        new() { Track = "Paul Ricard", Layout = "Layout 3C", TrackLengthMeters = 3841, MaxPitSlot = 34 },
        new() { Track = "Red Bull Ring", Layout = "GP", TrackLengthMeters = 4318, MaxPitSlot = 28 },
        new() { Track = "Red Bull Ring", Layout = "National", TrackLengthMeters = 2336, MaxPitSlot = 28 },
        new() { Track = "Road Atlanta", Layout = "GP", TrackLengthMeters = 4088, MaxPitSlot = 28 },
        new() { Track = "Sebring International Raceway", Layout = "GP", TrackLengthMeters = 5954, MaxPitSlot = 45 },
        new() { Track = "Suzuka", Layout = "GP", TrackLengthMeters = 5807, MaxPitSlot = 25 },
        new() { Track = "Suzuka", Layout = "East", TrackLengthMeters = 2243, MaxPitSlot = 25 },
        new() { Track = "Suzuka", Layout = "West", TrackLengthMeters = 3466, MaxPitSlot = 10 },
        new() { Track = "Watkins Glen International", Layout = "GP Inner Loop", TrackLengthMeters = 5552, MaxPitSlot = 40 },
        new() { Track = "Watkins Glen International", Layout = "Short Inner Loop", TrackLengthMeters = 3943, MaxPitSlot = 40 },
        new() { Track = "Watkins Glen International", Layout = "GP", TrackLengthMeters = 5435, MaxPitSlot = 40 },
        new() { Track = "Watkins Glen International", Layout = "Short", TrackLengthMeters = 3907, MaxPitSlot = 40 }
    ];

    public static AceTrackInfoProvider Instance => singletonInstance ??= new AceTrackInfoProvider();

    public AceTrackInfo? FindByTrackAndLayout(string track, string layout)
    {
        return this.tracks.FirstOrDefault(t => t.Track == track && t.Layout == layout);
    }

    public ReadOnlyCollection<string> GetTrackNames()
    {
        return this.tracks.Select(t => t.Track)
                   .Distinct()
                   .ToList()
                   .AsReadOnly();
    }

    public ReadOnlyCollection<AceTrackInfo> GetLayoutsForTrack(string track)
    {
        return this.tracks.Where(t => t.Track == track)
                   .ToList()
                   .AsReadOnly();
    }

    public ReadOnlyCollection<AceTrackInfo> GetTrackInfos()
    {
        return this.tracks.AsReadOnly();
    }
}
