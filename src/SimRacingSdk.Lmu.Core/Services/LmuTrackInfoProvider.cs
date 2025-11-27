using System.Collections.ObjectModel;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Services;

public class LmuTrackInfoProvider : ILmuTrackInfoProvider
{
    private static LmuTrackInfoProvider? singletonInstance;

    private readonly IList<LmuTrackInfo> tracks =
    [
        new()
        {
            Name = "Algarve International Circuit",
            ShortName = "Portimao",
            Country = "Portugal",
            CountryCode = "PRT",
            Latitude = 37.231944,
            Longitude = -8.631944,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 4553,
                    Corners = 15
                }
            }
        },
        new()
        {
            Name = "Autodromo Enzo E Dino Ferrari",
            ShortName = "Imola",
            Country = "Italy",
            CountryCode = "ITA",
            Latitude = 44.341111,
            Longitude = 11.713333,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 4909,
                    Corners = 20
                }
            }
        },
        new()
        {
            Name = "Autodromo Nazionale Monza",
            ShortName = "Monza",
            Country = "Italy",
            CountryCode = "ITA",
            Latitude = 45.620556,
            Longitude = 9.289444,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 5793,
                    Corners = 11
                },
                new()
                {
                    Name = "Curva Grande Circuit",
                    LengthM = 5793,
                    Corners = 10
                }
            }
        },
        new()
        {
            Name = "Autodromo Jose Carlos Pace",
            ShortName = "Interlagos",
            Country = "Brazil",
            CountryCode = "BRA",
            Latitude = -23.701111,
            Longitude = -46.697222,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 4309,
                    Corners = 15
                }
            }
        },
        new()
        {
            Name = "Bahrain International Circuit",
            ShortName = "Sakhir",
            Country = "Bahrain",
            CountryCode = "BHR",
            Latitude = 26.0325,
            Longitude = 50.510556,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 5412,
                    Corners = 15
                },
                new()
                {
                    Name = "Endurance Circuit",
                    LengthM = 6299,
                    Corners = 24
                },
                new()
                {
                    Name = "Outer Circuit",
                    LengthM = 3543,
                    Corners = 11
                },
                new()
                {
                    Name = "Paddock Circuit",
                    LengthM = 3705,
                    Corners = 10
                }
            }
        },
        new()
        {
            Name = "Circuit de la Sarthe",
            ShortName = "Le Mans",
            Country = "France",
            CountryCode = "FRA",
            Latitude = 47.933333,
            Longitude = 0.233333,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 13626,
                    Corners = 38
                },
                new()
                {
                    Name = "Mulsanne",
                    LengthM = 13530,
                    Corners = 34
                }
            }
        },
        new()
        {
            Name = "Circuit de Spa-Francorchamps",
            ShortName = "Spa",
            Country = "Belgium",
            CountryCode = "BEL",
            Latitude = 50.437222,
            Longitude = 5.971389,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 7004,
                    Corners = 20
                },
                new()
                {
                    Name = "Endurance Circuit",
                    LengthM = 7004,
                    Corners = 20
                }
            }
        },
        new()
        {
            Name = "Circuit of the Americas",
            ShortName = "COTA",
            Country = "USA",
            CountryCode = "USA",
            Latitude = 30.132778,
            Longitude = -97.641111,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 5513,
                    Corners = 20
                },
                new()
                {
                    Name = "National Circuit",
                    LengthM = 3702,
                    Corners = 17
                }
            }
        },
        new()
        {
            Name = "Fuji Speedway",
            ShortName = "Fuji",
            Country = "Japan",
            CountryCode = "JPN",
            Latitude = 35.371667,
            Longitude = 138.926667,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 4563,
                    Corners = 16
                },
                new()
                {
                    Name = "Classic Circuit",
                    LengthM = 4526,
                    Corners = 14
                }
            }
        },
        new()
        {
            Name = "Lusail International Circuit",
            ShortName = "Lusail",
            Country = "Qatar",
            CountryCode = "QAT",
            Latitude = 25.49,
            Longitude = 51.454167,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 5400,
                    Corners = 16
                },
                new()
                {
                    Name = "Short Circuit",
                    LengthM = 3701,
                    Corners = 11
                }
            }
        },
        new()
        {
            Name = "Sebring International Raceway",
            ShortName = "Sebring",
            Country = "USA",
            CountryCode = "USA",
            Latitude = 27.455,
            Longitude = -81.35,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 6019,
                    Corners = 17
                },
                new()
                {
                    Name = "School Circuit",
                    LengthM = 3219,
                    Corners = 7
                }
            }
        },
        new()
        {
            Name = "Silverstone International",
            ShortName = "Silverstone",
            Country = "England",
            CountryCode = "GBR-ENG",
            Latitude = 52.0680505,
            Longitude = -1.0292908,
            Layouts = new List<LmuTrackLayoutInfo>()
            {
                new()
                {
                    Name = "Default",
                    LengthM = 5891,
                    Corners = 18
                }
            }
        }
    ];

    public static LmuTrackInfoProvider Instance => singletonInstance ??= new LmuTrackInfoProvider();

    public LmuTrackInfo? FindTrackByShortName(string shortName)
    {
        return this.tracks.FirstOrDefault(t => t.ShortName == shortName);
    }

    public LmuTrackInfo? GeTrackInfoByVenue(string venue)
    {
        return this.tracks.FirstOrDefault(t => t.Name.Equals(venue,
                                              StringComparison.InvariantCultureIgnoreCase));
    }

    public IReadOnlyCollection<LmuTrackInfo> GetTrackInfos()
    {
        return this.tracks.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetTrackNames()
    {
        return this.tracks.Select(t => t.ShortName)
                   .ToList()
                   .AsReadOnly();
    }
}