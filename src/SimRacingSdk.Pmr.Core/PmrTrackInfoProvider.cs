using System.Collections.ObjectModel;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Core;

public class PmrTrackInfoProvider : IPmrTrackInfoProvider
{
    private static PmrTrackInfoProvider? singletonInstance;

    private readonly List<PmrTrackInfo> tracks =
    [
        new() { AltitudeMetersAmsl = 162, Continent = "Europe", Country = "Italy", CountryCode = "ITA", GameId = "ID_brianza", GridSize = 28, Latitude = 45.62056, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 5793, Longitude = 9.28111, Turns = 7, TrackId = "Brianza", TrackName = "Brianza" },
        new() { AltitudeMetersAmsl = 334, Continent = "Americas", Country = "Canada", CountryCode = "CAN", GameId = "ID_CanadianTireMotorsportPark", GridSize = 32, Latitude = 44.05889, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 4102, Longitude = -78.67722, Turns = 6, TrackId = "Canadian_Tire_Motorsport_Park", TrackName = "Canadian Tire Motorsport Park" },
        new() { AltitudeMetersAmsl = 420, Continent = "Europe", Country = "Belgium", CountryCode = "BEL", GameId = "ID_SpaFrancorchamps", GridSize = 32, Latitude = 50.43722, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 7004, Longitude = 5.97194, Turns = 14, TrackId = "Circuit_de_Spa-Francorchamps", TrackName = "Circuit de Spa-Francorchamps" },
        new() { AltitudeMetersAmsl = 8, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_orlando", GridSize = 32, Latitude = 29.1852, LayoutId = "Road_Course", LayoutName = "Road Course", LengthMeters = 5729, Longitude = -81.0705, Turns = 12, TrackId = "Daytona", TrackName = "Daytona" },
        new() { AltitudeMetersAmsl = 85, Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", GameId = "ID_derby", GridSize = 30, Latitude = 52.83083, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 4020, Longitude = -1.38667, Turns = 12, TrackId = "Derby", TrackName = "Derby" },
        new() { AltitudeMetersAmsl = 85, Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", GameId = "ID_derby", GridSize = 30, Latitude = 52.83083, LayoutId = "National", LayoutName = "National", LengthMeters = 3149, Longitude = -1.38667, Turns = 9, TrackId = "Derby", TrackName = "Derby" },
        new() { AltitudeMetersAmsl = 1532, Continent = "Rest of World", Country = "South Africa", CountryCode = "ZAF", GameId = "ID_Kyalami", GridSize = 32, Latitude = -25.99472, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 4580, Longitude = 28.08083, Turns = 12, TrackId = "Kyalami", TrackName = "Kyalami" },
        new() { AltitudeMetersAmsl = 420, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_lexington", GridSize = 32, Latitude = 40.69417, LayoutId = "Chicane", LayoutName = "Chicane", LengthMeters = 3862, Longitude = -82.52611, Turns = 14, TrackId = "Lexington", TrackName = "Lexington" },
        new() { AltitudeMetersAmsl = 420, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_lexington", GridSize = 32, Latitude = 40.69417, LayoutId = "No_Chicane", LayoutName = "No Chicane", LengthMeters = 3621, Longitude = -82.52611, Turns = 12, TrackId = "Lexington", TrackName = "Lexington" },
        new() { AltitudeMetersAmsl = 190, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_LimeRockPark", GridSize = 32, Latitude = 41.92722, LayoutId = "Chicanes", LayoutName = "Chicanes", LengthMeters = 2419, Longitude = -73.41472, Turns = 8, TrackId = "Lime_Rock_Park", TrackName = "Lime Rock Park" },
        new() { AltitudeMetersAmsl = 190, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_LimeRockPark", GridSize = 32, Latitude = 41.92722, LayoutId = "Road_Course", LayoutName = "Road Course", LengthMeters = 2462, Longitude = -73.41472, Turns = 6, TrackId = "Lime_Rock_Park", TrackName = "Lime Rock Park" },
        new() { AltitudeMetersAmsl = 190, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_LimeRockPark", GridSize = 32, Latitude = 41.92722, LayoutId = "Sports_Car", LayoutName = "Sports Car", LengthMeters = 2372, Longitude = -73.41472, Turns = 7, TrackId = "Lime_Rock_Park", TrackName = "Lime Rock Park" },
        new() { AltitudeMetersAmsl = 283, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_braselton", GridSize = 32, Latitude = 34.14667, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 4088, Longitude = -83.81778, Turns = 12, TrackId = "Michelin_Raceway_Road_Atlanta", TrackName = "Michelin Raceway Road Atlanta" },
        new() { AltitudeMetersAmsl = 283, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_braselton", GridSize = 32, Latitude = 34.14667, LayoutId = "Short_Course", LayoutName = "Short Course", LengthMeters = 2864, Longitude = -83.81778, Turns = 9, TrackId = "Michelin_Raceway_Road_Atlanta", TrackName = "Michelin Raceway Road Atlanta" },
        new() { AltitudeMetersAmsl = 770, Continent = "Rest of World", Country = "Australia", CountryCode = "AUS", GameId = "ID_blue_mount", GridSize = 32, Latitude = -33.43789, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 6213, Longitude = 149.57065, Turns = 23, TrackId = "Mount_Panorama_Circuit", TrackName = "Mount Panorama Circuit" },
        new() { AltitudeMetersAmsl = 153, Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", GameId = "ID_northampton", GridSize = 32, Latitude = 52.073265, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 5890, Longitude = -1.01452, Turns = 15, TrackId = "Northampton", TrackName = "Northampton" },
        new() { AltitudeMetersAmsl = 153, Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", GameId = "ID_northampton", GridSize = 32, Latitude = 52.073265, LayoutId = "International", LayoutName = "International", LengthMeters = 2979, Longitude = -1.01452, Turns = 7, TrackId = "Northampton", TrackName = "Northampton" },
        new() { AltitudeMetersAmsl = 153, Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", GameId = "ID_northampton", GridSize = 32, Latitude = 52.073265, LayoutId = "Original_Pits", LayoutName = "Original Pits", LengthMeters = 5890, Longitude = -1.01452, Turns = 15, TrackId = "Northampton", TrackName = "Northampton" },
        new() { AltitudeMetersAmsl = 330, Continent = "Europe", Country = "Germany", CountryCode = "DEU", GameId = "ID_NurburgringGP", GridSize = 32, Latitude = 50.33444, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 5148, Longitude = 6.94278, Turns = 16, TrackId = "Nürburgring", TrackName = "Nürburgring" },
        new() { AltitudeMetersAmsl = 330, Continent = "Europe", Country = "Germany", CountryCode = "DEU", GameId = "ID_NurburgringGP", GridSize = 32, Latitude = 50.33444, LayoutId = "Sprint", LayoutName = "Sprint", LengthMeters = 3629, Longitude = 6.94278, Turns = 11, TrackId = "Nürburgring", TrackName = "Nürburgring" },
        new() { AltitudeMetersAmsl = 472, Continent = "Europe", Country = "Germany", CountryCode = "DEU", GameId = "ID_Nordschleife", GridSize = 20, Latitude = 50.33444, LayoutId = "Nordschleife", LayoutName = "Nordschleife", LengthMeters = 20832, Longitude = 6.94278, Turns = 73, TrackId = "Nürburgring_Nordschleife", TrackName = "Nürburgring Nordschleife" },
        new() { AltitudeMetersAmsl = 472, Continent = "Europe", Country = "Germany", CountryCode = "DEU", GameId = "ID_Nordschleife_full", GridSize = 32, Latitude = 50.33444, LayoutId = "Nordschleife_24hrs", LayoutName = "Nordschleife 24hrs", LengthMeters = 25378, Longitude = 6.94278, Turns = 82, TrackId = "Nürburgring_Nordschleife_24hrs", TrackName = "Nürburgring Nordschleife 24hrs" },
        new() { AltitudeMetersAmsl = 307, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_milwaukee", GridSize = 32, Latitude = 43.7975, LayoutId = "Chicane", LayoutName = "Chicane", LengthMeters = 6502, Longitude = -87.99389, Turns = 15, TrackId = "Rocky_Knoll_Raceway", TrackName = "Rocky Knoll Raceway" },
        new() { AltitudeMetersAmsl = 307, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_milwaukee", GridSize = 32, Latitude = 43.7975, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 6515, Longitude = -87.99389, Turns = 14, TrackId = "Rocky_Knoll_Raceway", TrackName = "Rocky Knoll Raceway" },
        new() { AltitudeMetersAmsl = 50, Continent = "Europe", Country = "Italy", CountryCode = "ITA", GameId = "ID_san_marino", GridSize = 32, Latitude = 44.34306, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 4909, Longitude = 11.71389, Turns = 19, TrackId = "San_Marino", TrackName = "San Marino" },
        new() { AltitudeMetersAmsl = 20, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_jackson", GridSize = 32, Latitude = 27.45146, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 5954, Longitude = -81.36872, Turns = 17, TrackId = "Sebring", TrackName = "Sebring" },
        new() { AltitudeMetersAmsl = 20, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_jackson", GridSize = 30, Latitude = 27.45146, LayoutId = "Johnson_Club_Circuit", LayoutName = "Johnson Club Circuit", LengthMeters = 2736, Longitude = -81.36872, Turns = 13, TrackId = "Sebring", TrackName = "Sebring" },
        new() { AltitudeMetersAmsl = 20, Continent = "Americas", Country = "United States", CountryCode = "USA", GameId = "ID_jackson", GridSize = 32, Latitude = 27.45146, LayoutId = "School_Circuit", LayoutName = "School Circuit", LengthMeters = 3219, Longitude = -81.36872, Turns = 9, TrackId = "Sebring", TrackName = "Sebring" },
        new() { AltitudeMetersAmsl = 677, Continent = "Europe", Country = "Austria", CountryCode = "AUT", GameId = "ID_spielberg", GridSize = 32, Latitude = 47.21972, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 4318, Longitude = 14.76472, Turns = 7, TrackId = "Spielberg", TrackName = "Spielberg" },
        new() { AltitudeMetersAmsl = 677, Continent = "Europe", Country = "Austria", CountryCode = "AUT", GameId = "ID_spielberg", GridSize = 32, Latitude = 47.21972, LayoutId = "National", LayoutName = "National", LengthMeters = 2336, Longitude = 14.76472, Turns = 5, TrackId = "Spielberg", TrackName = "Spielberg" },
        new() { AltitudeMetersAmsl = 802, Continent = "Americas", Country = "Brazil", CountryCode = "BRA", GameId = "ID_sao_paulo", GridSize = 32, Latitude = -23.70361, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 4309, Longitude = -46.69722, Turns = 15, TrackId = "São_Paulo", TrackName = "São Paulo" },
        new() { AltitudeMetersAmsl = 94, Continent = "Asia", Country = "Japan", CountryCode = "JPN", GameId = "ID_takimiya", GridSize = 32, Latitude = 34.8417, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 3747, Longitude = 136.5389, Turns = 13, TrackId = "Takimaya", TrackName = "Takimaya" },
        new() { AltitudeMetersAmsl = 94, Continent = "Asia", Country = "Japan", CountryCode = "JPN", GameId = "ID_takimiya", GridSize = 32, Latitude = 34.8417, LayoutId = "Short", LayoutName = "Short", LengthMeters = 1956, Longitude = 136.5389, Turns = 7, TrackId = "Takimaya", TrackName = "Takimaya" },
        new() { AltitudeMetersAmsl = 52, Continent = "Europe", Country = "Belgium", CountryCode = "BEL", GameId = "ID_Zolder", GridSize = 31, Latitude = 50.98972, LayoutId = "Grand_Prix", LayoutName = "Grand Prix", LengthMeters = 4010, Longitude = 5.31083, Turns = 11, TrackId = "Zolder", TrackName = "Zolder" },
    ];

    public static PmrTrackInfoProvider Instance => singletonInstance ??= new PmrTrackInfoProvider();

    public PmrTrackInfo? FindByTrackAndLayout(string trackName, string layoutId)
    {
        return this.tracks.FirstOrDefault(t => t.TrackName == trackName && t.LayoutId == layoutId);
    }

    public ReadOnlyCollection<string> GetContinents()
    {
        return this.tracks.Select(t => t.Continent)
                           .Distinct()
                           .OrderBy(c => c)
                           .ToList()
                           .AsReadOnly();
    }

    public ReadOnlyCollection<PmrTrackInfo> GetLayoutsForGameId(string gameId)
    {
        return this.tracks.Where(t => string.Equals(t.GameId, gameId, StringComparison.OrdinalIgnoreCase))
                           .ToList()
                           .AsReadOnly();
    }

    public ReadOnlyCollection<PmrTrackInfo> GetLayoutsForTrack(string trackName)
    {
        return this.tracks.Where(t => t.TrackName == trackName)
                           .ToList()
                           .AsReadOnly();
    }

    public ReadOnlyCollection<PmrTrackInfo> GetTrackInfos()
    {
        return this.tracks.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetTrackNames()
    {
        return this.tracks.Select(t => t.TrackName)
                           .Distinct()
                           .OrderBy(n => n)
                           .ToList()
                           .AsReadOnly();
    }

    public ReadOnlyCollection<string> GetTrackNamesForContinent(string continent)
    {
        return this.tracks.Where(t => t.Continent == continent)
                           .Select(t => t.TrackName)
                           .Distinct()
                           .OrderBy(n => n)
                           .ToList()
                           .AsReadOnly();
    }
}
