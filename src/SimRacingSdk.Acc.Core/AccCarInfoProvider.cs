using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Models;
using System.Collections.ObjectModel;

namespace SimRacingSdk.Acc.Core;

public class AccCarInfoProvider : IAccCarInfoProvider
{
    private static AccCarInfoProvider? singletonInstance;

    private readonly List<AccCarInfo> cars =
    [
        new()
        {
            AccName = "porsche_991_gt3_r",
            Class = "GT3",
            DisplayName = "Porsche 991 GT3 R",
            ManufacturerTag = "Porsche",
            MaxFuel = 120,
            MaxRpm = 9250,
            ModelId = 0,
            Year = 2018
        },
        new()
        {
            AccName = "mercedes_amg_gt3",
            Class = "GT3",
            DisplayName = "Mercedes-AMG GT3",
            ManufacturerTag = "Mercedes",
            MaxFuel = 120,
            MaxRpm = 7900,
            ModelId = 1,
            Year = 2015
        },
        new()
        {
            AccName = "ferrari_488_gt3",
            Class = "GT3",
            DisplayName = "Ferrari 488 GT3",
            ManufacturerTag = "Ferrari",
            MaxFuel = 110,
            MaxRpm = 7300,
            ModelId = 2,
            Year = 2018
        },
        new()
        {
            AccName = "audi_r8_lms",
            Class = "GT3",
            DisplayName = "Audi R8 LMS",
            ManufacturerTag = "Audi",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 3,
            Year = 2015
        },
        new()
        {
            AccName = "lamborghini_huracán_gt3",
            Class = "GT3",
            DisplayName = "Lamborghini Huracán GT3",
            ManufacturerTag = "Lamborghini",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 4,
            Year = 2015
        },
        new()
        {
            AccName = "mclaren_650s_gt3",
            Class = "GT3",
            DisplayName = "McLaren 650S GT3",
            ManufacturerTag = "McLaren",
            MaxFuel = 125,
            MaxRpm = 7500,
            ModelId = 5,
            Year = 2015
        },
        new()
        {
            AccName = "nissan_gt_r_gt3_2018",
            Class = "GT3",
            DisplayName = "Nissan GT-R Nismo GT3",
            ManufacturerTag = "Nissan",
            MaxFuel = 132,
            MaxRpm = 7500,
            ModelId = 6,
            Year = 2018
        },
        new()
        {
            AccName = "bmw_m6_gt3",
            Class = "GT3",
            DisplayName = "BMW M6 GT3",
            ManufacturerTag = "BMW",
            MaxFuel = 125,
            MaxRpm = 7100,
            ModelId = 7,
            Year = 2017
        },
        new()
        {
            AccName = "bentley_continental_gt3_2018",
            Class = "GT3",
            DisplayName = "Bentley Continental GT3",
            ManufacturerTag = "Bentley",
            MaxFuel = 132,
            MaxRpm = 7400,
            ModelId = 8,
            Year = 2018
        },
        new()
        {
            AccName = "porsche_991ii_gt3_cup",
            Class = "GTC",
            DisplayName = "Porsche 991 II GT3 Cup",
            ManufacturerTag = "Porsche",
            MaxFuel = 100,
            MaxRpm = 8500,
            ModelId = 9,
            Year = 2017
        },
        new()
        {
            AccName = "nissan_gt_r_gt3_2017",
            Class = "GT3",
            DisplayName = "Nissan GT-R Nismo GT3",
            ManufacturerTag = "Nissan",
            MaxFuel = 132,
            MaxRpm = 7500,
            ModelId = 10,
            Year = 2015
        },
        new()
        {
            AccName = "bentley_continental_gt3_2016",
            Class = "GT3",
            DisplayName = "Bentley Continental GT3",
            ManufacturerTag = "Bentley",
            MaxFuel = 132,
            MaxRpm = 7500,
            ModelId = 11,
            Year = 2016
        },
        new()
        {
            AccName = "amr_v12_vantage_gt3",
            Class = "GT3",
            DisplayName = "AMR V12 Vantage GT3",
            ManufacturerTag = "Aston-Martin",
            MaxFuel = 132,
            MaxRpm = 7750,
            ModelId = 12,
            Year = 2013
        },
        new()
        {
            AccName = "lamborghini_gallardo_rex",
            Class = "GT3",
            DisplayName = "Reiter Engineering R-EX GT3",
            ManufacturerTag = "Lamborghini",
            MaxFuel = 130,
            MaxRpm = 8650,
            ModelId = 13,
            Year = 2017
        },
        new()
        {
            AccName = "jaguar_g3",
            Class = "GT3",
            DisplayName = "Emil Frey Jaguar G3",
            ManufacturerTag = "Jaguar",
            MaxFuel = 119,
            MaxRpm = 8750,
            ModelId = 14,
            Year = 2012
        },
        new()
        {
            AccName = "lexus_rc_f_gt3",
            Class = "GT3",
            DisplayName = "Lexus RC F GT3",
            ManufacturerTag = "Lexus",
            MaxFuel = 120,
            MaxRpm = 7750,
            ModelId = 15,
            Year = 2016
        },
        new()
        {
            AccName = "lamborghini_huracan_gt3_evo",
            Class = "GT3",
            DisplayName = "Lamborghini Huracan GT3 Evo",
            ManufacturerTag = "Lamborghini",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 16,
            Year = 2019
        },
        new()
        {
            AccName = "honda_nsx_gt3",
            Class = "GT3",
            DisplayName = "Honda NSX GT3",
            ManufacturerTag = "Honda",
            MaxFuel = 120,
            MaxRpm = 7500,
            ModelId = 17,
            Year = 2017
        },
        new()
        {
            AccName = "lamborghini_huracan_st",
            Class = "GTC",
            DisplayName = "Lamborghini Huracan SuperTrofeo",
            ManufacturerTag = "Lamborghini",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 18,
            Year = 2015
        },
        new()
        {
            AccName = "audi_r8_lms_evo",
            Class = "GT3",
            DisplayName = "Audi R8 LMS Evo",
            ManufacturerTag = "Audi",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 19,
            Year = 2019
        },
        new()
        {
            AccName = "amr_v8_vantage_gt3",
            Class = "GT3",
            DisplayName = "AMR V8 Vantage",
            ManufacturerTag = "Aston-Martin",
            MaxFuel = 120,
            MaxRpm = 7250,
            ModelId = 20,
            Year = 2019
        },
        new()
        {
            AccName = "honda_nsx_gt3_evo",
            Class = "GT3",
            DisplayName = "Honda NSX GT3 Evo",
            ManufacturerTag = "Honda",
            MaxFuel = 120,
            MaxRpm = 7650,
            ModelId = 21,
            Year = 2019
        },
        new()
        {
            AccName = "mclaren_720s_gt3",
            Class = "GT3",
            DisplayName = "McLaren 720S GT3",
            ManufacturerTag = "McLaren",
            MaxFuel = 125,
            MaxRpm = 7700,
            ModelId = 22,
            Year = 2019
        },
        new()
        {
            AccName = "porsche_991ii_gt3_r",
            Class = "GT3",
            DisplayName = "Porsche 991 II GT3 R",
            ManufacturerTag = "Porsche",
            MaxFuel = 120,
            MaxRpm = 9250,
            ModelId = 23,
            Year = 2019
        },
        new()
        {
            AccName = "ferrari_488_gt3_evo",
            Class = "GT3",
            DisplayName = "Ferrari 488 GT3 Evo",
            ManufacturerTag = "Ferrari",
            MaxFuel = 120,
            MaxRpm = 7600,
            ModelId = 24,
            Year = 2020
        },
        new()
        {
            AccName = "mercedes_amg_gt3_evo",
            Class = "GT3",
            DisplayName = "Mercedes-AMG GT3",
            ManufacturerTag = "Mercedes",
            MaxFuel = 120,
            MaxRpm = 7600,
            ModelId = 25,
            Year = 2020
        },
        new()
        {
            AccName = "ferrari_488_challenge_evo",
            Class = "GTC",
            DisplayName = "Ferrari 488 Challenge Evo",
            ManufacturerTag = "Ferrari",
            MaxFuel = 120,
            MaxRpm = 8000,
            ModelId = 26,
            Year = 2020
        },
        new()
        {
            AccName = "bmw_m2_cs_racing",
            Class = "TCX",
            DisplayName = "BMW M2 Club Sport Racing",
            ManufacturerTag = "BMW",
            MaxFuel = 120,
            MaxRpm = 7520,
            ModelId = 27,
            Year = 2020
        },
        new()
        {
            AccName = "porsche_992_gt3_cup",
            Class = "GTC",
            DisplayName = "Porsche 992 GT3 Cup",
            ManufacturerTag = "Porsche",
            MaxFuel = 120,
            MaxRpm = 8750,
            ModelId = 28,
            Year = 2021
        },
        new()
        {
            AccName = "lamborghini_huracan_st_evo2",
            Class = "GTC",
            DisplayName = "Lamborghini Huracan SuperTrofeo EVO2",
            ManufacturerTag = "Lamborghini",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 29,
            Year = 2021
        },
        new()
        {
            AccName = "bmw_m4_gt3",
            Class = "GT3",
            DisplayName = "BMW M4 GT3",
            ManufacturerTag = "BMW",
            MaxFuel = 120,
            MaxRpm = 7000,
            ModelId = 30,
            Year = 2022
        },
        new()
        {
            AccName = "audi_r8_lms_evo_ii",
            Class = "GT3",
            DisplayName = "Audi R8 LMS GT3 Evo 2",
            ManufacturerTag = "Audi",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 31,
            Year = 2022
        },
        new()
        {
            AccName = "alpine_a110_gt4",
            Class = "GT4",
            DisplayName = "Alpine A110 GT4",
            ManufacturerTag = "Alpine",
            MaxFuel = 120,
            MaxRpm = 6450,
            ModelId = 50,
            Year = 2018
        },
        new()
        {
            AccName = "amr_v8_vantage_gt4",
            Class = "GT4",
            DisplayName = "Aston Martin Vantage GT4",
            ManufacturerTag = "Aston-Martin",
            MaxFuel = 120,
            MaxRpm = 7000,
            ModelId = 51,
            Year = 2018
        },
        new()
        {
            AccName = "audi_r8_gt4",
            Class = "GT4",
            DisplayName = "Audi R8 GT4",
            ManufacturerTag = "Audi",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 52,
            Year = 2018
        },
        new()
        {
            AccName = "bmw_m4_gt4",
            Class = "GT4",
            DisplayName = "BMW M4 GT4",
            ManufacturerTag = "BMW",
            MaxFuel = 120,
            MaxRpm = 7600,
            ModelId = 53,
            Year = 2018
        },
        new()
        {
            AccName = "chevrolet_camaro_gt4",
            Class = "GT4",
            DisplayName = "Chevrolet Camaro GT4",
            ManufacturerTag = "Chevrolet",
            MaxFuel = 120,
            MaxRpm = 7500,
            ModelId = 55,
            Year = 2017
        },
        new()
        {
            AccName = "ginetta_g55_gt4",
            Class = "GT4",
            DisplayName = "Ginetta G55 GT4",
            ManufacturerTag = "Ginetta",
            MaxFuel = 120,
            MaxRpm = 7200,
            ModelId = 56,
            Year = 2012
        },
        new()
        {
            AccName = "ktm_xbow_gt4",
            Class = "GT4",
            DisplayName = "KTM X-Bow GT4",
            ManufacturerTag = "KTM",
            MaxFuel = 120,
            MaxRpm = 6500,
            ModelId = 57,
            Year = 2016
        },
        new()
        {
            AccName = "maserati_mc_gt4",
            Class = "GT4",
            DisplayName = "Maserati MC GT4",
            ManufacturerTag = "Maserati",
            MaxFuel = 120,
            MaxRpm = 7000,
            ModelId = 58,
            Year = 2016
        },
        new()
        {
            AccName = "mclaren_570s_gt4",
            Class = "GT4",
            DisplayName = "McLaren 570S GT4",
            ManufacturerTag = "McLaren",
            MaxFuel = 120,
            MaxRpm = 7600,
            ModelId = 59,
            Year = 2016
        },
        new()
        {
            AccName = "mercedes_amg_gt4",
            Class = "GT4",
            DisplayName = "Mercedes AMG GT4",
            ManufacturerTag = "Mercedes",
            MaxFuel = 120,
            MaxRpm = 7000,
            ModelId = 60,
            Year = 2016
        },
        new()
        {
            AccName = "porsche_718_cayman_gt4_mr",
            Class = "GT4",
            DisplayName = "Porsche 718 Cayman GT4 Clubsport",
            ManufacturerTag = "Porsche",
            MaxFuel = 120,
            MaxRpm = 7800,
            ModelId = 61,
            Year = 2019
        },
        new()
        {
            AccName = "ferrari_296_gt3",
            Class = "GT3",
            DisplayName = "Ferrari 296 GT3",
            ManufacturerTag = "Ferrari",
            MaxFuel = 120,
            MaxRpm = 7300,
            ModelId = 32,
            Year = 2023
        },
        new()
        {
            AccName = "lamborghini_huracan_gt3_evo2",
            Class = "GT3",
            DisplayName = "Lamborghini Huracan GT3 Evo 2",
            ManufacturerTag = "Lamborghini",
            MaxFuel = 120,
            MaxRpm = 8650,
            ModelId = 33,
            Year = 2023
        },
        new()
        {
            AccName = "porsche_992_gt3_r",
            Class = "GT3",
            DisplayName = "Porsche 992 GT3 R",
            ManufacturerTag = "Porsche",
            MaxFuel = 120,
            MaxRpm = 9250,
            ModelId = 34,
            Year = 2023
        },
        new()
        {
            AccName = "mclaren_720s_gt3_evo",
            Class = "GT3",
            DisplayName = "McLaren 720S GT3 Evo",
            ManufacturerTag = "McLaren",
            MaxFuel = 125,
            MaxRpm = 7700,
            ModelId = 35,
            Year = 2023
        },
        new()
        {
            AccName = "audi_r8_lms_gt2",
            Class = "GT2",
            DisplayName = "Audi R8 LMS",
            ManufacturerTag = "Audi",
            MaxFuel = 120,
            MaxRpm = 8700,
            ModelId = 80,
            Year = 2021
        },
        new()
        {
            AccName = "ktm_xbow_gt2",
            Class = "GT2",
            DisplayName = "KTM XBOW",
            ManufacturerTag = "KTM",
            MaxFuel = 120,
            MaxRpm = 7500,
            ModelId = 82,
            Year = 2021
        },
        new()
        {
            AccName = "maserati_mc20_gt2",
            Class = "GT2",
            DisplayName = "Maserati MC20",
            ManufacturerTag = "Maserati",
            MaxFuel = 120,
            MaxRpm = 8000,
            ModelId = 83,
            Year = 2023
        },
        new()
        {
            AccName = "mercedes_amg_gt2",
            Class = "GT2",
            DisplayName = "Mercedes AMG",
            ManufacturerTag = "Mercedes",
            MaxFuel = 120,
            MaxRpm = 7600,
            ModelId = 84,
            Year = 2023
        },
        new()
        {
            AccName = "porsche_991_gt2_rs_mr",
            Class = "GT2",
            DisplayName = "Porsche 991 RS CS Evo",
            ManufacturerTag = "Porsche",
            MaxFuel = 115,
            MaxRpm = 7300,
            ModelId = 85,
            Year = 2023
        },
        new()
        {
            AccName = "porsche_935",
            Class = "GT2",
            DisplayName = "Porsche 935",
            ManufacturerTag = "Porsche",
            MaxFuel = 115,
            MaxRpm = 7200,
            ModelId = 86,
            Year = 2019
        },
        new()
        {
            AccName = "ford_mustang_gt3",
            Class = "GT3",
            DisplayName = "Ford Mustang",
            ManufacturerTag = "Ford",
            MaxFuel = 120,
            MaxRpm = 8300,
            ModelId = 36,
            Year = 2024
        }
    ];
    public static AccCarInfoProvider Instance => singletonInstance ??= new AccCarInfoProvider();

    public AccCarInfo? FindByModelId(int modelId)
    {
        return this.cars.FirstOrDefault(c => c.ModelId == modelId);
    }

    public string GetCarDisplayNameWithYear(int modelId)
    {
        var car = this.FindByModelId(modelId);

        return car == null? string.Empty: $"{car.DisplayName} ({car.Year})";
    }

    public IReadOnlyCollection<AccCarInfo> GetCarInfos()
    {
        return this.cars.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetCarClasses()
    {
        return this.cars.Select(c => c.Class)
                   .Distinct()
                   .ToList()
                   .AsReadOnly();
    }

    public ReadOnlyCollection<AccCarInfo> GetCarInfosForClass(string carClass)
    {
        return this.cars.Where(c => c.Class == carClass)
                   .ToList()
                   .AsReadOnly();
    }
}