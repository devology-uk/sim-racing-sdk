using System.Collections.ObjectModel;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Core.Models;

namespace SimRacingSdk.Ams2.Core;

public class Ams2TrackInfoProvider : IAms2TrackInfoProvider
{
    private const string Grade1 = "Grade 1";
    private const string Grade2 = "Grade 2";
    private const string Grade3 = "Grade 3";
    private const string Grade4 = "Grade 4";
    private const string GradeHistoric = "Historic";
    private const string GradeKart = "Kart";
    private const string GradeOffRoad = "Off-road";
    private const string GradeOval = "Oval";
    private const string GradeRallycross = "Rallycross";
    private const string GradeTemporary = "Temporary";
    private const string TrackTypeCircuit = "Circuit";
    private const string TrackTypeKart = "Kart";
    private const string TrackTypeOval = "Oval";
    private const string TrackTypeRallycross = "Rallycross";

    private static Ams2TrackInfoProvider? singletonInstance;

    private readonly List<Ams2TrackInfo> tracks =
    [
        new()
        {
            AltitudeM = 52,
            Country = "Australia",
            CountryCode = "AUS",
            FullName = "Adelaide Street Circuit",
            Latitude = -34.7072786,
            Longitude = 138.3438002,
            ShortName = "Adelaide",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade3,
                    LengthM = 3210,
                    MaxGridSize = 39,
                    Name = "Adelaide",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Adelaide_Modern"
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Adelaide_Modern_STT"
                }
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
            Layouts =
            [
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "azure_circuit_2021"
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
                    TrackType = TrackTypeRallycross,
                    Ams2TrackId = "barcelona_rx"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = Grade1,
                    LengthM = 4670,
                    MaxGridSize = 48,
                    Name = "Circuit de Barcelona-Catalunya GP",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Barcelona_GP"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade1,
                    LengthM = 4650,
                    MaxGridSize = 48,
                    Name = "Circuit de Barcelona-Catalunya GP (no chicane)",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Barcelona_GP_NC"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = GradeHistoric,
                    LengthM = 4740,
                    MaxGridSize = 32,
                    Name = "Circuit de Barcelona-Catalunya Historic 1991",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "barcelona_1991"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade2,
                    LengthM = 2990,
                    MaxGridSize = 48,
                    Name = "Circuit de Barcelona-Catalunya National",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Barcelona_NAT_NC"
                }
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "bathurst_2020"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 20,
                    Grade = GradeHistoric,
                    LengthM = 6170,
                    MaxGridSize = 32,
                    Name = "Bathurst Historic 1983",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "bathurst_1983"
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "BrandsHatch_GP"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = Grade3,
                    LengthM = 1940,
                    MaxGridSize = 32,
                    Name = "Brands Hatch Indy",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "BrandsHatch_Indy"
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Brasilia"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = Grade3,
                    LengthM = 2910,
                    MaxGridSize = 30,
                    Name = "Brasília Outer",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Brasilia_outer"
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Buenos_Aries_6_T"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = Grade2,
                    LengthM = 4250,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.6",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Buenos_Aries_6"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = Grade2,
                    LengthM = 2600,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.7",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Buenos_Aries_7"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade2,
                    LengthM = 3320,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.8",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Buenos_Aries_8"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade2,
                    LengthM = 3330,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.9",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Buenos_Aries_9"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = Grade2,
                    LengthM = 5650,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.12",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Buenos_Aries_12"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade2,
                    LengthM = 5940,
                    MaxGridSize = 32,
                    Name = "Buenos Aires Circuito No.15",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Buenos_Aries_15"
                }
            ]
        },
        new()
        {
            AltitudeM = 35,
            Country = "Norway",
            CountryCode = "NOR",
            FullName = "Buskerud Kart Track",
            Latitude = 60.2446211,
            Longitude = 7.7015854,
            ShortName = "Buskerud",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 22,
                    Grade = GradeKart,
                    LengthM = 1530,
                    MaxGridSize = 32,
                    Name = "Buskerud Kart Long",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Buskerud_Long"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = GradeKart,
                    LengthM = 950,
                    MaxGridSize = 24,
                    Name = "Buskerud Kart Short",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Buskerud_Short"
                }
            ]
        },
        new()
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "cadwellpark"
                }
            ]
        },
        new()
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "campogrande"
                }
            ]
        },
        new()
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
        new()
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
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "cascavel"
                }
            ]
        },
        new()
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
        },
        new()
        {
            AltitudeM = 881,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Internacional de Curitiba",
            Latitude = -25.444722,
            Longitude = -49.196944,
            ShortName = "Curitiba",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade3,
                    LengthM = 3690,
                    MaxGridSize = 30,
                    Name = "Curitiba",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 6,
                    Grade = Grade3,
                    LengthM = 2600,
                    MaxGridSize = 30,
                    Name = "Curitiba Outer",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Curitiba_outer"
                }
            ]
        },
        new()
        {
            AltitudeM = 672,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Curvelo",
            Latitude = -18.9011312,
            Longitude = -44.557409,
            ShortName = "Curvelo",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 18,
                    Grade = Grade2,
                    LengthM = 4420,
                    MaxGridSize = 48,
                    Name = "Curvelo Long",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Curvelo"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 18,
                    Grade = Grade3,
                    LengthM = 3330,
                    MaxGridSize = 48,
                    Name = "Curvelo Short",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Curvelob"
                }
            ]
        },
        new()
        {
            AltitudeM = 586,
            Country = "Argentina",
            CountryCode = "ARG",
            FullName = "Autódromo Oscar Cabalén",
            Latitude = -31.5777004,
            Longitude = -64.3674171,
            ShortName = "Córdoba",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade3,
                    LengthM = 4060,
                    MaxGridSize = 26,
                    Name = "Córdoba No.4",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Cordoba_GP"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = Grade3,
                    LengthM = 3650,
                    MaxGridSize = 26,
                    Name = "Córdoba TC",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Cordoba"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 2060,
                    MaxGridSize = 26,
                    Name = "Córdoba No.2",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Cordoba_NATL"
                }
            ]
        },
        new()
        {
            AltitudeM = 9,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Daytona International Speedway",
            Latitude = 17.3011292,
            Longitude = -69.8407186,
            ShortName = "Daytona",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade3,
                    LengthM = 5720,
                    MaxGridSize = 48,
                    Name = "Daytona Sports Car Course",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Daytona_Circuit"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade3,
                    LengthM = 5720,
                    MaxGridSize = 48,
                    Name = "Daytona Nascar Road Course",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 3,
                    Grade = GradeOval,
                    LengthM = 4020,
                    MaxGridSize = 48,
                    Name = "Daytona Nascar Tri-Oval",
                    TrackType = TrackTypeOval,
                    Ams2TrackId = "Daytona"
                }
            ]
        },
        new()
        {
            AltitudeM = 96,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Donington Park",
            Latitude = 52.8304,
            Longitude = -1.3749,
            ShortName = "Donington",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade2,
                    LengthM = 4020,
                    MaxGridSize = 38,
                    Name = "Donington GP",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade3,
                    LengthM = 3180,
                    MaxGridSize = 38,
                    Name = "Donington National",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Donington_Nat"
                }
            ]
        },
        new()
        {
            AltitudeM = 377,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Auto Club Speedway",
            Latitude = 34.0867452,
            Longitude = -117.4953334,
            ShortName = "Fontana",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = GradeOval,
                    LengthM = 3210,
                    MaxGridSize = 32,
                    Name = "Auto Club Speedway Oval",
                    TrackType = TrackTypeOval,
                    Ams2TrackId = "Fontana_OVAL"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = Grade2,
                    LengthM = 4500,
                    MaxGridSize = 30,
                    Name = "Auto Club Speedway Sports Car Course",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Fontana_SCC"
                }
            ]
        },
        new()
        {
            AltitudeM = 164,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Foz do Iguaçu",
            Latitude = -25.5182827,
            Longitude = -54.5879,
            ShortName = "Foz",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = GradeRallycross,
                    LengthM = 910,
                    MaxGridSize = 8,
                    Name = "Foz",
                    TrackType = TrackTypeRallycross,
                    Ams2TrackId = "foz_rx"
                }
            ]
        },
        new()
        {
            AltitudeM = 10,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Galeao Airport",
            Latitude = -22.802723,
            Longitude = -43.2501475,
            ShortName = "Galeao Airport",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = GradeTemporary,
                    LengthM = 3200,
                    MaxGridSize = 32,
                    Name = "Galeao Airport",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "galeao_airport"
                }
            ]
        },
        new()
        {
            AltitudeM = 377,
            Country = "USA",
            CountryCode = "USA",
            FullName = "World Wide Technology Raceway",
            Latitude = 38.6507585,
            Longitude = -90.1379298,
            ShortName = "Gateway",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = GradeOval,
                    LengthM = 2010,
                    MaxGridSize = 32,
                    Name = "WWT Raceway Oval",
                    TrackType = TrackTypeOval,
                    Ams2TrackId = "Gateway_OVAL"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade2,
                    LengthM = 2570,
                    MaxGridSize = 32,
                    Name = "WWT Raceway Road Course (Short)",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Gateway_RC1"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade2,
                    LengthM = 3210,
                    MaxGridSize = 32,
                    Name = "WWT Raceway Road Course (Long)",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Gateway_RC2"
                }
            ]
        },
        new()
        {
            AltitudeM = 757,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Internacional Ayrton Senna",
            Latitude = -16.7191167,
            Longitude = -49.1946293,
            ShortName = "Goiânia",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade4,
                    LengthM = 3820,
                    MaxGridSize = 48,
                    Name = "Goiânia",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Goiania"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = Grade3,
                    LengthM = 1910,
                    MaxGridSize = 36,
                    Name = "Goiânia Short",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "GoianiaShort"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = Grade3,
                    LengthM = 2590,
                    MaxGridSize = 48,
                    Name = "Goiânia External",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "GoianiaOuter"
                }
            ]
        },
        new()
        {
            AltitudeM = 828,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Kartódromo Internacional da Granja Viana",
            Latitude = -23.6051739,
            Longitude = -46.8386573,
            ShortName = "Granja Viana",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = GradeKart,
                    LengthM = 980,
                    MaxGridSize = 28,
                    Name = "Copa São Paulo Kart Stage 2",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Granja_VianaCSP2"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = GradeKart,
                    LengthM = 1010,
                    MaxGridSize = 28,
                    Name = "Granja Viana Kart 101",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Granja_VianaKGV101"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = GradeKart,
                    LengthM = 990,
                    MaxGridSize = 28,
                    Name = "Granja Viana Kart 102",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Granja_VianaKGV102"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = GradeKart,
                    LengthM = 800,
                    MaxGridSize = 28,
                    Name = "Granja Viana Kart 121",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Granja_VianaKGV121"
                }
            ]
        },
        new()
        {
            AltitudeM = 474,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Internacional de Guaporé",
            Latitude = -28.8444262,
            Longitude = -51.8542861,
            ShortName = "Guaporé",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = Grade3,
                    LengthM = 3080,
                    MaxGridSize = 40,
                    Name = "Guaporé",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Guapore"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = Grade3,
                    LengthM = 3080,
                    MaxGridSize = 20,
                    Name = "Guaporé STT",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Guapore_STT"
                }
            ]
        },
        new()
        {
            AltitudeM = 102,
            Country = "Germany",
            CountryCode = "DEU",
            FullName = "Hockenheimring Baden-Württemberg",
            Latitude = 49.3271529,
            Longitude = 8.562912,
            ShortName = "Hockenheimring",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = Grade1,
                    LengthM = 4570,
                    MaxGridSize = 48,
                    Name = "Hockenheim",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Hockenheim_GP"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = GradeRallycross,
                    LengthM = 1120,
                    MaxGridSize = 8,
                    Name = "Hockenheim Rallycross",
                    TrackType = TrackTypeRallycross,
                    Ams2TrackId = "Hockenheim_RX"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = Grade2,
                    LengthM = 3690,
                    MaxGridSize = 30,
                    Name = "Hockenheim National",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 2630,
                    MaxGridSize = 30,
                    Name = "Hockenheim Short A",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Hockenheim_ShortA"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade3,
                    LengthM = 2600,
                    MaxGridSize = 30,
                    Name = "Hockenheim Short B",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Hockenheim_ShortB"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = GradeHistoric,
                    LengthM = 6820,
                    MaxGridSize = 30,
                    Name = "Hockenheim Historic 2001",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Hockenheim_2001"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 2630,
                    MaxGridSize = 20,
                    Name = "Hockenheim STT",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Hockenheim_STT"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = GradeHistoric,
                    LengthM = 6790,
                    MaxGridSize = 36,
                    Name = "Hockenheim Historic 1988",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Hockenheim_1988"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = GradeHistoric,
                    LengthM = 2630,
                    MaxGridSize = 36,
                    Name = "Hockenheim Historic 1988 Short",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Hockenheim_1988_short"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = GradeHistoric,
                    LengthM = 6780,
                    MaxGridSize = 36,
                    Name = "Hockenheim Historic 1977",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Hockenheim_1977"
                }
            ]
        },
        new()
        {
            AltitudeM = 2190,
            Country = "Ecuador",
            CountryCode = "ECU",
            FullName = "Autódromo Internacional José Tobar",
            Latitude = 0.3802654,
            Longitude = -78.1035038,
            ShortName = "Ibarra",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 18,
                    Grade = Grade4,
                    LengthM = 4300,
                    MaxGridSize = 30,
                    Name = "Autódromo Yahuarcocha",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Ibarra"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 18,
                    Grade = Grade4,
                    LengthM = 4300,
                    MaxGridSize = 30,
                    Name = "Autódromo Yahuarcocha Reverse",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Ibarra_R"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 18,
                    Grade = Grade4,
                    LengthM = 4300,
                    MaxGridSize = 20,
                    Name = "Autódromo Yahuarcocha STT",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Ibarra_STT"
                }
            ]
        },
        new()
        {
            AltitudeM = 47,
            Country = "Italy",
            CountryCode = "ITA",
            FullName = "Autodromo Enzo e Dino Ferrari",
            Latitude = 44.3443939,
            Longitude = 11.7130472,
            ShortName = "Imola",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 19,
                    Grade = Grade1,
                    LengthM = 4900,
                    MaxGridSize = 48,
                    Name = "Imola",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "imola"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 21,
                    Grade = GradeHistoric,
                    LengthM = 4930,
                    MaxGridSize = 26,
                    Name = "Imola Historic 2001",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "imola_01"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 21,
                    Grade = GradeHistoric,
                    LengthM = 5040,
                    MaxGridSize = 26,
                    Name = "Imola Historic 1988",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "imola_88"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = GradeHistoric,
                    LengthM = 5010,
                    MaxGridSize = 26,
                    Name = "Imola Historic 1972",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "imola_72"
                }
            ]
        },
        new()
        {
            AltitudeM = 220,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Indianapolis Motor Speedway",
            Latitude = 39.7903187,
            Longitude = -86.236236,
            ShortName = "Indianapolis",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade1,
                    LengthM = 3920,
                    MaxGridSize = 48,
                    Name = "Indianapolis Motor Speedway Road Course",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Indianapolis_2022_RC"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = GradeOval,
                    LengthM = 4020,
                    MaxGridSize = 48,
                    Name = "Indianapolis Motor Speedway Oval",
                    TrackType = TrackTypeOval,
                    Ams2TrackId = "Indianapolis_2022_Oval"
                }
            ]
        },
        new()
        {
            AltitudeM = 750,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo José Carlos Pace",
            Latitude = -23.7045832,
            Longitude = -46.7018401,
            ShortName = "Interlagos",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade1,
                    LengthM = 4290,
                    MaxGridSize = 48,
                    Name = "Interlagos GP",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Interlagos"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade2,
                    LengthM = 4290,
                    MaxGridSize = 48,
                    Name = "Interlagos Stock Car Brasil",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Interlagos_SCB"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = GradeHistoric,
                    LengthM = 4320,
                    MaxGridSize = 32,
                    Name = "Interlagos Historic 1993",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Interlagos_1993"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = GradeHistoric,
                    LengthM = 4320,
                    MaxGridSize = 32,
                    Name = "Interlagos Historic 1991",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Interlagos_1991"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = GradeHistoric,
                    LengthM = 7920,
                    MaxGridSize = 32,
                    Name = "Interlagos Historic 1976",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Interlagos_Historic"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = GradeHistoric,
                    LengthM = 3230,
                    MaxGridSize = 32,
                    Name = "Interlagos Historic 1978 Outer",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Interlagos_Historic_Outer"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 19,
                    Grade = GradeKart,
                    LengthM = 1120,
                    MaxGridSize = 38,
                    Name = "Interlagos Kart One",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Interlagos_Kart1"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = GradeKart,
                    LengthM = 1120,
                    MaxGridSize = 38,
                    Name = "Interlagos Kart Two",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Interlagos_Kart2"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = GradeKart,
                    LengthM = 690,
                    MaxGridSize = 38,
                    Name = "Interlagos Kart Three",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Interlagos_Kart3"
                }
            ]
        },
        new()
        {
            AltitudeM = 30,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Internacional Nelson Piquet",
            Latitude = -22.975556,
            Longitude = -43.395,
            ShortName = "Jacarepaguá",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = GradeHistoric,
                    LengthM = 4900,
                    MaxGridSize = 40,
                    Name = "Jacarepaguá Historic 2005",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Jacarepagua2005"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = GradeHistoric,
                    LengthM = 5000,
                    MaxGridSize = 40,
                    Name = "Jacarepaguá Historic 1988",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "jacarepagua_historic"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = GradeOval,
                    LengthM = 3000,
                    MaxGridSize = 32,
                    Name = "Jacarepaguá Historic 2005 Oval",
                    TrackType = TrackTypeOval,
                    Ams2TrackId = "JacarepaguaOVAL"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = GradeHistoric,
                    LengthM = 3300,
                    MaxGridSize = 40,
                    Name = "Jacarepaguá Historic 2012 SCB",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "JacarepaguaSCB"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = GradeHistoric,
                    LengthM = 3040,
                    MaxGridSize = 40,
                    Name = "Jacarepaguá Historic 2012 Short",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "JacarepaguaSHORT"
                }
            ]
        },
        new()
        {
            AltitudeM = 38,
            Country = "Spain",
            CountryCode = "ESP",
            FullName = "Circuito de Jerez – Ángel Nieto",
            Latitude = 36.708333,
            Longitude = -6.034167,
            ShortName = "Jerez",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade3,
                    LengthM = 4410,
                    MaxGridSize = 42,
                    Name = "Jerez Moto",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Jerez_Standard"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade1,
                    LengthM = 4420,
                    MaxGridSize = 40,
                    Name = "Jerez Chicane",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Jerez_2019"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = GradeHistoric,
                    LengthM = 4210,
                    MaxGridSize = 32,
                    Name = "Jerez Historic 1988",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "jerez_1988"
                }
            ]
        },
        new()
        {
            AltitudeM = 47,
            Country = "Japan",
            CountryCode = "JAP",
            FullName = "Suzuka International Racing Course Simulation",
            Latitude = 34.8417,
            Longitude = 136.5389,
            ShortName = "Kansai",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 19,
                    Grade = Grade1,
                    LengthM = 5810,
                    MaxGridSize = 48,
                    Name = "Kansai GP",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 3460,
                    MaxGridSize = 26,
                    Name = "Kansai West",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade3,
                    LengthM = 2240,
                    MaxGridSize = 48,
                    Name = "Kansai East",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 19,
                    Grade = Grade2,
                    LengthM = 5820,
                    MaxGridSize = 48,
                    Name = "Kansai Classic",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new()
        {
            AltitudeM = 1532,
            Country = "South Africa",
            CountryCode = "ZAF",
            FullName = "Kyalami Grand Prix Circuit",
            Latitude = -25.998056,
            Longitude = 28.068889,
            ShortName = "Kyalami",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade2,
                    LengthM = 4520,
                    MaxGridSize = 48,
                    Name = "Kyalami",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "kyalami_2019"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = GradeHistoric,
                    LengthM = 4040,
                    MaxGridSize = 26,
                    Name = "Kyalami Historic 1976",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "kyalami_historic"
                }
            ]
        },
        new()
        {
            AltitudeM = 220,
            Country = "USA",
            CountryCode = "USA",
            FullName = "WeatherTech Raceway Laguna Seca",
            Latitude = 36.584167,
            Longitude = -121.753611,
            ShortName = "Laguna Seca",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade2,
                    LengthM = 3600,
                    MaxGridSize = 48,
                    Name = "Laguna Seca 2020",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "laguna_seca_2020"
                }
            ]
        },
        new()
        {
            AltitudeM = 52,
            Country = "France",
            CountryCode = "FRA",
            FullName = "Circuit de la Sarthe",
            Latitude = 47.933333,
            Longitude = 0.233333,
            ShortName = "Le Mans",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 38,
                    Grade = Grade2,
                    LengthM = 13620,
                    MaxGridSize = 60,
                    Name = "Circuit des 24 Heures du Mans",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "le_mans_24h"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 38,
                    Grade = Grade2,
                    LengthM = 13650,
                    MaxGridSize = 48,
                    Name = "Circuit des 24 Heures du Mans Historic 2005",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "le_mans_24h_2005"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade2,
                    LengthM = 4180,
                    MaxGridSize = 32,
                    Name = "Le Mans Circuit Bugatti",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "le_mans_bugatti"
                }
            ]
        },
        new()
        {
            AltitudeM = 566,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Internacional Ayrton Senna",
            Latitude = -23.282222,
            Longitude = -51.167222,
            ShortName = "Londrina",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade3,
                    LengthM = 3020,
                    MaxGridSize = 48,
                    Name = "Londrina Short",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Londrina"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade3,
                    LengthM = 3140,
                    MaxGridSize = 48,
                    Name = "Londrina Long",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Londrina_long"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = GradeKart,
                    LengthM = 1000,
                    MaxGridSize = 28,
                    Name = "Londrina Kart One",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Londrinakart1"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = GradeKart,
                    LengthM = 860,
                    MaxGridSize = 28,
                    Name = "Londrina Kart Two",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Londrinakart2"
                }
            ]
        },
        new()
        {
            AltitudeM = 2,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Long Beach",
            Latitude = 33.766389,
            Longitude = -118.192778,
            ShortName = "Long Beach",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 3160,
                    MaxGridSize = 48,
                    Name = "Long Beach",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 3160,
                    MaxGridSize = 20,
                    Name = "Long Beach STT",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new()
        {
            AltitudeM = 13,
            Country = "Canada",
            CountryCode = "CAN",
            FullName = "Circuit Gilles Villeneuve",
            Latitude = 45.500556,
            Longitude = -73.5225,
            ShortName = "Montreal",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade1,
                    LengthM = 4360,
                    MaxGridSize = 40,
                    Name = "Montreal",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "montrealmodern"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = GradeHistoric,
                    LengthM = 4430,
                    MaxGridSize = 40,
                    Name = "Montreal Historic 1991",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "MontrealHistoric_1991"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = GradeHistoric,
                    LengthM = 4430,
                    MaxGridSize = 40,
                    Name = "Montreal Historic 1988",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "montrealhistoric"
                }
            ]
        },
        new()
        {
            AltitudeM = 180,
            Country = "Italy",
            CountryCode = "ITA",
            FullName = "Autodromo Nazionale Monza",
            Latitude = 45.620556,
            Longitude = 9.289444,
            ShortName = "Monza",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade1,
                    LengthM = 5790,
                    MaxGridSize = 48,
                    Name = "Monza",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Monza_2020"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = Grade3,
                    LengthM = 2400,
                    MaxGridSize = 32,
                    Name = "Monza Junior",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Monza_2020_Junior"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = GradeHistoric,
                    LengthM = 5800,
                    MaxGridSize = 32,
                    Name = "Monza Historic 1991",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Monza_1991"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = GradeHistoric,
                    LengthM = 5750,
                    MaxGridSize = 26,
                    Name = "Monza Historic 1971",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Monza_1971"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = GradeHistoric,
                    LengthM = 2400,
                    MaxGridSize = 26,
                    Name = "Monza Historic 1971 Junior",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Monza_1971_Junior"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = GradeHistoric,
                    LengthM = 10100,
                    MaxGridSize = 26,
                    Name = "Monza Historic 1971 10K",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Monza_1971_10k"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = GradeHistoric,
                    LengthM = 10000,
                    MaxGridSize = 26,
                    Name = "Monza Historic 1971 10K (no chicane)",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Monza_1971_10knc"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 4,
                    Grade = Grade3,
                    LengthM = 2040,
                    MaxGridSize = 20,
                    Name = "Monza STT",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Monza_2020_STT"
                }
            ]
        },
        new()
        {
            AltitudeM = 300,
            Country = "Canada",
            CountryCode = "CAN",
            FullName = "Canadian Tire Motorsport Park",
            Latitude = 44.0512778,
            Longitude = -78.6797009,
            ShortName = "Mosport",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade2,
                    LengthM = 3950,
                    MaxGridSize = 48,
                    Name = "Mosport",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "mosport"
                }
            ]
        },
        new()
        {
            AltitudeM = 397,
            Country = "Germany",
            CountryCode = "DEU",
            FullName = "Nürburgring",
            Latitude = 50.335556,
            Longitude = 6.9475,
            ShortName = "Nürburgring",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 73,
                    Grade = Grade3,
                    LengthM = 20830,
                    MaxGridSize = 28,
                    Name = "Nordschleife 2025",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "nordschleife_2025"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 73,
                    Grade = Grade3,
                    LengthM = 20830,
                    MaxGridSize = 28,
                    Name = "Nordschleife 2020",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = GradeHistoric,
                    LengthM = 2390,
                    MaxGridSize = 32,
                    Name = "Betonschleife Historic 1971",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_1921_Beton"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 88,
                    Grade = Grade3,
                    LengthM = 25370,
                    MaxGridSize = 48,
                    Name = "Nordschleife 24 Hour 2025",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "nordschleife_2025_24hr"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 88,
                    Grade = Grade3,
                    LengthM = 25370,
                    MaxGridSize = 48,
                    Name = "Nordschleife 24 Hour 2020",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "nordschleife_2020_24hr"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 6,
                    Grade = GradeRallycross,
                    LengthM = 1020,
                    MaxGridSize = 8,
                    Name = "Nürburgring RX",
                    TrackType = TrackTypeRallycross,
                    Ams2TrackId = "Nurb_2020_RX"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade1,
                    LengthM = 5140,
                    MaxGridSize = 48,
                    Name = "Nürburgring GP 2025",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_GP_2025"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade1,
                    LengthM = 5140,
                    MaxGridSize = 48,
                    Name = "Nürburgring GP 2020",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_GP_2020"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade1,
                    LengthM = 5130,
                    MaxGridSize = 48,
                    Name = "Nürburgring Veedol 2020",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_GP_2020_Veedol"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 3620,
                    MaxGridSize = 48,
                    Name = "Nürburgring Sprint 2020",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_GP_2020_Sprint"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 3050,
                    MaxGridSize = 30,
                    Name = "Nürburgring Sprint S 2020",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_GP_2020_Sprint_S"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 187,
                    Grade = GradeHistoric,
                    LengthM = 28260,
                    MaxGridSize = 32,
                    Name = "Gesamtstrecke Historic 1971",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_1971_Gesamt"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 160,
                    Grade = GradeHistoric,
                    LengthM = 22830,
                    MaxGridSize = 32,
                    Name = "Nordschleife Historic 1971",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_1971_Nords"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 27,
                    Grade = GradeHistoric,
                    LengthM = 7740,
                    MaxGridSize = 32,
                    Name = "Südschleife Historic 1971",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Nurb_1971_Suds"
                }
            ]
        },
        new()
        {
            AltitudeM = 137,
            Country = "Italy",
            CountryCode = "ITA",
            FullName = "Circuito Internazionale d'Abruzzo",
            Latitude = 42.3056608,
            Longitude = 14.3778113,
            ShortName = "Ortona",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = GradeKart,
                    LengthM = 1500,
                    MaxGridSize = 30,
                    Name = "Ortona Kart One",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Ortona"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 20,
                    Grade = GradeKart,
                    LengthM = 1550,
                    MaxGridSize = 30,
                    Name = "Ortona Kart TWo",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Ortona2"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = GradeKart,
                    LengthM = 1330,
                    MaxGridSize = 30,
                    Name = "Ortona Kart Three",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Ortona3"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = GradeKart,
                    LengthM = 960,
                    MaxGridSize = 30,
                    Name = "Ortona Kart Four",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Ortona4"
                }
            ]
        },
        new()
        {
            AltitudeM = 65,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Oulton Park",
            Latitude = 53.1768,
            Longitude = -2.6168,
            ShortName = "Oulton Park",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = Grade3,
                    LengthM = 4330,
                    MaxGridSize = 28,
                    Name = "Oulton Park International",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "OultonPark"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade4,
                    LengthM = 3630,
                    MaxGridSize = 28,
                    Name = "Oulton Park Island",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "OultonParkIsland"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade4,
                    LengthM = 2660,
                    MaxGridSize = 28,
                    Name = "Oulton Park Fosters",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "OultonParkFosters"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade3,
                    LengthM = 4290,
                    MaxGridSize = 28,
                    Name = "Oulton Park Classic",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "OultonParkClassic"
                }
            ]
        },
        new()
        {
            AltitudeM = 300,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Rod America",
            Latitude = 43.7975,
            Longitude = -87.993889,
            ShortName = "Road America",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade2,
                    LengthM = 6510,
                    MaxGridSize = 48,
                    Name = "Road America",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Road_America"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade2,
                    LengthM = 6510,
                    MaxGridSize = 48,
                    Name = "Road America (Bend)",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Road_America_RCB"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade2,
                    LengthM = 6510,
                    MaxGridSize = 20,
                    Name = "Road America STT",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Road_America_STT"
                }
            ]
        },
        new()
        {
            AltitudeM = 277,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Michelin Raceway Road Atlanta",
            Latitude = 34.146667,
            Longitude = -83.817778,
            ShortName = "Road Atlanta",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade2,
                    LengthM = 4080,
                    MaxGridSize = 48,
                    Name = "Road Atlanta",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade2,
                    LengthM = 4080,
                    MaxGridSize = 48,
                    Name = "Road Atlanta Historic 2005",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "road_atlanta_2005"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade2,
                    LengthM = 2580,
                    MaxGridSize = 48,
                    Name = "Road Atlanta Historic 2005 Short",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "road_atlanta_2005_short"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade2,
                    LengthM = 4100,
                    MaxGridSize = 32,
                    Name = "Road Atlanta Moto",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new()
        {
            AltitudeM = 68,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Circuito Ayrton Senna",
            Latitude = -12.9475,
            Longitude = -38.428056,
            ShortName = "Salvador",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = GradeTemporary,
                    LengthM = 2720,
                    MaxGridSize = 34,
                    Name = "Salvador Street Circuit",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "salvador"
                }
            ]
        },
        new()
        {
            AltitudeM = 120,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Internacional de Santa Cruz do Sul",
            Latitude = -29.8,
            Longitude = -52.436667,
            ShortName = "Santa Cruz",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade4,
                    LengthM = 3320,
                    MaxGridSize = 48,
                    Name = "Santa Cruz do Sul",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "santacruz"
                }
            ]
        },
        new()
        {
            AltitudeM = 18,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Sebring International Raceway",
            Latitude = 27.455,
            Longitude = -81.35,
            ShortName = "Sebring",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = Grade2,
                    LengthM = 5850,
                    MaxGridSize = 48,
                    Name = "Sebring",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade2,
                    LengthM = 3210,
                    MaxGridSize = 32,
                    Name = "Sebring School",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "sebring"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade2,
                    LengthM = 2730,
                    MaxGridSize = 32,
                    Name = "Sebring Club",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade2,
                    LengthM = 3210,
                    MaxGridSize = 20,
                    Name = "Sebring STT",
                    TrackType = TrackTypeCircuit
                }
            ]
        },
        new()
        {
            AltitudeM = 154,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Silverstone Circuit",
            Latitude = 52.071,
            Longitude = -1.0147,
            ShortName = "Silverstone",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 18,
                    Grade = Grade1,
                    LengthM = 5890,
                    MaxGridSize = 48,
                    Name = "Silverstone",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Silverstone_2019"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade2,
                    LengthM = 2970,
                    MaxGridSize = 32,
                    Name = "Silverstone International",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Silverstone_Intl_2019"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = Grade3,
                    LengthM = 2630,
                    MaxGridSize = 36,
                    Name = "Silverstone National",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Silverstone_Natl_2019"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = GradeHistoric,
                    LengthM = 5140,
                    MaxGridSize = 36,
                    Name = "Silverstone Historic 2001",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Silverstone_2001"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = GradeHistoric,
                    LengthM = 3610,
                    MaxGridSize = 36,
                    Name = "Silverstone International Historic 2001",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Silverstone_Intl_2001"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = GradeHistoric,
                    LengthM = 2630,
                    MaxGridSize = 36,
                    Name = "Silverstone National Historic 2001",
                    TrackType = TrackTypeCircuit, 
                        Ams2TrackId = "Silverstone_Natl_2001"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 17,
                    Grade = GradeHistoric,
                    LengthM = 5220,
                    MaxGridSize = 36,
                    Name = "Silverstone Historic 1991",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "silverstone_1991"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = GradeHistoric,
                    LengthM = 4710,
                    MaxGridSize = 36,
                    Name = "Silverstone Historic 1975",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Silverstone_1975"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = GradeHistoric,
                    LengthM = 4710,
                    MaxGridSize = 36,
                    Name = "Silverstone Historic 1975 (no chicane)",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Silverstone_1975nc"
                }
            ]
        },
        new()
        {
            AltitudeM = 43,
            Country = "England",
            CountryCode = "GBR-ENG",
            FullName = "Snetterton Circuit",
            Latitude = 52.4648,
            Longitude = 0.9473,
            ShortName = "Snetterton",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = Grade2,
                    LengthM = 4780,
                    MaxGridSize = 26,
                    Name = "Snetterton 300",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Snetterton2019_300"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = Grade2,
                    LengthM = 3220,
                    MaxGridSize = 26,
                    Name = "Snetterton 200",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Snetterton2019_200"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 6,
                    Grade = Grade4,
                    LengthM = 1580,
                    MaxGridSize = 26,
                    Name = "Snetterton 100",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Snetterton2019_100"
                }
            ]
        },
        new()
        {
            AltitudeM = 394,
            Country = "Belgium",
            CountryCode = "BEL",
            FullName = "Circuit de Spa-Francorchamps",
            Latitude = 50.4375,
            Longitude = 5.9685,
            ShortName = "Spa-Francorchamps",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 20,
                    Grade = Grade1,
                    LengthM = 7000,
                    MaxGridSize = 48,
                    Name = "Spa-Francorchamps 2022",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 20,
                    Grade = Grade1,
                    LengthM = 7000,
                    MaxGridSize = 48,
                    Name = "Spa-Francorchamps 2020",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 5,
                    Grade = GradeRallycross,
                    LengthM = 890,
                    MaxGridSize = 8,
                    Name = "Spa-Francorchamps RX",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Spa-francorchamps_2022_RX"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 23,
                    Grade = GradeHistoric,
                    LengthM = 6940,
                    MaxGridSize = 30,
                    Name = "Spa-Francorchamps Historic 1993",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "spa-francorchamps_1993"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 21,
                    Grade = GradeHistoric,
                    LengthM = 14120,
                    MaxGridSize = 32,
                    Name = "Spa-Francorchamps Historic 1970",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Spa-francorchamps_1970"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 21,
                    Grade = GradeHistoric,
                    LengthM = 14120,
                    MaxGridSize = 32,
                    Name = "Spa-Francorchamps Historic 1970 1000KM",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Spa-francorchamps_1970_NC"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 24,
                    Grade = Grade1,
                    LengthM = 6970,
                    MaxGridSize = 48,
                    Name = "Spa-Francorchamps Historic 2005 Endurance",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "spa-francorchamps_2005_ec"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 24,
                    Grade = Grade1,
                    LengthM = 6970,
                    MaxGridSize = 48,
                    Name = "Spa-Francorchamps Historic 2005",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "spa-francorchamps_2005"
                }
            ]
        },
        new()
        {
            AltitudeM = 700,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Speedland Kart Centre",
            Latitude = -23.5333361,
            Longitude = -46.5863756,
            ShortName = "Speedland",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 29,
                    Grade = GradeKart,
                    LengthM = 960,
                    MaxGridSize = 14,
                    Name = "Speedland Kart 1",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Speedland1"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 30,
                    Grade = GradeKart,
                    LengthM = 980,
                    MaxGridSize = 14,
                    Name = "Speedland Kart 2",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Speedland2"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = GradeKart,
                    LengthM = 430,
                    MaxGridSize = 14,
                    Name = "Speedland Kart 2",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Speedland3"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 12,
                    Grade = GradeKart,
                    LengthM = 580,
                    MaxGridSize = 14,
                    Name = "Speedland Kart 4",
                    TrackType = TrackTypeKart,
                    Ams2TrackId = "Speedland4"
                }
            ]
        },
        new()
        {
            AltitudeM = 660,
            Country = "Austria",
            CountryCode = "AUT",
            FullName = "Red Bull Ring",
            Latitude = 47.2228736,
            Longitude = 14.760198,
            ShortName = "Spielberg",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade1,
                    LengthM = 4310,
                    MaxGridSize = 32,
                    Name = "Spielberg",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Spielberg_GP"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 6,
                    Grade = Grade3,
                    LengthM = 2330,
                    MaxGridSize = 32,
                    Name = "Spielberg Short",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 6,
                    Grade = Grade3,
                    LengthM = 2330,
                    MaxGridSize = 20,
                    Name = "Spielberg STT",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = GradeHistoric,
                    LengthM = 5890,
                    MaxGridSize = 26,
                    Name = "Spielberg Historic 1974",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Spielberg_Vintage"
                }
            ]
        },
        new()
        {
            AltitudeM = 71,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Internacional de Tarumã",
            Latitude = -30.048611,
            Longitude = -51.019167,
            ShortName = "Tarumã",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade4,
                    LengthM = 3010,
                    MaxGridSize = 32,
                    Name = "Tarumã Internacional",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Taruma"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade3,
                    LengthM = 3070,
                    MaxGridSize = 32,
                    Name = "Tarumã Chicane",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "TarumaC"
                }
            ]
        },
        new()
        {
            AltitudeM = 280,
            Country = "Argentina",
            CountryCode = "ARG",
            FullName = "Autódromo Termas de Río Hondo",
            Latitude = -27.505861,
            Longitude = -64.914472,
            ShortName = "Termas de Río Hondo",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade2,
                    LengthM = 4800,
                    MaxGridSize = 48,
                    Name = "Termas de Río Hondo",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "termas_rio_hondo"
                }
            ]
        },
        new()
        {
            AltitudeM = 81,
            Country = "Finland",
            CountryCode = "FIN",
            FullName = "Kouvola Circuit",
            Latitude = 60.8838619,
            Longitude = 26.7913711,
            ShortName = "Tykki",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = GradeRallycross,
                    LengthM = 1010,
                    MaxGridSize = 8,
                    Name = "Tykki RX",
                    TrackType = TrackTypeRallycross
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = GradeOffRoad,
                    LengthM = 750,
                    MaxGridSize = 16,
                    Name = "Tykki Tarmac",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 8,
                    Grade = GradeOffRoad,
                    LengthM = 910,
                    MaxGridSize = 16,
                    Name = "Tykki Dirt 1",
                    TrackType = TrackTypeRallycross,
                    Ams2TrackId = "Tykki_Dirt1"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = GradeOffRoad,
                    LengthM = 1680,
                    MaxGridSize = 16,
                    Name = "Tykki Dirt 2",
                    TrackType = TrackTypeRallycross,
                    Ams2TrackId = "Tykki_Dirt2"
                }
            ]
        },
        new()
        {
            AltitudeM = 750,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Autódromo Velo Città",
            Latitude = -22.288889,
            Longitude = -46.848333,
            ShortName = "Velo Città",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade3,
                    LengthM = 3320,
                    MaxGridSize = 34,
                    Name = "Velo Città",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Velocitta"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 16,
                    Grade = Grade3,
                    LengthM = 3360,
                    MaxGridSize = 34,
                    Name = "Velo Città Track Day",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "VelocittaTD"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = Grade3,
                    LengthM = 1720,
                    MaxGridSize = 34,
                    Name = "Velo Città Club Day",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "VelocittaCD"
                }
            ]
        },
        new()
        {
            AltitudeM = 15,
            Country = "Brazil",
            CountryCode = "BRA",
            FullName = "Velopark",
            Latitude = -29.822778,
            Longitude = -51.320833,
            ShortName = "Velopark",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade3,
                    LengthM = 2270,
                    MaxGridSize = 20,
                    Name = "Velopark",
                    TrackType = TrackTypeCircuit
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade3,
                    LengthM = 2270,
                    MaxGridSize = 34,
                    Name = "Velopark 2017",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Velopark2"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 10,
                    Grade = Grade3,
                    LengthM = 2150,
                    MaxGridSize = 34,
                    Name = "Velopark 2010",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Velopark2_STT"
                }
            ]
        },
        new()
        {
            AltitudeM = 65,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Virginia International Raceway (VIR)",
            Latitude = 36.561667,
            Longitude = -79.204722,
            ShortName = "Virginia",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 18,
                    Grade = Grade2,
                    LengthM = 5260,
                    MaxGridSize = 48,
                    Name = "VIRginia International Raceway Full",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "VirginiaFull"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 24,
                    Grade = Grade3,
                    LengthM = 6750,
                    MaxGridSize = 28,
                    Name = "VIRginia International Raceway Grand",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "VirginiaGrand"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 14,
                    Grade = Grade3,
                    LengthM = 3620,
                    MaxGridSize = 48,
                    Name = "VIRginia International Raceway North",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "VirginiaNorth"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 9,
                    Grade = Grade3,
                    LengthM = 2650,
                    MaxGridSize = 26,
                    Name = "VIRginia International Raceway South",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "VirginiaSouth"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 13,
                    Grade = Grade4,
                    LengthM = 1770,
                    MaxGridSize = 26,
                    Name = "VIRginia International Raceway Patriot",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "VirginiaPatriot"
                }
            ]
        },
        new()
        {
            AltitudeM = 452,
            Country = "USA",
            CountryCode = "USA",
            FullName = "Watkins Glen International",
            Latitude = 42.3362,
            Longitude = -76.9252,
            ShortName = "Watkins Glen",
            Layouts =
            [
                new Ams2TrackLayoutInfo
                {
                    Corners = 18,
                    Grade = Grade2,
                    LengthM = 5260,
                    MaxGridSize = 48,
                    Name = "Watkins Glen GP",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Watkins_Glen"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 15,
                    Grade = Grade2,
                    LengthM = 5550,
                    MaxGridSize = 48,
                    Name = "Watkins Glen GP (Inner Loop)",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Watkins_Glen_GPIL"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 7,
                    Grade = Grade2,
                    LengthM = 3900,
                    MaxGridSize = 48,
                    Name = "Watkins Glen Short",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Watkins_Glen_S"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade2,
                    LengthM = 3940,
                    MaxGridSize = 48,
                    Name = "Watkins Glen Short (Inner Loop)",
                    TrackType = TrackTypeCircuit,
                    Ams2TrackId = "Watkins_Glen_SIL"
                },
                new Ams2TrackLayoutInfo
                {
                    Corners = 11,
                    Grade = Grade2,
                    LengthM = 3940,
                    MaxGridSize = 20,
                    Name = "Watkins Glen STT",
                    TrackType = TrackTypeCircuit
                }
            ]
        }
    ];

    public static Ams2TrackInfoProvider Instance => singletonInstance ??= new Ams2TrackInfoProvider();

    public Ams2TrackInfo? FindTrackByShortName(string shortName)
    {
        return this.tracks.FirstOrDefault(t => t.ShortName == shortName);
    }

    public ReadOnlyCollection<Ams2TrackInfo> GeTrackInfos()
    {
        return this.tracks.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetTrackName()
    {
        return this.tracks.Select(t => t.ShortName)
                   .ToList()
                   .AsReadOnly();
    }
}