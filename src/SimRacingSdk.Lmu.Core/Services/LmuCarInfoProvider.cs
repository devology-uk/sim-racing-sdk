using System.Collections.ObjectModel;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Services;

public class LmuCarInfoProvider : ILmuCarInfoProvider
{
    private static LmuCarInfoProvider? singletonInstance;

    public static LmuCarInfoProvider Instance => singletonInstance ??= new LmuCarInfoProvider();

    private readonly List<LmuCarInfo> cars =
    [
        new()
        {
            Category = "LMDh",
            Class = "HY",
            DisplayName = "Alpine A424",
            Engine = "3.4L V6 Turbo",
            HeightMm = 1055,
            LengthMm = 5088,
            Manufacturer = "Alpine",
            MaxFuel = 89,
            MaxRpm = 9000,
            ModelId = "Alpine_A424_2024",
            PowerBhp = 675,
            PowerKw = 520,
            ResultCarType = "Alpine A424",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 1992,
            Year = 2024
        },

        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Aston Martin Valkyrie",
            Engine = "6.5L Naturally Aspirated V12",
            HeightMm = 1070,
            LengthMm = 4500,
            Manufacturer = "Aston Martin",
            MaxFuel = 90,
            MaxRpm = 7500,
            ModelId = "Aston_Martin_Valkyrie_2025",
            PowerBhp = 670,
            PowerKw = 500,
            ResultCarType = "Aston Martin Valkyrie",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 1965,
            Year = 2025
        },

        new()
        {
            Category = "LMDh",
            Class = "HY",
            DisplayName = "BMW M Hybrid V8",
            Engine = "4.0L V8 Twin Turbo",
            HeightMm = 1200,
            LengthMm = 4991,
            Manufacturer = "BMW",
            MaxFuel = 89,
            MaxRpm = 8200,
            ModelId = "BMW_M_Hybrid_V8_2023",
            PowerBhp = 650,
            PowerKw = 485,
            ResultCarType = "BMW M Hybrid V8",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 1994,
            Year = 2023
        },

        new()
        {
            Category = "LMDh",
            Class = "HY",
            DisplayName = "Cadillac V-Series.R",
            Engine = "5.5L V8",
            HeightMm = 1168,
            LengthMm = 5100,
            Manufacturer = "Cadillac",
            MaxFuel = 88,
            MaxRpm = 8800,
            ModelId = "Cadillac_V-lmdh_2023",
            PowerBhp = 670,
            PowerKw = 520,
            ResultCarType = "Cadillac V-Series.R",
            Transmission = "7 Speed Sequential",
            WeightKg = 1046,
            WidthMm = 2000,
            Year = 2023
        },

        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Ferrari 499P",
            Engine = "3.0L V6 Twin Turbo",
            HeightMm = 1055,
            LengthMm = 5100,
            Manufacturer = "Ferrari",
            MaxFuel = 90,
            MaxRpm = 8000,
            ModelId = "Ferrari_499P_2023",
            PowerBhp = 670,
            PowerKw = 500,
            ResultCarType = "Ferrari 499P",
            Transmission = "7 Speed Sequential",
            WeightKg = 1057,
            WidthMm = 2000,
            Year = 2023
        },
        new() { Category="LMDh", Class="HY", DisplayName="Genesis GMR-001", Engine="3.2L V8 Twin Turbo", HeightMm=1055, LengthMm=5000, Manufacturer="Genesis", MaxFuel=89, MaxRpm=8500, PowerBhp= 690, PowerKw=0, ResultCarType="Genesis GMR-001", Transmission="7 Speed Sequential", WeightKg=1030, WidthMm=2000, Year=2026},


        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Glickenhaus SCG 007",
            Engine = "3.5L V8 Twin Turbo",
            HeightMm = 1224,
            LengthMm = 4991,
            Manufacturer = "Glickenhaus",
            MaxFuel = 90,
            MaxRpm = 9000,
            ModelId = "SGC_007_2023",
            PowerBhp = 670,
            PowerKw = 520,
            ResultCarType = "Glickenhaus SCG 007",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 2000,
            Year = 2023
        },

        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Isotta Fraschini Tipo 6",
            Engine = "3.0L V6 Turbo",
            HeightMm = 1260,
            LengthMm = 5000,
            Manufacturer = "Isotta Fraschini",
            MaxFuel = 90,
            MaxRpm = 8000,
            ModelId = "Isotta_Tipo6_2024",
            PowerBhp = 670,
            PowerKw = 520,
            ResultCarType = "Isotta Fraschini TIPO6",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 2000,
            Year = 2024
        },
        new()
        {
            Category = "LMDh",
            Class = "HY",
            DisplayName = "Lamborghini SC63",
            Engine = "3.8L V8 Twin Turbo",
            HeightMm = 1170,
            LengthMm = 5100,
            Manufacturer = "Lamborghini",
            MaxFuel = 89,
            MaxRpm = 8500,
            ModelId = "Lamborghini_SC63_2024",
            PowerBhp = 670,
            PowerKw = 520,
            ResultCarType = "Lamborghini SC63",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 2000,
            Year = 2024
        },

        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Peugeot 9X8",
            Engine = "2.6L V6 Twin Turbo",
            HeightMm = 1145,
            LengthMm = 4995,
            Manufacturer = "Peugeot",
            MaxFuel = 90,
            MaxRpm = 8500,
            ModelId = "Peugeot_9x8_2023",
            PowerBhp = 670,
            PowerKw = 520,
            ResultCarType = "Peugeot 9X8",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 2000,
            Year = 2023
        },

        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Peugeot 9X8 2024",
            Engine = "2.6L V6 Twin Turbo",
            HeightMm = 1180,
            LengthMm = 5000,
            Manufacturer = "Peugeot",
            MaxFuel = 90,
            MaxRpm = 8500,
            ModelId = "Peugeot_9x8_2024",
            PowerBhp = 670,
            PowerKw = 520,
            ResultCarType = "Peugeot 9X8 2024",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 2080,
            Year = 2024
        },

        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Porsche 963",
            Engine = "4.6L V8 Twin Turbo",
            HeightMm = 1060,
            LengthMm = 5100,
            Manufacturer = "Porsche",
            MaxFuel = 87,
            MaxRpm = 8300,
            ModelId = "Porsche_963_2023",
            PowerBhp = 670,
            PowerKw = 500,
            ResultCarType = "Porsche 963",
            Transmission = "7 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 2000,
            Year = 2023
        },

        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Toyota GR010-Hybrid",
            Engine = "3.5L V6 Twin Turbo",
            HeightMm = 1150,
            LengthMm = 4900,
            Manufacturer = "Toyota",
            MaxFuel = 90,
            MaxRpm = 7800,
            ModelId = "Toyota_GR10_2023",
            PowerBhp = 670,
            PowerKw = 500,
            ResultCarType = "Toyota GR010",
            Transmission = "7 Speed Sequential",
            WeightKg = 1062,
            WidthMm = 2000,
            Year = 2023
        },

        new()
        {
            Category = "LMH",
            Class = "HY",
            DisplayName = "Vanwall Vandervell 680",
            Engine = "4.5L V8",
            HeightMm = 1168,
            LengthMm = 5000,
            Manufacturer = "Vanwall",
            MaxFuel = 90,
            MaxRpm = 8750,
            ModelId = "Vandervell_680_2023",
            PowerBhp = 670,
            PowerKw = 520,
            ResultCarType = "Vanwall Vandervell 680",
            Transmission = "6 Speed Sequential",
            WeightKg = 1030,
            WidthMm = 2000,
            Year = 2023
        },

        new()
        {
            Category = "LMP2",
            Class = "P2",
            DisplayName = "ORECA 07 Gibson 2023",
            Engine = "4.2l V8",
            HeightMm = 1045,
            LengthMm = 4745,
            Manufacturer = "ORECA",
            MaxFuel = 75,
            MaxRpm = 9000,
            ModelId = "Oreca_07_LM_2023",
            PowerBhp = 603,
            PowerKw = 0,
            ResultCarType = "ORECA 07",
            Transmission = "6 Speed Sequential",
            WeightKg = 930,
            WidthMm = 1895,
            Year = 2023
        },

        new()
        {
            Category = "LMP2",
            Class = "P2",
            DisplayName = "ORECA 07 Gibson 2024",
            Engine = "4.2l V8",
            HeightMm = 1045,
            LengthMm = 4745,
            Manufacturer = "ORECA",
            MaxFuel = 75,
            MaxRpm = 9000,
            ModelId = "Oreca_07_LM_2023",
            PowerBhp = 603,
            PowerKw = 0,
            ResultCarType = "ORECA 07",
            Transmission = "6 Speed Sequential",
            WeightKg = 930,
            WidthMm = 1895,
            Year = 2024
        },

        new() { Category="LMP3", Class="P3", DisplayName="ADESS-03", Engine="3.5L V6 Twin Turbo", HeightMm=1050, LengthMm=4643, Manufacturer="ADESS", MaxFuel=100, MaxRpm=6600, PowerBhp= 470, PowerKw=0, ResultCarType="ADESS-03", Transmission="6 Speed Sequential", WeightKg=930, WidthMm=1890, Year=2025},
new() { Category="LMP3", Class="P3", DisplayName="Duqueine 09", Engine="3.5L V6 Twin Turbo", HeightMm=1050, LengthMm=4643, Manufacturer="Duqueine", MaxFuel=100, MaxRpm=6600, PowerBhp= 470, PowerKw=0, ResultCarType="Duqueine 09", Transmission="6 Speed Sequential", WeightKg=930, WidthMm=1890, Year=2025},
new() { Category="LMP3", Class="P3", DisplayName="Ginetta G61-LT-P325-Evo", Engine="3.5L V6 Twin Turbo", HeightMm=1050, LengthMm=4605, Manufacturer="Ginetta", MaxFuel=100, MaxRpm=6600, PowerBhp= 470, PowerKw=0, ResultCarType="Ginetta G61-LT-P325-Evo", Transmission="6 Speed Sequential", WeightKg=950, WidthMm=1900, Year=2025},
new() { Category="LMP3", Class="P3", DisplayName="Ligier JS P325", Engine="3.5L V6 Twin Turbo", HeightMm=1180, LengthMm=4605, Manufacturer="Ligier", MaxFuel=100, MaxRpm=6600, PowerBhp= 470, PowerKw=0, ResultCarType="Ligier JS P325", Transmission="6 Speed Sequential", WeightKg=950, WidthMm=1900, Year=2025},

        new()
        {
            Category = "GTE",
            Class = "GTE",
            DisplayName = "Aston Martin Vantage AMR",
            Engine = "4.0L V8 Turbo",
            HeightMm = 1274,
            LengthMm = 4665,
            Manufacturer = "Aston Martin",
            MaxFuel = 115,
            MaxRpm = 7000,
            ModelId = "Aston_Martin_Vantage_AMR_2023",
            PowerBhp = 500,
            PowerKw = 0,
            ResultCarType = "Aston Martin Vantage AMR",
            Transmission = "6 Speed Sequential",
            WeightKg = 1245,
            WidthMm = 2153,
            Year = 2023
        },

        new()
        {
            Category = "GTE",
            Class = "GTE",
            DisplayName = "Ferrari 488 GTE Evo",
            Engine = "4.0L V8 Turbo",
            HeightMm = 1090,
            LengthMm = 4614,
            Manufacturer = "Ferrari",
            MaxFuel = 90,
            MaxRpm = 7400,
            ModelId = "Ferrari_488GTE_LM_2023",
            PowerBhp = 500,
            PowerKw = 0,
            ResultCarType = "Ferrari 488 GTE Evo",
            Transmission = "6 Speed Sequential",
            WeightKg = 1245,
            WidthMm = 2050,
            Year = 2023
        },

        new()
        {
            Category = "GTE",
            Class = "GTE",
            DisplayName = "Porsche 911 RSR-19",
            Engine = "4.0L Flat Six",
            HeightMm = 1250,
            LengthMm = 4557,
            Manufacturer = "Porsche",
            MaxFuel = 99,
            MaxRpm = 7400,
            ModelId = "Porsche_911RSR-19_2023",
            PowerBhp = 510,
            PowerKw = 0,
            ResultCarType = "Porsche 911 RSR-19",
            Transmission = "6 Speed Sequential",
            WeightKg = 1243,
            WidthMm = 2042,
            Year = 2023
        },

        new()
        {
            Category = "GTE",
            Class = "GTE",
            DisplayName = "Chevrolet Corvette C8.R",
            Engine = "5.0L V8",
            HeightMm = 1148,
            LengthMm = 4630,
            Manufacturer = "Chevrolet",
            MaxFuel = 96,
            MaxRpm = 7400,
            ModelId = "Chevrolet_C8R_LM_2023",
            PowerBhp = 500,
            PowerKw = 0,
            ResultCarType = "Corvette C8.R GTE",
            Transmission = "6 Speed Sequential",
            WeightKg = 1240,
            WidthMm = 2053,
            Year = 2023
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "Aston Martin Vantage AMR LMGT3",
            Engine = "4.0L V8 Twin Turbo",
            HeightMm = 1144,
            LengthMm = 4616,
            Manufacturer = "Aston Martin",
            MaxFuel = 120,
            MaxRpm = 8000,
            ModelId = "Vantage_AMR_GT3Evo_2024",
            PowerBhp = 535,
            PowerKw = 0,
            ResultCarType = "Aston Martin Vantage AMR LMGT3",
            Transmission = "6 Speed Sequential",
            WeightKg = 1306,
            WidthMm = 2049,
            Year = 2024
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "BMW M4 LMGT3",
            Engine = "3.0L Straight Six Turbo",
            HeightMm = 1308,
            LengthMm = 5020,
            Manufacturer = "BMW",
            MaxFuel = 120,
            MaxRpm = 8000,
            ModelId = "BMW_M4_LMGT3_2023",
            PowerBhp = 590,
            PowerKw = 0,
            ResultCarType = "BMW M4 LMGT3",
            Transmission = "6 Speed Sequential",
            WeightKg = 1339,
            WidthMm = 2040,
            Year = 2023
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "Corvette Z06 LMGT3.R",
            Engine = "5.5L V8",
            HeightMm = 1148,
            LengthMm = 4630,
            Manufacturer = "Chevrolet",
            MaxFuel = 104,
            MaxRpm = 8600,
            ModelId = "Corvette_Z06GT3R_2023",
            PowerBhp = 600,
            PowerKw = 0,
            ResultCarType = "Chevrolet Corvette Z06 LMGT3.R",
            Transmission = "6 Speed Sequential",
            WeightKg = 1344,
            WidthMm = 2050,
            Year = 2023
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "Ferrari 296 LMGT3",
            Engine = "2.9L V6 Turbo",
            HeightMm = 1191,
            LengthMm = 4565,
            Manufacturer = "Ferrari",
            MaxFuel = 120,
            MaxRpm = 8000,
            ModelId = "Ferrari_296GT3_2023",
            PowerBhp = 600,
            PowerKw = 0,
            ResultCarType = "Ferrari 296 LMGT3",
            Transmission = "6 Speed Sequential",
            WeightKg = 1341,
            WidthMm = 2050,
            Year = 2023
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "Ford Mustang LMGT3",
            Engine = "5.4L V8",
            HeightMm = 1392,
            LengthMm = 4797,
            Manufacturer = "Ford",
            MaxFuel = 110,
            MaxRpm = 7625,
            ModelId = "Ford_Mustang_GT3_2024",
            PowerBhp = 550,
            PowerKw = 0,
            ResultCarType = "Ford Mustang LMGT3",
            Transmission = "6 Speed Sequential",
            WeightKg = 1329,
            WidthMm = 1918,
            Year = 2024
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "Lamborghini Huracan LMGT3 Evo2",
            Engine = "V10 Naturally Aspirated",
            HeightMm = 1165,
            LengthMm = 4551,
            Manufacturer = "Lamborghini",
            MaxFuel = 120,
            MaxRpm = 8500,
            ModelId = "Lamborghini_Huracan_GT3_2024",
            PowerBhp = 640,
            PowerKw = 0,
            ResultCarType = "Lamborghini Huracan LMGT3 Evo2",
            Transmission = "6 Speed Sequential",
            WeightKg = 1355,
            WidthMm = 2221,
            Year = 2024
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "Lexus RC F LMGT3",
            Engine = "5.4L V8",
            HeightMm = 1271,
            LengthMm = 4846,
            Manufacturer = "Lexus",
            MaxFuel = 120,
            MaxRpm = 7300,
            ModelId = "LexusRCF_GT3_2024",
            PowerBhp = 500,
            PowerKw = 0,
            ResultCarType = "Lexus RCF LMGT3",
            Transmission = "6 Speed Sequential",
            WeightKg = 1355,
            WidthMm = 2030,
            Year = 2024
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "McLaren 720S LMGT3 Evo",
            Engine = "4.0L V8 Twin Turbo",
            HeightMm = 1196,
            LengthMm = 4543,
            Manufacturer = "McLaren",
            MaxFuel = 120,
            MaxRpm = 8500,
            ModelId = "McLaren_720sGT3Evo_2023",
            PowerBhp = 600,
            PowerKw = 0,
            ResultCarType = "McLaren 720S LMGT3 Evo",
            Transmission = "6 Speed Sequential",
            WeightKg = 1345,
            WidthMm = 2161,
            Year = 2023
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "Porsche 911 GT3 R LMGT3",
            Engine = "4.2L Straight Six",
            HeightMm = 1279,
            LengthMm = 4619,
            Manufacturer = "Porsche",
            MaxFuel = 117,
            MaxRpm = 9250,
            ModelId = "911GT3R_2024",
            PowerBhp = 565,
            PowerKw = 0,
            ResultCarType = "Porsche 911 GT3 R LMGT3",
            Transmission = "6 Speed Sequential",
            WeightKg = 1317,
            WidthMm = 2050,
            Year = 2024
        },

        new()
        {
            Category = "LMGT3",
            Class = "GT3",
            DisplayName = "Mercedes-AMG LMGT3",
            Engine = "6.3-litre V8 Naturally Aspirated",
            HeightMm = 1238,
            LengthMm = 4746,
            Manufacturer = "Mercedes",
            MaxFuel = 120,
            MaxRpm = 7250,
            ModelId = "Mercedes_AMGGT3Evo_2025",
            PowerBhp = 560,
            PowerKw = 0,
            ResultCarType = "Mercedes-AMG LMGT3",
            Transmission = "6 Speed Sequential",
            WeightKg = 1360,
            WidthMm = 2049,
            Year = 2025
        },
    ];

    public LmuCarInfo? GetCarInfoByDisplayName(string displayName)
    {
        return this.cars.FirstOrDefault(
            car => car.DisplayName.Equals(displayName, StringComparison.InvariantCultureIgnoreCase));
    }

    public LmuCarInfo? FindByModelId(string modelId)
    {
        return this.cars.FirstOrDefault(
            car => car.ModelId != null && car.ModelId.Equals(modelId, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    ///     Sadly in some cases the CarType field in LMU result files does not match the Display Name
    /// </summary>
    public LmuCarInfo? GetCarInfoByResultCarType(string carType)
    {
        return this.cars.FirstOrDefault(
            car => car.ResultCarType.Equals(carType, StringComparison.InvariantCultureIgnoreCase));
    }

    public IReadOnlyCollection<LmuCarInfo> GetCarInfos()
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


    public ReadOnlyCollection<LmuCarInfo> GetCarInfosForClass(string carClass)
    {
        return this.cars.Where(c => c.Class == carClass)
                   .ToList()
                   .AsReadOnly();
    }
}