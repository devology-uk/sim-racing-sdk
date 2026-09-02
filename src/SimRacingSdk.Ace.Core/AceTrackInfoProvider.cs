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
        new() { Continent = "Europe", Corners = 9, CountryCode = "GBR-ENG", Track = "Brands Hatch", ShortName = "Brands Hatch", Layout = "GP", TrackLengthMeters = 3916, MaxPitSlot = 32, Latitude = 51.3566, Longitude = 0.2614 },
        new() { Continent = "Europe", Corners = 7, CountryCode = "GBR-ENG", Track = "Brands Hatch", ShortName = "Brands Hatch", Layout = "Indy", TrackLengthMeters = 1944, MaxPitSlot = 32, Latitude = 51.3566, Longitude = 0.2614 },
        new() { Continent = "North America", Corners = 20, CountryCode = "USA", Track = "Circuit Of The Americas", ShortName = "COTA", Layout = "GP", TrackLengthMeters = 5615, MaxPitSlot = 34, Latitude = 30.135, Longitude = -97.6341 },
        new() { Continent = "North America", Corners = 19, CountryCode = "USA", Track = "Circuit Of The Americas", ShortName = "COTA", Layout = "National", TrackLengthMeters = 3702, MaxPitSlot = 34, Latitude = 30.135, Longitude = -97.6341 },
        new() { Continent = "Europe", Corners = 19, CountryCode = "BEL", Track = "Circuit de Spa Francorchamps", ShortName = "Spa", Layout = "GP", TrackLengthMeters = 7004, MaxPitSlot = 35, Latitude = 50.4375, Longitude = 5.9685 },
        new() { Continent = "Europe", Corners = 12, CountryCode = "GBR-ENG", Track = "Donington Park", ShortName = "Donington", Layout = "GP", TrackLengthMeters = 4020, MaxPitSlot = 19, Latitude = 52.8304, Longitude = -1.3749 },
        new() { Continent = "Europe", Corners = 10, CountryCode = "GBR-ENG", Track = "Donington Park", ShortName = "Donington", Layout = "National", TrackLengthMeters = 3149, MaxPitSlot = 19, Latitude = 52.8304, Longitude = -1.3749 },
        new() { Continent = "Asia", Corners = 16, CountryCode = "JPN", Track = "Fuji Speedway", ShortName = "Fuji", Layout = "GP", TrackLengthMeters = 4549, MaxPitSlot = 34, Latitude = 35.371667, Longitude = 138.926667 },
        new() { Continent = "Asia", Corners = 14, CountryCode = "JPN", Track = "Fuji Speedway", ShortName = "Fuji", Layout = "GP Short", TrackLengthMeters = 4526, MaxPitSlot = 34, Latitude = 35.371667, Longitude = 138.926667 },
        new() { Continent = "Europe", Corners = 22, CountryCode = "ITA", Track = "Imola", ShortName = "Imola", Layout = "GP", TrackLengthMeters = 4909, MaxPitSlot = 29, Latitude = 44.3408, Longitude = 11.7137 },
        new() { Continent = "Africa", Corners = 16, CountryCode = "ZAF", Track = "Kyalami", ShortName = "Kyalami", Layout = "GP", TrackLengthMeters = 4522, MaxPitSlot = 20, Latitude = -25.9976, Longitude = 28.0682 },
        new() { Continent = "North America", Corners = 11, CountryCode = "USA", Track = "Laguna Seca", ShortName = "Laguna Seca", Layout = "GP", TrackLengthMeters = 4502, MaxPitSlot = 24, Latitude = 36.5845, Longitude = -121.7535 },
        new() { Continent = "Europe", Corners = 11, CountryCode = "ITA", Track = "Monza", ShortName = "Monza", Layout = "GP", TrackLengthMeters = 5793, MaxPitSlot = 30, Latitude = 45.621, Longitude = 9.286 },
        new() { Continent = "Australia", Corners = 23, CountryCode = "AUS", Track = "Mount Panorama", ShortName = "Mount Panorama", Layout = "GP", TrackLengthMeters = 6213, MaxPitSlot = 35, Latitude = -33.4486, Longitude = 149.5547 },
        new() { Continent = "Europe", Corners = 170, CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "24H", TrackLengthMeters = 25947, MaxPitSlot = 30, Latitude = 50.3309, Longitude = 6.9414 },
        new() { Continent = "Europe", Corners = 17, CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "Gp Strecke", TrackLengthMeters = 5148, MaxPitSlot = 30, Latitude = 50.3309, Longitude = 6.9414 },
        new() { Continent = "Europe", Corners = 154, CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "Nordschleife", TrackLengthMeters = 20832, MaxPitSlot = 5, Latitude = 50.3309, Longitude = 6.9414 },
        new() { Continent = "Europe", Corners = 11, CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "Sprint", TrackLengthMeters = 3629, MaxPitSlot = 30, Latitude = 50.3309, Longitude = 6.9414 },
        new() { Continent = "Europe", Corners = 154, CountryCode = "DEU", Track = "Nurburgring", ShortName = "Nurburgring", Layout = "Touristenfarten", TrackLengthMeters = 19300, MaxPitSlot = 50, Latitude = 50.3309, Longitude = 6.9414 },
        new() { Continent = "Europe", Corners = 17, CountryCode = "GBR-ENG", Track = "Oulton Park", ShortName = "Oulton Park", Layout = "International", TrackLengthMeters = 4333, MaxPitSlot = 16, Latitude = 53.1768, Longitude = -2.6168 },
        new() { Continent = "Europe", Corners = 7, CountryCode = "GBR-ENG", Track = "Oulton Park", ShortName = "Oulton Park", Layout = "Fosters", TrackLengthMeters = 2552, MaxPitSlot = 16, Latitude = 53.1768, Longitude = -2.6168 },
        new() { Continent = "Europe", Corners = 13, CountryCode = "FRA", Track = "Paul Ricard", ShortName = "Paul Ricard", Layout = "Layout 1A-V2", TrackLengthMeters = 5770, MaxPitSlot = 34, Latitude = 43.2529, Longitude = 5.7912 },
        new() { Continent = "Europe", Corners = 15, CountryCode = "FRA", Track = "Paul Ricard", ShortName = "Paul Ricard", Layout = "Layout 1C-V2", TrackLengthMeters = 5842, MaxPitSlot = 34, Latitude = 43.2529, Longitude = 5.7912 },
        new() { Continent = "Europe", Corners = 10, CountryCode = "FRA", Track = "Paul Ricard", ShortName = "Paul Ricard", Layout = "Layout 3A", TrackLengthMeters = 3793, MaxPitSlot = 34, Latitude = 43.2529, Longitude = 5.7912 },
        new() { Continent = "Europe", Corners = 11, CountryCode = "FRA", Track = "Paul Ricard", ShortName = "Paul Ricard", Layout = "Layout 3C", TrackLengthMeters = 3841, MaxPitSlot = 34, Latitude = 43.2529, Longitude = 5.7912 },
        new() { Continent = "Europe", Corners = 10, CountryCode = "AUT", Track = "Red Bull Ring", ShortName = "Red Bull Ring", Layout = "GP", TrackLengthMeters = 4318, MaxPitSlot = 28, Latitude = 47.2228736, Longitude = 14.760198 },
        new() { Continent = "Europe", Corners = 5, CountryCode = "AUT", Track = "Red Bull Ring", ShortName = "Red Bull Ring", Layout = "National", TrackLengthMeters = 2336, MaxPitSlot = 28, Latitude = 47.2228736, Longitude = 14.760198 },
        new() { Continent = "North America", Corners = 12, CountryCode = "USA", Track = "Road Atlanta", ShortName = "Road Atlanta", Layout = "GP", TrackLengthMeters = 4088, MaxPitSlot = 28, Latitude = 34.146667, Longitude = -83.817778 },
        new() { Continent = "North America", Corners = 17, CountryCode = "USA", Track = "Sebring International Raceway", ShortName = "Sebring", Layout = "GP", TrackLengthMeters = 5954, MaxPitSlot = 45, Latitude = 27.455, Longitude = -81.35 },
        new() { Continent = "Asia", Corners = 18, CountryCode = "JPN", Track = "Suzuka", ShortName = "Suzuka", Layout = "GP", TrackLengthMeters = 5807, MaxPitSlot = 25, Latitude = 34.8441, Longitude = 136.5329 },
        new() { Continent = "Asia", Corners = 7, CountryCode = "JPN", Track = "Suzuka", ShortName = "Suzuka", Layout = "East", TrackLengthMeters = 2243, MaxPitSlot = 25, Latitude = 34.8441, Longitude = 136.5329 },
        new() { Continent = "Asia", Corners = 9, CountryCode = "JPN", Track = "Suzuka", ShortName = "Suzuka", Layout = "West", TrackLengthMeters = 3466, MaxPitSlot = 10, Latitude = 34.8441, Longitude = 136.5329 },
        new() { Continent = "North America", Corners = 13, CountryCode = "USA", Track = "Watkins Glen", ShortName = "Watkins Glen", Layout = "GP Inner Loop", TrackLengthMeters = 5552, MaxPitSlot = 40, Latitude = 42.3362, Longitude = -76.9252 },
        new() { Continent = "North America", Corners = 7, CountryCode = "USA", Track = "Watkins Glen", ShortName = "Watkins Glen", Layout = "Short Inner Loop", TrackLengthMeters = 3943, MaxPitSlot = 40, Latitude = 42.3362, Longitude = -76.9252 },
        new() { Continent = "North America", Corners = 11, CountryCode = "USA", Track = "Watkins Glen", ShortName = "Watkins Glen", Layout = "GP", TrackLengthMeters = 5435, MaxPitSlot = 40, Latitude = 42.3362, Longitude = -76.9252 },
        new() { Continent = "North America", Corners = 7, CountryCode = "USA", Track = "Watkins Glen", ShortName = "Watkins Glen", Layout = "Short", TrackLengthMeters = 3907, MaxPitSlot = 40, Latitude = 42.3362, Longitude = -76.9252 }
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
