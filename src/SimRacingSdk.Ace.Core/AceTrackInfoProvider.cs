using System.Collections.ObjectModel;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core;

public class AceTrackInfoProvider : IAceTrackInfoProvider
{
    private static AceTrackInfoProvider? singletonInstance;

    private readonly List<AceTrackInfo> tracks =
    [
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Latitude = 51.3566, Layouts = [new() { Corners = 9, LengthMeters = 3916, MaxPitSlot = 32, Name = "GP" }, new() { Corners = 7, LengthMeters = 1944, MaxPitSlot = 32, Name = "Indy" }], Longitude = 0.2614, Name = "Brands Hatch", ShortName = "Brands Hatch" },
        new() { Continent = "North America", CountryCode = "USA", Latitude = 30.135, Layouts = [new() { Corners = 20, LengthMeters = 5615, MaxPitSlot = 34, Name = "GP" }, new() { Corners = 19, LengthMeters = 3702, MaxPitSlot = 34, Name = "National" }], Longitude = -97.6341, Name = "Circuit Of The Americas", ShortName = "COTA" },
        new() { Continent = "Europe", CountryCode = "BEL", Latitude = 50.4375, Layouts = [new() { Corners = 19, LengthMeters = 7004, MaxPitSlot = 35, Name = "GP" }], Longitude = 5.9685, Name = "Circuit de Spa Francorchamps", ShortName = "Spa" },
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Latitude = 52.8304, Layouts = [new() { Corners = 12, LengthMeters = 4020, MaxPitSlot = 19, Name = "GP" }, new() { Corners = 10, LengthMeters = 3149, MaxPitSlot = 19, Name = "National" }], Longitude = -1.3749, Name = "Donington Park", ShortName = "Donington" },
        new() { Continent = "Asia", CountryCode = "JPN", Latitude = 35.371667, Layouts = [new() { Corners = 16, LengthMeters = 4549, MaxPitSlot = 34, Name = "GP" }, new() { Corners = 14, LengthMeters = 4526, MaxPitSlot = 34, Name = "GP Short" }], Longitude = 138.926667, Name = "Fuji Speedway", ShortName = "Fuji" },
        new() { Continent = "Europe", CountryCode = "ITA", Latitude = 44.3408, Layouts = [new() { Corners = 22, LengthMeters = 4909, MaxPitSlot = 29, Name = "GP" }], Longitude = 11.7137, Name = "Imola", ShortName = "Imola" },
        new() { Continent = "Africa", CountryCode = "ZAF", Latitude = -25.9976, Layouts = [new() { Corners = 16, LengthMeters = 4522, MaxPitSlot = 20, Name = "GP" }], Longitude = 28.0682, Name = "Kyalami", ShortName = "Kyalami" },
        new() { Continent = "North America", CountryCode = "USA", Latitude = 36.5845, Layouts = [new() { Corners = 11, LengthMeters = 4502, MaxPitSlot = 24, Name = "GP" }], Longitude = -121.7535, Name = "Laguna Seca", ShortName = "Laguna Seca" },
        new() { Continent = "Europe", CountryCode = "ITA", Latitude = 45.621, Layouts = [new() { Corners = 11, LengthMeters = 5793, MaxPitSlot = 30, Name = "GP" }], Longitude = 9.286, Name = "Monza", ShortName = "Monza" },
        new() { Continent = "Australia", CountryCode = "AUS", Latitude = -33.4486, Layouts = [new() { Corners = 23, LengthMeters = 6213, MaxPitSlot = 35, Name = "GP" }], Longitude = 149.5547, Name = "Mount Panorama", ShortName = "Mount Panorama" },
        new() { Continent = "Europe", CountryCode = "DEU", Latitude = 50.3309, Layouts = [new() { Corners = 170, LengthMeters = 25947, MaxPitSlot = 30, Name = "24H" }, new() { Corners = 17, LengthMeters = 5148, MaxPitSlot = 30, Name = "Gp Strecke" }, new() { Corners = 154, LengthMeters = 20832, MaxPitSlot = 5, Name = "Nordschleife" }, new() { Corners = 11, LengthMeters = 3629, MaxPitSlot = 30, Name = "Sprint" }, new() { Corners = 154, LengthMeters = 19300, MaxPitSlot = 50, Name = "Touristenfarten" }], Longitude = 6.9414, Name = "Nurburgring", ShortName = "Nurburgring" },
        new() { Continent = "Europe", CountryCode = "GBR-ENG", Latitude = 53.1768, Layouts = [new() { Corners = 17, LengthMeters = 4333, MaxPitSlot = 16, Name = "International" }, new() { Corners = 7, LengthMeters = 2552, MaxPitSlot = 16, Name = "Fosters" }], Longitude = -2.6168, Name = "Oulton Park", ShortName = "Oulton Park" },
        new() { Continent = "Europe", CountryCode = "FRA", Latitude = 43.2529, Layouts = [new() { Corners = 13, LengthMeters = 5770, MaxPitSlot = 34, Name = "Layout 1A-V2" }, new() { Corners = 15, LengthMeters = 5842, MaxPitSlot = 34, Name = "Layout 1C-V2" }, new() { Corners = 10, LengthMeters = 3793, MaxPitSlot = 34, Name = "Layout 3A" }, new() { Corners = 11, LengthMeters = 3841, MaxPitSlot = 34, Name = "Layout 3C" }], Longitude = 5.7912, Name = "Paul Ricard", ShortName = "Paul Ricard" },
        new() { Continent = "Europe", CountryCode = "AUT", Latitude = 47.2228736, Layouts = [new() { Corners = 10, LengthMeters = 4318, MaxPitSlot = 28, Name = "GP" }, new() { Corners = 5, LengthMeters = 2336, MaxPitSlot = 28, Name = "National" }], Longitude = 14.760198, Name = "Red Bull Ring", ShortName = "Red Bull Ring" },
        new() { Continent = "North America", CountryCode = "USA", Latitude = 34.146667, Layouts = [new() { Corners = 12, LengthMeters = 4088, MaxPitSlot = 28, Name = "GP" }], Longitude = -83.817778, Name = "Road Atlanta", ShortName = "Road Atlanta" },
        new() { Continent = "North America", CountryCode = "USA", Latitude = 27.455, Layouts = [new() { Corners = 17, LengthMeters = 5954, MaxPitSlot = 45, Name = "GP" }], Longitude = -81.35, Name = "Sebring International Raceway", ShortName = "Sebring" },
        new() { Continent = "Asia", CountryCode = "JPN", Latitude = 34.8441, Layouts = [new() { Corners = 18, LengthMeters = 5807, MaxPitSlot = 25, Name = "GP" }, new() { Corners = 7, LengthMeters = 2243, MaxPitSlot = 25, Name = "East" }, new() { Corners = 9, LengthMeters = 3466, MaxPitSlot = 10, Name = "West" }], Longitude = 136.5329, Name = "Suzuka", ShortName = "Suzuka" },
        new() { Continent = "North America", CountryCode = "USA", Latitude = 42.3362, Layouts = [new() { Corners = 13, LengthMeters = 5552, MaxPitSlot = 40, Name = "GP Inner Loop" }, new() { Corners = 7, LengthMeters = 3943, MaxPitSlot = 40, Name = "Short Inner Loop" }, new() { Corners = 11, LengthMeters = 5435, MaxPitSlot = 40, Name = "GP" }, new() { Corners = 7, LengthMeters = 3907, MaxPitSlot = 40, Name = "Short" }], Longitude = -76.9252, Name = "Watkins Glen", ShortName = "Watkins Glen" },
    ];

    public static AceTrackInfoProvider Instance => singletonInstance ??= new AceTrackInfoProvider();

    public AceTrackInfo? FindByName(string name)
    {
        return this.tracks.FirstOrDefault(t => t.Name == name);
    }

    public AceTrackInfo? FindByShortName(string shortName)
    {
        return this.tracks.FirstOrDefault(t => t.ShortName == shortName);
    }

    public ReadOnlyCollection<string> GetContinents()
    {
        return this.tracks.Select(t => t.Continent)
                   .Distinct()
                   .OrderBy(c => c)
                   .ToList()
                   .AsReadOnly();
    }

    public ReadOnlyCollection<AceTrackInfo> GetTrackInfos()
    {
        return this.tracks.AsReadOnly();
    }

    public ReadOnlyCollection<AceTrackInfo> GetTrackInfosForContinent(string continent)
    {
        return this.tracks.Where(t => t.Continent == continent)
                   .ToList()
                   .AsReadOnly();
    }
}

