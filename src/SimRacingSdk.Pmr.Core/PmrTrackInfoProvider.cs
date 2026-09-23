using System.Collections.ObjectModel;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Core;

public class PmrTrackInfoProvider : IPmrTrackInfoProvider
{
    private static PmrTrackInfoProvider? singletonInstance;

    private readonly List<PmrTrackInfo> tracks =
    [
        new()
        {
            AltitudeMetersAmsl = 162, Continent = "Europe", Country = "Italy", CountryCode = "ITA", GameId = "ID_brianza", Latitude = 45.62056, Longitude = 9.28111, Name = "Brianza",
            Layouts = [new() { GridSize = 28, LengthMeters = 5793, Name = "Grand Prix", Turns = 7 }]
        },
        new()
        {
            AltitudeMetersAmsl = 334, Continent = "Americas", Country = "Canada", CountryCode = "CAN", GameId = "ID_CanadianTireMotorsportPark", Latitude = 44.05889, Longitude = -78.67722, Name = "Canadian Tire Motorsport Park",
            Layouts = [new() { GridSize = 32, LengthMeters = 4102, Name = "Grand Prix", Turns = 6 }]
        },
        new()
        {
            AltitudeMetersAmsl = 420, Continent = "Europe", Country = "Belgium", CountryCode = "BEL", GameId = "ID_SpaFrancorchamps", Latitude = 50.43722, Longitude = 5.97194, Name = "Circuit de Spa-Francorchamps",
            Layouts = [new() { GridSize = 32, LengthMeters = 7004, Name = "Grand Prix", Turns = 14 }]
        },
        new()
        {
            AltitudeMetersAmsl = 8, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_orlando", Latitude = 29.1852, Longitude = -81.0705, Name = "Daytona",
            Layouts = [new() { GridSize = 32, LengthMeters = 5729, Name = "Road Course", Turns = 12 }]
        },
        new()
        {
            AltitudeMetersAmsl = 85, Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", GameId = "ID_derby", Latitude = 52.83083, Longitude = -1.38667, Name = "Derby",
            Layouts =
            [
                new() { GridSize = 30, LengthMeters = 4020, Name = "Grand Prix", Turns = 12 },
                new() { GridSize = 30, LengthMeters = 3149, Name = "National", Turns = 9 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 1532, Continent = "Rest of World", Country = "South Africa", CountryCode = "ZAF", GameId = "ID_Kyalami", Latitude = -25.99472, Longitude = 28.08083, Name = "Kyalami",
            Layouts = [new() { GridSize = 32, LengthMeters = 4580, Name = "Grand Prix", Turns = 12 }]
        },
        new()
        {
            AltitudeMetersAmsl = 420, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_lexington", Latitude = 40.69417, Longitude = -82.52611, Name = "Lexington",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 3862, Name = "Chicane", Turns = 14 },
                new() { GridSize = 32, LengthMeters = 3621, Name = "No Chicane", Turns = 12 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 190, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_LimeRockPark", Latitude = 41.92722, Longitude = -73.41472, Name = "Lime Rock Park",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 2419, Name = "Chicanes", Turns = 8 },
                new() { GridSize = 32, LengthMeters = 2462, Name = "Road Course", Turns = 6 },
                new() { GridSize = 32, LengthMeters = 2372, Name = "Sports Car", Turns = 7 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 283, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_braselton", Latitude = 34.14667, Longitude = -83.81778, Name = "Michelin Raceway Road Atlanta",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 4088, Name = "Grand Prix", Turns = 12 },
                new() { GridSize = 32, LengthMeters = 2864, Name = "Short Course", Turns = 9 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 770, Continent = "Rest of World", Country = "Australia", CountryCode = "AUS", GameId = "ID_blue_mount", Latitude = -33.43789, Longitude = 149.57065, Name = "Mount Panorama Circuit",
            Layouts = [new() { GridSize = 32, LengthMeters = 6213, Name = "Grand Prix", Turns = 23 }]
        },
        new()
        {
            AltitudeMetersAmsl = 153, Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", GameId = "ID_northampton", Latitude = 52.073265, Longitude = -1.01452, Name = "Northampton",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 5890, Name = "Grand Prix", Turns = 15 },
                new() { GridSize = 32, LengthMeters = 2979, Name = "International", Turns = 7 },
                new() { GridSize = 32, LengthMeters = 5890, Name = "Original Pits", Turns = 15 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 330, Continent = "Europe", Country = "Germany", CountryCode = "DEU", GameId = "ID_NurburgringGP", Latitude = 50.33444, Longitude = 6.94278, Name = "Nürburgring",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 5148, Name = "Grand Prix", Turns = 16 },
                new() { GridSize = 32, LengthMeters = 3629, Name = "Sprint", Turns = 11 }
            ]
        },
        new()
        {
            // A genuinely separate track from "Nürburgring" above, not another layout of it - see
            // TrackInfo.cs's comment on the DataManager side for why.
            AltitudeMetersAmsl = 472, Continent = "Europe", Country = "Germany", CountryCode = "DEU", GameId = "ID_Nordschleife", Latitude = 50.33444, Longitude = 6.94278, Name = "Nürburgring Nordschleife",
            Layouts = [new() { GridSize = 20, LengthMeters = 20832, Name = "Nordschleife", Turns = 73 }]
        },
        new()
        {
            AltitudeMetersAmsl = 472, Continent = "Europe", Country = "Germany", CountryCode = "DEU", GameId = "ID_Nordschleife_full", Latitude = 50.33444, Longitude = 6.94278, Name = "Nürburgring Nordschleife 24hrs",
            Layouts = [new() { GridSize = 32, LengthMeters = 25378, Name = "Nordschleife 24hrs", Turns = 82 }]
        },
        new()
        {
            AltitudeMetersAmsl = 307, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_milwaukee", Latitude = 43.7975, Longitude = -87.99389, Name = "Rocky Knoll Raceway",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 6502, Name = "Chicane", Turns = 15 },
                new() { GridSize = 32, LengthMeters = 6515, Name = "Grand Prix", Turns = 14 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 50, Continent = "Europe", Country = "Italy", CountryCode = "ITA", GameId = "ID_san_marino", Latitude = 44.34306, Longitude = 11.71389, Name = "San Marino",
            Layouts = [new() { GridSize = 32, LengthMeters = 4909, Name = "Grand Prix", Turns = 19 }]
        },
        new()
        {
            AltitudeMetersAmsl = 20, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_jackson", Latitude = 27.45146, Longitude = -81.36872, Name = "Sebring",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 5954, Name = "Grand Prix", Turns = 17 },
                new() { GridSize = 30, LengthMeters = 2736, Name = "Johnson Club Circuit", Turns = 13 },
                new() { GridSize = 32, LengthMeters = 3219, Name = "School Circuit", Turns = 9 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 677, Continent = "Europe", Country = "Austria", CountryCode = "AUT", GameId = "ID_spielberg", Latitude = 47.21972, Longitude = 14.76472, Name = "Spielberg",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 4318, Name = "Grand Prix", Turns = 7 },
                new() { GridSize = 32, LengthMeters = 2336, Name = "National", Turns = 5 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 802, Continent = "Americas", Country = "Brazil", CountryCode = "BRA", GameId = "ID_sao_paulo", Latitude = -23.70361, Longitude = -46.69722, Name = "São Paulo",
            Layouts = [new() { GridSize = 32, LengthMeters = 4309, Name = "Grand Prix", Turns = 15 }]
        },
        new()
        {
            AltitudeMetersAmsl = 94, Continent = "Asia", Country = "Japan", CountryCode = "JPN", GameId = "ID_takimiya", Latitude = 34.8417, Longitude = 136.5389, Name = "Takimaya",
            Layouts =
            [
                new() { GridSize = 32, LengthMeters = 3747, Name = "Grand Prix", Turns = 13 },
                new() { GridSize = 32, LengthMeters = 1956, Name = "Short", Turns = 7 }
            ]
        },
        new()
        {
            AltitudeMetersAmsl = 52, Continent = "Europe", Country = "Belgium", CountryCode = "BEL", GameId = "ID_Zolder", Latitude = 50.98972, Longitude = 5.31083, Name = "Zolder",
            Layouts = [new() { GridSize = 31, LengthMeters = 4010, Name = "Grand Prix", Turns = 11 }]
        }
    ];

    public static PmrTrackInfoProvider Instance => singletonInstance ??= new PmrTrackInfoProvider();

    public PmrTrackInfo? FindByGameId(string gameId)
    {
        return this.tracks.FirstOrDefault(t => string.Equals(t.GameId, gameId, StringComparison.OrdinalIgnoreCase));
    }

    public PmrTrackInfo? FindByName(string name)
    {
        return this.tracks.FirstOrDefault(t => t.Name == name);
    }

    public ReadOnlyCollection<string> GetContinents()
    {
        return this.tracks.Select(t => t.Continent)
                           .Distinct()
                           .OrderBy(c => c)
                           .ToList()
                           .AsReadOnly();
    }

    public ReadOnlyCollection<PmrTrackInfo> GetTrackInfos()
    {
        return this.tracks.AsReadOnly();
    }

    public ReadOnlyCollection<PmrTrackInfo> GetTrackInfosForContinent(string continent)
    {
        return this.tracks.Where(t => t.Continent == continent)
                           .OrderBy(t => t.Name)
                           .ToList()
                           .AsReadOnly();
    }
}
