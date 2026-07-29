using System.Collections.ObjectModel;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core;

public class AceTrackInfoProvider : IAceTrackInfoProvider
{
    private static AceTrackInfoProvider? singletonInstance;

    // CountryCode uses JPN rather than the game's own "JAP" label, matching the ISO-alpha-3 flag
    // assets under Images/Flags (same convention as Acc/Lmu's track providers).
    private readonly List<AceTrackInfo> tracks =
    [
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Track = "Brands Hatch", ShortName = "Brands Hatch", Layout = "GP", TrackLengthMeters = 3916, MaxPitSlot = 32 },
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Track = "Brands Hatch", ShortName = "Brands Hatch", Layout = "Indy", TrackLengthMeters = 1944, MaxPitSlot = 32 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Circuit Of The Americas", ShortName = "COTA", Layout = "GP", TrackLengthMeters = 5615, MaxPitSlot = 34 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Circuit Of The Americas", ShortName = "COTA", Layout = "National", TrackLengthMeters = 3702, MaxPitSlot = 34 },
        new() { Continent = "Europe", CountryCode = "BEL", Track = "Circuit de Spa Francorchamps", ShortName = "Spa", Layout = "GP", TrackLengthMeters = 7004, MaxPitSlot = 35 },
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Track = "Donington Park", ShortName = "Donington", Layout = "GP", TrackLengthMeters = 4020, MaxPitSlot = 19 },
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Track = "Donington Park", ShortName = "Donington", Layout = "National", TrackLengthMeters = 3149, MaxPitSlot = 19 },
        new() { Continent = "Asia", CountryCode = "JPN", Track = "Fuji Speedway", ShortName = "Fuji", Layout = "GP", TrackLengthMeters = 4549, MaxPitSlot = 34 },
        new() { Continent = "Asia", CountryCode = "JPN", Track = "Fuji Speedway", ShortName = "Fuji", Layout = "GP Short", TrackLengthMeters = 4526, MaxPitSlot = 34 },
        new() { Continent = "Europe", CountryCode = "ITA", Track = "Imola", ShortName = "Imola", Layout = "GP", TrackLengthMeters = 4909, MaxPitSlot = 29 },
        new() { Continent = "Africa", CountryCode = "ZAF", Track = "Kyalami", ShortName = "Kyalami", Layout = "GP", TrackLengthMeters = 4522, MaxPitSlot = 20 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Laguna Seca", ShortName = "Laguna Seca", Layout = "GP", TrackLengthMeters = 4502, MaxPitSlot = 24 },
        new() { Continent = "Europe", CountryCode = "ITA", Track = "Monza", ShortName = "Monza", Layout = "GP", TrackLengthMeters = 5793, MaxPitSlot = 30 },
        new() { Continent = "Australia", CountryCode = "AUS", Track = "Mount Panorama", ShortName = "Mount Panorama", Layout = "GP", TrackLengthMeters = 6213, MaxPitSlot = 35 },
        new() { Continent = "Europe", CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "24H", TrackLengthMeters = 25947, MaxPitSlot = 30 },
        new() { Continent = "Europe", CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "Gp Strecke", TrackLengthMeters = 5148, MaxPitSlot = 30 },
        new() { Continent = "Europe", CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "Nordschleife", TrackLengthMeters = 20832, MaxPitSlot = 5 },
        new() { Continent = "Europe", CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "Sprint", TrackLengthMeters = 3629, MaxPitSlot = 30 },
        new() { Continent = "Europe", CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "Touristenfarten", TrackLengthMeters = 19300, MaxPitSlot = 50 },
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Track = "Oulton Park", ShortName = "Oulton Park", Layout = "International", TrackLengthMeters = 4333, MaxPitSlot = 16 },
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Track = "Oulton Park", ShortName = "Oulton Park", Layout = "Fosters", TrackLengthMeters = 2552, MaxPitSlot = 16 },
        new() { Continent = "Europe", CountryCode = "FRA", Track = "Paul Ricard", ShortName = "Paul Ricard", Layout = "Layout 1A-V2", TrackLengthMeters = 5770, MaxPitSlot = 34 },
        new() { Continent = "Europe", CountryCode = "FRA", Track = "Paul Ricard", ShortName = "Paul Ricard", Layout = "Layout 1C-V2", TrackLengthMeters = 5842, MaxPitSlot = 34 },
        new() { Continent = "Europe", CountryCode = "FRA", Track = "Paul Ricard", ShortName = "Paul Ricard", Layout = "Layout 3A", TrackLengthMeters = 3793, MaxPitSlot = 34 },
        new() { Continent = "Europe", CountryCode = "FRA", Track = "Paul Ricard", ShortName = "Paul Ricard", Layout = "Layout 3C", TrackLengthMeters = 3841, MaxPitSlot = 34 },
        new() { Continent = "Europe", CountryCode = "AUT", Track = "Red Bull Ring", ShortName = "Red Bull Ring", Layout = "GP", TrackLengthMeters = 4318, MaxPitSlot = 28 },
        new() { Continent = "Europe", CountryCode = "AUT", Track = "Red Bull Ring", ShortName = "Red Bull Ring", Layout = "National", TrackLengthMeters = 2336, MaxPitSlot = 28 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Road Atlanta", ShortName = "Road Atlanta", Layout = "GP", TrackLengthMeters = 4088, MaxPitSlot = 28 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Sebring International Raceway", ShortName = "Sebring", Layout = "GP", TrackLengthMeters = 5954, MaxPitSlot = 45 },
        new() { Continent = "Asia", CountryCode = "JPN", Track = "Suzuka", ShortName = "Suzuka", Layout = "GP", TrackLengthMeters = 5807, MaxPitSlot = 25 },
        new() { Continent = "Asia", CountryCode = "JPN", Track = "Suzuka", ShortName = "Suzuka", Layout = "East", TrackLengthMeters = 2243, MaxPitSlot = 25 },
        new() { Continent = "Asia", CountryCode = "JPN", Track = "Suzuka", ShortName = "Suzuka", Layout = "West", TrackLengthMeters = 3466, MaxPitSlot = 10 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Watkins Glen", ShortName = "Watkins Glen", Layout = "GP Inner Loop", TrackLengthMeters = 5552, MaxPitSlot = 40 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Watkins Glen", ShortName = "Watkins Glen", Layout = "Short Inner Loop", TrackLengthMeters = 3943, MaxPitSlot = 40 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Watkins Glen", ShortName = "Watkins Glen", Layout = "GP", TrackLengthMeters = 5435, MaxPitSlot = 40 },
        new() { Continent = "North America", CountryCode = "USA", Track = "Watkins Glen", ShortName = "Watkins Glen", Layout = "Short", TrackLengthMeters = 3907, MaxPitSlot = 40 }
    ];

    public static AceTrackInfoProvider Instance => singletonInstance ??= new AceTrackInfoProvider();

    public AceTrackInfo? FindByTrackAndLayout(string track, string layout)
    {
        return this.tracks.FirstOrDefault(t => t.Track == track && t.Layout == layout);
    }

    public ReadOnlyCollection<string> GetContinents()
    {
        return this.tracks.Select(t => t.Continent)
                   .Distinct()
                   .OrderBy(c => c)
                   .ToList()
                   .AsReadOnly();
    }

    public ReadOnlyCollection<string> GetTrackNamesForContinent(string continent)
    {
        return this.tracks.Where(t => t.Continent == continent)
                   .Select(t => t.Track)
                   .Distinct()
                   .OrderBy(t => t)
                   .ToList()
                   .AsReadOnly();
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
