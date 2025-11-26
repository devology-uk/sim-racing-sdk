using System.Collections.ObjectModel;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Core;

public class AccTrackInfoProvider : IAccTrackInfoProvider
{
    private static AccTrackInfoProvider? singletonInstance;

    private readonly List<AccTrackInfo> tracks =
    [
        new()
        {
            Corners = 16,
            Country = "Spain",
            CountryCode = "ESP",
            FullName = "Circuit de Barcelona-Catalunya",
            Latitude = 41.5695,
            Longitude = 2.2575,
            ShortName = "Barcelona",
            TrackId = "barcelona"
        },
        new()
        {
            Corners = 9,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Brands Hatch Circuit",
            Latitude = 51.3566,
            Longitude = 0.2614,
            ShortName = "Brands Hatch",
            TrackId = "brands_hatch"
        },
        new()
        {
            Corners = 20,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Circuit of the Americas",
            Latitude = 30.135,
            Longitude = -97.6341,
            ShortName = "COTA",
            TrackId = "cota"
        },
        new()
        {
            Corners = 12,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Donington Park",
            Latitude = 52.8304,
            Longitude = -1.3749,
            ShortName = "Donington Park",
            TrackId = "donington"
        },
        new()
        {
            Corners = 14,
            Country = "Hungary",
            CountryCode = "HUN",
            FullName = "Hungaroring",
            Latitude = 47.583,
            Longitude = 19.2498,
            ShortName = "Hungaroring",
            TrackId = "hungaroring"
        },
        new()
        {
            Corners = 22,
            Country = "Italy",
            CountryCode = "ITA",
            FullName = "Autodromo Enzo e Dino Ferrari",
            Latitude = 44.3408,
            Longitude = 11.7137,
            ShortName = "Imola",
            TrackId = "imola"
        },
        new()
        {
            Corners = 14,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Indianapolis Motor Speedway",
            Latitude = 39.7951,
            Longitude = -86.2348,
            ShortName = "Indianapolis",
            TrackId = "indianapolis"
        },
        new()
        {
            Corners = 16,
            Country = "South Africa",
            CountryCode = "ZAF",
            FullName = "Kyalami Grand Prix Circuit",
            Latitude = -25.9976,
            Longitude = 28.0682,
            ShortName = "Kyalami",
            TrackId = "kyalami"
        },
        new()
        {
            Corners = 11,
            Country = "USA",
            CountryCode = "USA",
            FullName = "WeatherTech Raceway Laguna Seca",
            Latitude = 36.5845,
            Longitude = -121.7535,
            ShortName = "Laguna Seca",
            TrackId = "laguna_seca"
        },
        new()
        {
            Corners = 16,
            Country = "Italy",
            CountryCode = "ITA",
            FullName = "Misano World Circuit",
            Latitude = 43.96242,
            Longitude = 12.68381,
            ShortName = "Misano",
            TrackId = "misano"
        },
        new()
        {
            Corners = 11,
            Country = "Italy",
            CountryCode = "ITA",
            FullName = "Monza Circuit",
            Latitude = 45.621,
            Longitude = 9.286,
            ShortName = "Monza",
            TrackId = "monza"
        },
        new()
        {
            Corners = 23,
            Country = "Australia",
            CountryCode = "AUS",
            FullName = "Mount Panorama Circuit",
            Latitude = -33.4486,
            Longitude = 149.5547,
            ShortName = "Mount Panorama",
            TrackId = "mount_panorama"
        },
        new()
        {
            Corners = 17,
            Country = "Germany",
            CountryCode = "DEU",
            FullName = "Nürburgring",
            Latitude = 50.3309,
            Longitude = 6.9414,
            ShortName = "Nürburgring",
            TrackId = "nurburgring"
        },
        new()
        {
            Corners = 17,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Oulton Park",
            Latitude = 53.1768,
            Longitude = -2.6168,
            ShortName = "Oulton Park",
            TrackId = "oulton_park"
        },
        new()
        {
            Corners = 13,
            Country = "France",
            CountryCode = "FRA",
            FullName = "Circuit Paul Ricard",
            Latitude = 43.2529,
            Longitude = 5.7912,
            ShortName = "Paul Ricard",
            TrackId = "paul_ricard"
        },
        new()
        {
            Corners = 18,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Silverstone",
            Latitude = 52.071,
            Longitude = -1.0147,
            ShortName = "Silverstone",
            TrackId = "silverstone"
        },
        new()
        {
            Corners = 12,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Snetterton Circuit",
            Latitude = 52.4648,
            Longitude = 0.9473,
            ShortName = "Snetterton",
            TrackId = "snetterton"
        },
        new()
        {
            Corners = 19,
            Country = "Belgium",
            CountryCode = "BEL",
            FullName = "Circuit de Spa-Francorchamps",
            Latitude = 50.4375,
            Longitude = 5.9685,
            ShortName = "Spa-Francorchamps",
            TrackId = "spa"
        },
        new()
        {
            Corners = 18,
            Country = "Japan",
            CountryCode = "JPN",
            FullName = "Suzuka Circuit",
            Latitude = 34.8441,
            Longitude = 136.5329,
            ShortName = "Suzuka",
            TrackId = "suzuka"
        },
        new()
        {
            Corners = 11,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Watkins Glen International",
            Latitude = 42.3362,
            Longitude = -76.9252,
            ShortName = "Watkins Glen",
            TrackId = "watkins_glen"
        },
        new()
        {
            Corners = 14,
            Country = "Netherlands",
            CountryCode = "NLD",
            FullName = "Circuit Zandvoort",
            Latitude = 52.3881,
            Longitude = 4.545,
            ShortName = "Zandvoort",
            TrackId = "zandvoort"
        },
        new()
        {
            Corners = 10,
            Country = "Belgium",
            CountryCode = "BEL",
            FullName = "Circuit Zolder",
            Latitude = 50.9905,
            Longitude = 5.258,
            ShortName = "Zolder",
            TrackId = "zolder"
        },
        new()
        {
            Corners = 14,
            Country = "Spain",
            CountryCode = "ESP",
            FullName = "Circuit Ricardo Tormo Valencia",
            Latitude = 39.48562,
            Longitude = -0.63056,
            ShortName = "Valencia",
            TrackId = "valencia"
        },
        new()
        {
            Corners = 10,
            Country = "Austria",
            CountryCode = "AUT",
            FullName = "Red Bull Ring",
            Latitude = 47.2228736,
            Longitude = 14.760198,
            ShortName = "Red Bull Ring",
            TrackId = "red_bull_ring"
        },
        new()
        {
            Corners = 170,
            Country = "Germany",
            CountryCode = "DEU",
            FullName = "24H Nürburgring",
            Latitude = 50.3576,
            Longitude = 6.955,
            ShortName = "24H Nürb",
            TrackId = "nurburgring_24h"
        }
    ];
    public static AccTrackInfoProvider Instance => singletonInstance ??= new AccTrackInfoProvider();

    public AccTrackInfo? FindByFullName(string fullName)
    {
        return this.tracks.FirstOrDefault(
            t => t.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase));
    }

    public AccTrackInfo? FindByTrackId(string trackId)
    {
        return this.tracks.FirstOrDefault(t => t.TrackId == trackId);
    }

    public string GetNameByTrackId(string trackId)
    {
        var track = this.FindByTrackId(trackId);
        return track?.ShortName ?? string.Empty;
    }

    public ReadOnlyCollection<AccTrackInfo> GetTrackInfos()
    {
        return this.tracks.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetTrackNames()
    {
        return this.tracks.Select(t => t.FullName)
                   .ToList()
                   .AsReadOnly();
    }
}