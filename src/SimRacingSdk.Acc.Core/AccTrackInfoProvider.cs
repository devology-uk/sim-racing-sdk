using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Core;

public class AccTrackInfoProvider
{
    private static AccTrackInfoProvider? singletonInstance;
    public static AccTrackInfoProvider Instance => singletonInstance ??= new AccTrackInfoProvider();

    public static List<AccTrackInfo> Tracks { get; } =
    [
        new()
        {
            Corners = 16,
            CountryTag = "ESP",
            Latitude = 41.5695,
            Longitude = 2.2575,
            Name = "Barcelona",
            TrackId = "barcelona"
        },
        new()
        {
            Corners = 9,
            CountryTag = "GBR-ENG",
            Latitude = 51.3566,
            Longitude = 0.2614,
            Name = "Brands Hatch",
            TrackId = "brands_hatch"
        },
        new()
        {
            Corners = 20,
            CountryTag = "USA",
            Latitude = 30.135,
            Longitude = -97.6341,
            Name = "Circuit of the Americas",
            TrackId = "cota"
        },
        new()
        {
            Corners = 12,
            CountryTag = "GBR-ENG",
            Latitude = 52.8304,
            Longitude = -1.3749,
            Name = "Donington Park",
            TrackId = "donington"
        },
        new()
        {
            Corners = 14,
            CountryTag = "HUN",
            Latitude = 47.583,
            Longitude = 19.2498,
            Name = "Hungaroring",
            TrackId = "hungaroring"
        },
        new()
        {
            Corners = 22,
            CountryTag = "ITA",
            Latitude = 44.3408,
            Longitude = 11.7137,
            Name = "Imola",
            TrackId = "imola"
        },
        new()
        {
            Corners = 14,
            CountryTag = "USA",
            Latitude = 39.7951,
            Longitude = -86.2348,
            Name = "Indianapolis",
            TrackId = "indianapolis"
        },
        new()
        {
            Corners = 16,
            CountryTag = "ZAF",
            Latitude = -25.9976,
            Longitude = 28.0682,
            Name = "Kyalami",
            TrackId = "kyalami"
        },
        new()
        {
            Corners = 11,
            CountryTag = "USA",
            Latitude = 36.5845,
            Longitude = -121.7535,
            Name = "Laguna Seca",
            TrackId = "laguna_seca"
        },
        new()
        {
            Corners = 16,
            CountryTag = "ITA",
            Latitude = 43.96242,
            Longitude = 12.68381,
            Name = "Misano",
            TrackId = "misano"
        },
        new()
        {
            Corners = 11,
            CountryTag = "ITA",
            Latitude = 45.621,
            Longitude = 9.286,
            Name = "Monza",
            TrackId = "monza"
        },
        new()
        {
            Corners = 23,
            CountryTag = "AUT",
            Latitude = -33.4486,
            Longitude = 149.5547,
            Name = "Mount Panorama",
            TrackId = "mount_panorama"
        },
        new()
        {
            Corners = 17,
            CountryTag = "DEU",
            Latitude = 50.3309,
            Longitude = 6.9414,
            Name = "Nurburgring",
            TrackId = "nurburgring"
        },
        new()
        {
            Corners = 17,
            CountryTag = "GBR-ENG",
            Latitude = 53.1768,
            Longitude = -2.6168,
            Name = "Oulton Park",
            TrackId = "oulton_park"
        },
        new()
        {
            Corners = 13,
            CountryTag = "FRA",
            Latitude = 43.2529,
            Longitude = 5.7912,
            Name = "Paul Ricard",
            TrackId = "paul_ricard"
        },
        new()
        {
            Corners = 18,
            CountryTag = "GBR-ENG",
            Latitude = 52.071,
            Longitude = -1.0147,
            Name = "Silverstone",
            TrackId = "silverstone"
        },
        new()
        {
            Corners = 12,
            CountryTag = "GBR-ENG",
            Latitude = 52.4648,
            Longitude = 0.9473,
            Name = "Snetterton",
            TrackId = "snetterton"
        },
        new()
        {
            Corners = 19,
            CountryTag = "BEL",
            Latitude = 50.4375,
            Longitude = 5.9685,
            Name = "Spa Francorchamps",
            TrackId = "spa"
        },
        new()
        {
            Corners = 18,
            CountryTag = "JPN",
            Latitude = 34.8441,
            Longitude = 136.5329,
            Name = "Suzuka",
            TrackId = "suzuka"
        },
        new()
        {
            Corners = 11,
            CountryTag = "USA",
            Latitude = 42.3362,
            Longitude = -76.9252,
            Name = "Watkins Glen",
            TrackId = "watkins_glen"
        },
        new()
        {
            Corners = 14,
            CountryTag = "NLD",
            Latitude = 52.3881,
            Longitude = 4.545,
            Name = "Zandvoort",
            TrackId = "zandvoort"
        },
        new()
        {
            Corners = 10,
            CountryTag = "BEL",
            Latitude = 50.9905,
            Longitude = 5.258,
            Name = "Zolder",
            TrackId = "zolder"
        },
        new()
        {
            Corners = 14,
            CountryTag = "ESP",
            Latitude = 39.48562,
            Longitude = -0.63056,
            Name = "Valencia",
            TrackId = "valencia"
        },
        new()
        {
            Corners = 10,
            CountryTag = "AUT",
            Latitude = 47.2228736,
            Longitude = 14.760198,
            Name = "Red Bull Ring",
            TrackId = "red_bull_ring"
        },
        new()
        {
            Corners = 170,
            CountryTag = "DEU",
            Latitude = 50.3576,
            Longitude = 6.955,
            Name = "24H Nurburgring",
            TrackId = "nurburgring_24h"
        }
    ];

    public static AccTrackInfo? FindByTrackId(string trackId)
    {
        return Tracks.FirstOrDefault(t => t.TrackId == trackId);
    }

    public static string GetNameByTrackId(string trackId)
    {
        var track = FindByTrackId(trackId);
        return track?.Name ?? string.Empty;
    }
}