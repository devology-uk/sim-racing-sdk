using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Core.Models;

namespace SimRacingSdk.Ams2.Core;

public class Ams2TrackInfoProvider : IAms2TrackInfoProvider
{
    private static Ams2TrackInfoProvider? singletonInstance;
    private const string Grade3 = "Grade 3";
    private const string GradeHistoric = "Historic";
    private const string GradeOffRoad = "Off-road";
    private const string GradeRallycross = "Rallycross";
    private const string TrackTypeCircuit = "Circuit";
    private const string TrackTypeRallycross = "Rallycross";
    private const string Grade1 = "Grade 1";
    private const string Grade2 = "Grade 2";
    private const string GradeKart = "Kart";
    private const string TrackTypeKart = "Kart";
    private const string GradeTemporary = "Temporary";

    public static Ams2TrackInfoProvider Instance => singletonInstance ??= new Ams2TrackInfoProvider();

    private readonly List<Ams2TrackInfo> tracks = [
        new()
        {
            AltitudeM = 52,
            Country = "Australia",
            CountryCode = "AUS",
            FullName = "Adelaide Street Circuit",
            Latitude = -34.7072786,
            Longitude = 138.3438002,
            ShortName = "Adelaide",
            Layouts = [
                new Ams2TrackLayoutInfo
                {
                    
                    Corners = 14,
                    Grade = Grade3,
                    LengthM = 3210,
                    MaxGridSize = 39,
                    Name = "Adelaide",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = GradeHistoric,
                    LengthM = 3780,
                    MaxGridSize = 26,
                    Name = "Adelaide Historic",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade3,
                    LengthM = 3210,
                    MaxGridSize = 20,
                    Name = "Adelaide STT",
                    TrackType = TrackTypeCircuit
                },

            ]
        },
        new()
        {
            AltitudeM = 88,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Max Mohr Circuit",
            Latitude = -26.9372386,
            Longitude = -49.3903302,
            ShortName = "Ascurra",
            Layouts = [
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = GradeOffRoad,
                    LengthM = 910,
                    MaxGridSize = 16,
                    Name = "Ascurra Dirt",
                    TrackType = TrackTypeRallycross
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = GradeRallycross,
                    LengthM = 910,
                    MaxGridSize = 16,
                    Name = "Ascurra RX",
                    TrackType = TrackTypeRallycross
                }
            ]
        },
        new()
        {
            AltitudeM = 1,
            Country = "Reiza",
            CountryCode = "RZA",
            FullName = "Azure Circuit",
            Latitude = 0,
            Longitude = 0,
            ShortName = "Azure Circuit",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 19,
                    Grade = Grade1,
                    LengthM = 3330,
                    MaxGridSize = 26,
                    Name = "Azure Circuit",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new()
        {
            AltitudeM = 115,
            Country = "Spain",
            CountryCode = "ESP",
            FullName = "Circuit de Barcelona-Catalunya",
            Latitude = 41.5695,
            Longitude = 2.2575,
            ShortName = "Barcelona",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = GradeRallycross,
                    LengthM = 1120,
                    MaxGridSize = 8,
                    Name = "Circuit de Barcelona-Catalunya RX",
                    TrackType = TrackTypeRallycross
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = Grade1,
                    LengthM = 4670,
                    MaxGridSize = 48,
                    Name = "Circuit de Barcelona-Catalunya GP",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade1,
                    LengthM = 4650,
                    MaxGridSize = 48,
                    Name = "Circuit de Barcelona-Catalunya GP (no chicane)",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = GradeHistoric,
                    LengthM = 4740,
                    MaxGridSize = 32,
                    Name = "Circuit de Barcelona-Catalunya Historic 1991",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade2,
                    LengthM = 2990,
                    MaxGridSize = 48,
                    Name = "Circuit de Barcelona-Catalunya National",
                    TrackType = TrackTypeCircuit
                },
            ]
        },
        new()
        {
            AltitudeM = 780,
            Country = "Australia",
            CountryCode = "AUS",
            FullName = "Mount Panorama Circuit",
            Latitude = -33.4486,
            Longitude = 149.5547,
            ShortName = "Bathurst",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 23,
                    Grade = Grade3,
                    LengthM = 6210,
                    MaxGridSize = 48,
                    Name = "Bathurst 2020",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 20,
                    Grade = GradeHistoric,
                    LengthM = 6170,
                    MaxGridSize = 32,
                    Name = "Bathurst Historic 1983",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new()
        {
            AltitudeM = 177,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Brands Hatch Circuit",
            Latitude = 51.3566,
            Longitude = 0.2614,
            ShortName = "Brands Hatch",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade2,
                    LengthM = 3910,
                    MaxGridSize = 32,
                    Name = "Brands Hatch",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = Grade3,
                    LengthM = 1940,
                    MaxGridSize = 32,
                    Name = "Brands Hatch Indy",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new()
        {
            AltitudeM = 1172,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Brasília BRB",
            Latitude = -15.7728644,
            Longitude = -47.9020767,
            ShortName = "Brasília",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade3,
                    LengthM = 5470,
                    MaxGridSize = 30,
                    Name = "Brasília Full",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = Grade3,
                    LengthM = 2910,
                    MaxGridSize = 30,
                    Name = "Brasília Outer",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new()
        {
            AltitudeM = 10,
            Country = "Argentina",
            CountryCode = "ARG",
            FullName = "Autódromo Óscar y Juan Gálvez",
            Latitude = -34.6978258,
            Longitude = -58.4714564,
            ShortName = "Buenos Aires",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = Grade2,
                    LengthM = 4310,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.6 S",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = Grade2,
                    LengthM = 4250,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.6",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = Grade2,
                    LengthM = 2600,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.7",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade2,
                    LengthM = 3320,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.8",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade2,
                    LengthM = 3330,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.9",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = Grade2,
                    LengthM = 5650,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.12",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade2,
                    LengthM = 5940,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.15",
                    TrackType = TrackTypeCircuit
                },
            ]
        },
        new Ams2TrackInfo
        {
            AltitudeM = 35,
            Country = "Norway",
            CountryCode = "NOR",
            FullName = "Buskerud Kart Track",
            Latitude = 60.2446211,
            Longitude = 7.7015854,
            ShortName = "Buskerud",
            Layouts = [
                new Ams2TrackLayoutInfo
                {
                    Corners = 22,
                    Grade = GradeKart,
                    LengthM = 1530,
                    MaxGridSize = 32,
                    Name = "Buskerud Kart Long",
                    TrackType = TrackTypeKart
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = GradeKart,
                    LengthM = 950,
                    MaxGridSize = 24,
                    Name = "Buskerud Kart Short",
                    TrackType = TrackTypeKart
                }
            ]
        },
        new Ams2TrackInfo
        {
            AltitudeM = 122,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Cadwell Park Circuit",
            Latitude = 53.3096001,
            Longitude = -0.0697802,
            ShortName = "Cadwell Park",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = Grade3,
                    LengthM = 3500,
                    MaxGridSize = 26,
                    Name = "Cadwell Park",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new Ams2TrackInfo
        {
            AltitudeM = 592,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Orlando Moura",
            Latitude = 3.0513405,
            Longitude = -82.0531904,
            ShortName = "Campo Grande",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade3,
                    LengthM = 3430,
                    MaxGridSize = 48,
                    Name = "Campo Grande",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new Ams2TrackInfo
        {
            AltitudeM = 130,
            Country = "Portugal",
            CountryCode = "PRT",
            FullName = "Autódromo Fernanda Pires da Silva",
            Latitude = 38.7491015,
            Longitude = -9.3960739,
            ShortName = "Cascais",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade1,
                    LengthM = 4180,
                    MaxGridSize = 48,
                    Name = "Cascais",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 4150,
                    MaxGridSize = 48,
                    Name = "Cascais Alternate",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = GradeHistoric,
                    LengthM = 4350,
                    MaxGridSize = 30,
                    Name = "Cascais Historic 1988",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new Ams2TrackInfo
        {
            AltitudeM = 750,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Zilmar Beux de Cascavel",
            Latitude = -24.9819606,
            Longitude = -53.3897087,
            ShortName = "Cascavel",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = Grade3,
                    LengthM = 3300,
                    MaxGridSize = 48,
                    Name = "Cascavel",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new Ams2TrackInfo
        {
            AltitudeM = 199,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Grand Prix of Cleveland",
            Latitude = 41.5312313,
            Longitude = -81.6790431,
            ShortName = "Cleveland",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = GradeTemporary,
                    LengthM = 3380,
                    MaxGridSize = 32,
                    Name = "Cleveland GP",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = GradeTemporary,
                    LengthM = 3380,
                    MaxGridSize = 20,
                    Name = "Cleveland STT",
                    TrackType = TrackTypeCircuit
                }
            ]
        }
    ];
}