using System.Collections.ObjectModel;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Core;

public class PmrTrackInfoProvider : IPmrTrackInfoProvider
{
    private static PmrTrackInfoProvider? singletonInstance;

    private readonly List<PmrTrackInfo> tracks =
    [
        new() { AltitudeMetresAmsl = 162, City = "Brianza", Continent = "Europe", Country = "Italy", CountryCode = "ITA", Direction = "Clockwise", GridSize = 28, Latitude = 45.62056, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 5.793, Longitude = 9.28111, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 7, TrackFolder = "brianza", TrackId = "ID_brianza", TrackName = "Brianza", TimeZoneUtcOffset = 1, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 334, City = "Bowmanville", Continent = "Americas", Country = "Canada", CountryCode = "CAN", Direction = "Clockwise", GridSize = 32, Latitude = 44.05889, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 4.102, Longitude = -78.67722, MaxOvertimeSeconds = 120, PitSpeedLimitMetresPerSecond = 16.667, Turns = 6, TrackFolder = "Mosport", TrackId = "ID_CanadianTireMotorsportPark", TrackName = "Canadian Tire Motorsport Park", TimeZoneUtcOffset = -5, TrackYear = 2025 },
        new() { AltitudeMetresAmsl = 420, City = "Stavelot", Continent = "Europe", Country = "Belgium", CountryCode = "BEL", Direction = "Clockwise", GridSize = 32, Latitude = 50.43722, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 7.004, Longitude = 5.97194, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 14, TrackFolder = "spa", TrackId = "ID_SpaFrancorchamps", TrackName = "Circuit de Spa-Francorchamps", TimeZoneUtcOffset = 1, TrackYear = 2025 },
        new() { AltitudeMetresAmsl = 8, City = "Daytona Beach", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Anti-Clockwise", GridSize = 32, Latitude = 29.1852, LayoutId = "layout_a", LayoutName = "Road Course", LengthKm = 5.729, Longitude = -81.0705, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 12, TrackFolder = "orlando", TrackId = "ID_orlando", TrackName = "Daytona", TimeZoneUtcOffset = -5, TrackYear = 2025 },
        new() { AltitudeMetresAmsl = 85, City = "Derbyshire", Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", Direction = "Clockwise", GridSize = 30, Latitude = 52.83083, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 4.02, Longitude = -1.38667, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 12, TrackFolder = "derby", TrackId = "ID_derby", TrackName = "Derby", TimeZoneUtcOffset = 0, TrackYear = 2025 },
        new() { AltitudeMetresAmsl = 85, City = "Derbyshire", Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", Direction = "Clockwise", GridSize = 30, Latitude = 52.83083, LayoutId = "layout_b", LayoutName = "National", LengthKm = 3.149, Longitude = -1.38667, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 9, TrackFolder = "derby", TrackId = "ID_derby", TrackName = "Derby", TimeZoneUtcOffset = 0, TrackYear = 2025 },
        new() { AltitudeMetresAmsl = 1532, City = "Midrand", Continent = "Rest of World", Country = "South Africa", CountryCode = "ZAF", Direction = "Clockwise", GridSize = 32, Latitude = -25.99472, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 4.58, Longitude = 28.08083, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 12, TrackFolder = "Kyalami", TrackId = "ID_Kyalami", TrackName = "Kyalami", TimeZoneUtcOffset = 2, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 420, City = "Troy", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Clockwise", GridSize = 32, Latitude = 40.69417, LayoutId = "layout_b", LayoutName = "Chicane", LengthKm = 3.862, Longitude = -82.52611, MaxOvertimeSeconds = 120, PitSpeedLimitMetresPerSecond = 16.667, Turns = 14, TrackFolder = "lexington", TrackId = "ID_lexington", TrackName = "Lexington", TimeZoneUtcOffset = -5, TrackYear = 2023 },
        new() { AltitudeMetresAmsl = 420, City = "Troy", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Clockwise", GridSize = 32, Latitude = 40.69417, LayoutId = "layout_a", LayoutName = "No Chicane", LengthKm = 3.621, Longitude = -82.52611, MaxOvertimeSeconds = 120, PitSpeedLimitMetresPerSecond = 16.667, Turns = 12, TrackFolder = "lexington", TrackId = "ID_lexington", TrackName = "Lexington", TimeZoneUtcOffset = -5, TrackYear = 2023 },
        new() { AltitudeMetresAmsl = 190, City = "Lakeville", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Clockwise", GridSize = 32, Latitude = 41.92722, LayoutId = "layout_b", LayoutName = "Chicanes", LengthKm = 2.419, Longitude = -73.41472, MaxOvertimeSeconds = 90, PitSpeedLimitMetresPerSecond = 16.667, Turns = 8, TrackFolder = "limerock", TrackId = "ID_LimeRockPark", TrackName = "Lime Rock Park", TimeZoneUtcOffset = -5, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 190, City = "Lakeville", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Clockwise", GridSize = 32, Latitude = 41.92722, LayoutId = "layout_a", LayoutName = "Road Course", LengthKm = 2.462, Longitude = -73.41472, MaxOvertimeSeconds = 90, PitSpeedLimitMetresPerSecond = 16.667, Turns = 6, TrackFolder = "limerock", TrackId = "ID_LimeRockPark", TrackName = "Lime Rock Park", TimeZoneUtcOffset = -5, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 190, City = "Lakeville", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Clockwise", GridSize = 32, Latitude = 41.92722, LayoutId = "layout_c", LayoutName = "Sports Car", LengthKm = 2.372, Longitude = -73.41472, MaxOvertimeSeconds = 90, PitSpeedLimitMetresPerSecond = 16.667, Turns = 7, TrackFolder = "limerock", TrackId = "ID_LimeRockPark", TrackName = "Lime Rock Park", TimeZoneUtcOffset = -5, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 770, City = "Bathurst", Continent = "Rest of World", Country = "Australia", CountryCode = "AUS", Direction = "Anti-Clockwise", GridSize = 32, Latitude = -33.43789, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 6.213, Longitude = 149.57065, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 23, TrackFolder = "blue_mount", TrackId = "ID_blue_mount", TrackName = "Mount Panorama Circuit", TimeZoneUtcOffset = 10, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 153, City = "Northamptonshire", Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", Direction = "Clockwise", GridSize = 32, Latitude = 52.073265, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 5.89, Longitude = -1.01452, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 15, TrackFolder = "northampton", TrackId = "ID_northampton", TrackName = "Northampton", TimeZoneUtcOffset = 0, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 153, City = "Northamptonshire", Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", Direction = "Clockwise", GridSize = 32, Latitude = 52.073265, LayoutId = "layout_b", LayoutName = "International", LengthKm = 2.979, Longitude = -1.01452, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 7, TrackFolder = "northampton", TrackId = "ID_northampton", TrackName = "Northampton", TimeZoneUtcOffset = 0, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 153, City = "Northamptonshire", Continent = "Europe", Country = "United Kingdom", CountryCode = "GBR", Direction = "Clockwise", GridSize = 32, Latitude = 52.073265, LayoutId = "layout_c", LayoutName = "Original Pits", LengthKm = 5.89, Longitude = -1.01452, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 15, TrackFolder = "northampton", TrackId = "ID_northampton", TrackName = "Northampton", TimeZoneUtcOffset = 0, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 330, City = "Nürburg", Continent = "Europe", Country = "Germany", CountryCode = "DEU", Direction = "Clockwise", GridSize = 32, Latitude = 50.33444, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 5.148, Longitude = 6.94278, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 16, TrackFolder = "Nurburgring_gp", TrackId = "ID_NurburgringGP", TrackName = "Nürburgring", TimeZoneUtcOffset = 1, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 330, City = "Nürburg", Continent = "Europe", Country = "Germany", CountryCode = "DEU", Direction = "Clockwise", GridSize = 32, Latitude = 50.33444, LayoutId = "layout_b", LayoutName = "Sprint", LengthKm = 3.629, Longitude = 6.94278, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 11, TrackFolder = "Nurburgring_gp", TrackId = "ID_NurburgringGP", TrackName = "Nürburgring", TimeZoneUtcOffset = 1, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 472, City = "Nürburg", Continent = "Europe", Country = "Germany", CountryCode = "DEU", Direction = "Clockwise", GridSize = 20, Latitude = 50.33444, LayoutId = "layout_a", LayoutName = "Nordschleife", LengthKm = 20.832, Longitude = 6.94278, MaxOvertimeSeconds = 540, PitSpeedLimitMetresPerSecond = 8.334, Turns = 73, TrackFolder = "Nurburg_full", TrackId = "ID_Nordschleife", TrackName = "Nürburgring Nordschleife", TimeZoneUtcOffset = 1, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 472, City = "Nürburg", Continent = "Europe", Country = "Germany", CountryCode = "DEU", Direction = "Clockwise", GridSize = 32, Latitude = 50.33444, LayoutId = "full_layout_a", LayoutName = "Nordschleife 24hrs", LengthKm = 25.378, Longitude = 6.94278, MaxOvertimeSeconds = 600, PitSpeedLimitMetresPerSecond = 16.667, Turns = 82, TrackFolder = "Nurburg_full", TrackId = "ID_Nordschleife_full", TrackName = "Nürburgring Nordschleife 24hrs", TimeZoneUtcOffset = 1, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 50, City = "Emilia-Romagna", Continent = "Europe", Country = "Italy", CountryCode = "ITA", Direction = "Anti-Clockwise", GridSize = 32, Latitude = 44.34306, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 4.909, Longitude = 11.71389, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 19, TrackFolder = "san_marino", TrackId = "ID_san_marino", TrackName = "San Marino", TimeZoneUtcOffset = 1, TrackYear = 2024 },
        new() { AltitudeMetresAmsl = 20, City = "Sebring", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Clockwise", GridSize = 32, Latitude = 27.45146, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 5.954, Longitude = -81.36872, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 17, TrackFolder = "jackson", TrackId = "ID_jackson", TrackName = "Sebring", TimeZoneUtcOffset = -5, TrackYear = 2023 },
        new() { AltitudeMetresAmsl = 20, City = "Sebring", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Clockwise", GridSize = 30, Latitude = 27.45146, LayoutId = "layout_b", LayoutName = "Johnson Club Circuit", LengthKm = 2.736, Longitude = -81.36872, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 13, TrackFolder = "jackson", TrackId = "ID_jackson", TrackName = "Sebring", TimeZoneUtcOffset = -5, TrackYear = 2023 },
        new() { AltitudeMetresAmsl = 20, City = "Sebring", Continent = "Americas", Country = "United States", CountryCode = "USA", Direction = "Clockwise", GridSize = 32, Latitude = 27.45146, LayoutId = "layout_c", LayoutName = "School Circuit", LengthKm = 3.219, Longitude = -81.36872, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 9, TrackFolder = "jackson", TrackId = "ID_jackson", TrackName = "Sebring", TimeZoneUtcOffset = -5, TrackYear = 2023 },
        new() { AltitudeMetresAmsl = 677, City = "Spielberg", Continent = "Europe", Country = "Austria", CountryCode = "AUT", Direction = "Clockwise", GridSize = 32, Latitude = 47.21972, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 4.318, Longitude = 14.76472, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 7, TrackFolder = "spielberg", TrackId = "ID_spielberg", TrackName = "Spielberg", TimeZoneUtcOffset = 1, TrackYear = 2025 },
        new() { AltitudeMetresAmsl = 677, City = "Spielberg", Continent = "Europe", Country = "Austria", CountryCode = "AUT", Direction = "Clockwise", GridSize = 32, Latitude = 47.21972, LayoutId = "layout_b", LayoutName = "National", LengthKm = 2.336, Longitude = 14.76472, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 5, TrackFolder = "spielberg", TrackId = "ID_spielberg", TrackName = "Spielberg", TimeZoneUtcOffset = 1, TrackYear = 2025 },
        new() { AltitudeMetresAmsl = 802, City = "São Paulo", Continent = "Americas", Country = "Brazil", CountryCode = "BRA", Direction = "Anti-Clockwise", GridSize = 32, Latitude = -23.70361, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 4.309, Longitude = -46.69722, MaxOvertimeSeconds = 180, PitSpeedLimitMetresPerSecond = 16.667, Turns = 15, TrackFolder = "sao_paulo", TrackId = "ID_sao_paulo", TrackName = "São Paulo", TimeZoneUtcOffset = -3, TrackYear = 2022 },
        new() { AltitudeMetresAmsl = 52, City = "Heusden-Zolder", Continent = "Europe", Country = "Belgium", CountryCode = "BEL", Direction = "Clockwise", GridSize = 31, Latitude = 50.98972, LayoutId = "layout_a", LayoutName = "Grand Prix", LengthKm = 4.01, Longitude = 5.31083, MaxOvertimeSeconds = 120, PitSpeedLimitMetresPerSecond = 16.667, Turns = 11, TrackFolder = "Zolder", TrackId = "ID_Zolder", TrackName = "Zolder", TimeZoneUtcOffset = 1, TrackYear = 2024 },
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
