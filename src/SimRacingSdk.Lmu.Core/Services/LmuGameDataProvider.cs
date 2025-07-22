using System.Collections.ObjectModel;
using System.Text.Json;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Services
{
    public class LmuGameDataProvider : ILmuGameDataProvider
    {
        private static LmuGameDataProvider? singletonInstance;
        private readonly IList<LmuCarInfo> cars = new List<LmuCarInfo>
        {
            new()
            {
                Category = "LMDh",
                Class = "HYP",
                DisplayName = "Alpine A424",
                Engine = "3.4L V6 Turbo",
                HeightMm = 1055,
                LengthMm = 5088,
                Manufacturer = "Alpine",
                PowerBhp = 675,
                PowerKw = 520,
                ResultCarType = "Alpine A424",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 1992
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Aston Martin Valkyrie",
                Engine = "6.5L Naturally Aspirated V12",
                HeightMm = 1070,
                LengthMm = 4500,
                Manufacturer = "Aston Martin",
                PowerBhp = 670,
                PowerKw = 500,
                ResultCarType = "Aston Martin Valkyrie",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 1965
            },
            new()
            {
                Category = "LMDh",
                Class = "HYP",
                DisplayName = "BMW M Hybrid V8",
                Engine = "4.0L V8 Twin Turbo",
                HeightMm = 1200,
                LengthMm = 4991,
                Manufacturer = "BMW",
                PowerBhp = 650,
                PowerKw = 485,
                ResultCarType = "BMW M Hybrid V8",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 1994
            },
            new()
            {
                Category = "LMDh",
                Class = "HYP",
                DisplayName = "Cadillac V-Series.R",
                Engine = "5.5L V8",
                HeightMm = 1168,
                LengthMm = 5100,
                Manufacturer = "Cadillac",
                PowerBhp = 670,
                PowerKw = 520,
                ResultCarType = "Cadillac V-Series.R",
                Transmission = "7 Speed Sequential",
                WeightKg = 1046,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Ferrari 499P",
                Engine = "3.0L V6 Twin Turbo",
                HeightMm = 1055,
                LengthMm = 5100,
                Manufacturer = "Ferrari",
                PowerBhp = 670,
                PowerKw = 500,
                ResultCarType = "Ferrari 499P",
                Transmission = "7 Speed Sequential",
                WeightKg = 1057,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Glickenhaus SCG 007",
                Engine = "3.5L V8 Twin Turbo",
                HeightMm = 1224,
                LengthMm = 4991,
                Manufacturer = "Glickenhaus",
                PowerBhp = 670,
                PowerKw = 520,
                ResultCarType = "Glickenhaus SCG 007",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Isotta Fraschini Tipo 6",
                Engine = "3.0L V6 Turbo",
                HeightMm = 1260,
                LengthMm = 5000,
                Manufacturer = "Isotta Fraschini",
                PowerBhp = 670,
                PowerKw = 520,
                ResultCarType = "Isotta Fraschini TIPO6",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMDh",
                Class = "HYP",
                DisplayName = "Lamborghini SC63",
                Engine = "3.8L V8 Twin Turbo",
                HeightMm = 1170,
                LengthMm = 5100,
                Manufacturer = "Lamborghini",
                PowerBhp = 670,
                PowerKw = 520,
                ResultCarType = "Lamborghini SC63",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Peugeot 9X8",
                Engine = "2.6L V6 Twin Turbo",
                HeightMm = 1145,
                LengthMm = 4995,
                Manufacturer = "Peugeot",
                PowerBhp = 670,
                PowerKw = 520,
                ResultCarType = "Peugeot 9X8",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Peugeot 9X8 2024",
                Engine = "2.6L V6 Twin Turbo",
                HeightMm = 1180,
                LengthMm = 5000,
                Manufacturer = "Peugeot",
                PowerBhp = 670,
                PowerKw = 520,
                ResultCarType = "Peugeot 9X8 2024",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 2080
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Porsche 963",
                Engine = "4.6L V8 Twin Turbo",
                HeightMm = 1060,
                LengthMm = 5100,
                Manufacturer = "Porsche",
                PowerBhp = 670,
                PowerKw = 500,
                ResultCarType = "Porsche 963",
                Transmission = "7 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Toyota GR010-Hybrid",
                Engine = "3.5L V6 Twin Turbo",
                HeightMm = 1150,
                LengthMm = 4900,
                Manufacturer = "Toyota",
                PowerBhp = 670,
                PowerKw = 500,
                ResultCarType = "Toyota GR010",
                Transmission = "7 Speed Sequential",
                WeightKg = 1062,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMH",
                Class = "HYP",
                DisplayName = "Vanwall Vandervell 680",
                Engine = "4.5L V8",
                HeightMm = 1168,
                LengthMm = 5000,
                Manufacturer = "Vanwall",
                PowerBhp = 670,
                PowerKw = 520,
                ResultCarType = "Vanwall Vandervell 680",
                Transmission = "6 Speed Sequential",
                WeightKg = 1030,
                WidthMm = 2000
            },
            new()
            {
                Category = "LMP2",
                Class = "LMP2",
                DisplayName = "ORECA 07 Gibson 2023",
                Engine = "4.2l V8",
                HeightMm = 1045,
                LengthMm = 4745,
                Manufacturer = "ORECA",
                PowerBhp = 603,
                PowerKw = 0,
                ResultCarType = "ORECA 07",
                Transmission = "6 Speed Sequential",
                WeightKg = 930,
                WidthMm = 1895
            },
            new()
            {
                Category = "LMP2",
                Class = "LMP2",
                DisplayName = "ORECA 07 Gibson 2024",
                Engine = "4.2l V8",
                HeightMm = 1045,
                LengthMm = 4745,
                Manufacturer = "ORECA",
                PowerBhp = 603,
                PowerKw = 0,
                ResultCarType = "ORECA 07",
                Transmission = "6 Speed Sequential",
                WeightKg = 930,
                WidthMm = 1895
            },
            new()
            {
                Category = "GTE",
                Class = "GTE",
                DisplayName = "Aston Martin Vantage AMR",
                Engine = "4.0L V8 Turbo",
                HeightMm = 1274,
                LengthMm = 4665,
                Manufacturer = "Aston Martin",
                PowerBhp = 500,
                PowerKw = 0,
                ResultCarType = "Aston Martin Vantage AMR",
                Transmission = "6 Speed Sequential",
                WeightKg = 1245,
                WidthMm = 2153
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
                PowerBhp = 500,
                PowerKw = 0,
                ResultCarType = "Ferrari 488 GTE Evo",
                Transmission = "6 Speed Sequential",
                WeightKg = 1245,
                WidthMm = 2050
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
                PowerBhp = 510,
                PowerKw = 0,
                ResultCarType = "Porsche 911 RSR-19",
                Transmission = "6 Speed Sequential",
                WeightKg = 1243,
                WidthMm = 2042
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
                PowerBhp = 500,
                PowerKw = 0,
                ResultCarType = "Corvette C8.R GTE",
                Transmission = "6 Speed Sequential",
                WeightKg = 1240,
                WidthMm = 2053
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
                PowerBhp = 535,
                PowerKw = 0,
                ResultCarType = "Aston Martin Vantage AMR LMGT3",
                Transmission = "6 Speed Sequential",
                WeightKg = 1306,
                WidthMm = 2049
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
                PowerBhp = 590,
                PowerKw = 0,
                ResultCarType = "BMW M4 LMGT3",
                Transmission = "6 Speed Sequential",
                WeightKg = 1339,
                WidthMm = 2040
            },
            new()
            {
                Category = "LMGT3",
                Class = "GT3",
                DisplayName = "Corvette Z06 LMGT3.R",
                Engine = "5.5L V8",
                HeightMm = 1148,
                LengthMm = 4630,
                Manufacturer = "Corvette",
                PowerBhp = 600,
                PowerKw = 0,
                ResultCarType = "Chevrolet Corvette Z06 LMGT3.R",
                Transmission = "6 Speed Sequential",
                WeightKg = 1344,
                WidthMm = 2050
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
                PowerBhp = 600,
                PowerKw = 0,
                ResultCarType = "Ferrari 296 LMGT3",
                Transmission = "6 Speed Sequential",
                WeightKg = 1341,
                WidthMm = 2050
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
                PowerBhp = 550,
                PowerKw = 0,
                ResultCarType = "Ford Mustang LMGT3",
                Transmission = "6 Speed Sequential",
                WeightKg = 1329,
                WidthMm = 1918
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
                PowerBhp = 640,
                PowerKw = 0,
                ResultCarType = "Lamborghini Huracan LMGT3 Evo2",
                Transmission = "6 Speed Sequential",
                WeightKg = 1355,
                WidthMm = 2221
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
                PowerBhp = 500,
                PowerKw = 0,
                ResultCarType = "Lexus RCF LMGT3",
                Transmission = "6 Speed Sequential",
                WeightKg = 1355,
                WidthMm = 2030
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
                PowerBhp = 600,
                PowerKw = 0,
                ResultCarType = "McLaren 720S LMGT3 Evo",
                Transmission = "6 Speed Sequential",
                WeightKg = 1345,
                WidthMm = 2161
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
                PowerBhp = 565,
                PowerKw = 0,
                ResultCarType = "Porsche 911 GT3 R LMGT3",
                Transmission = "6 Speed Sequential",
                WeightKg = 1317,
                WidthMm = 2050
            }
        };
        private readonly ILmuPathProvider pathProvider;
        private readonly IList<LmuTrackInfo> tracks = new List<LmuTrackInfo>
        {
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
                Country = "Quatar",
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
            }
        };

        public LmuGameDataProvider(ILmuPathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        public static LmuGameDataProvider Instance =>
            singletonInstance ??= new LmuGameDataProvider(LmuPathProvider.Instance);

        public LmuCarInfo? GetCarInfoByDisplayName(string displayName)
        {
            return this.cars.FirstOrDefault(
                car => car.DisplayName.Equals(displayName, StringComparison.InvariantCultureIgnoreCase));
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
            return new ReadOnlyCollection<LmuCarInfo>(this.cars);
        }

        public LmuTrackInfo? GeTrackInfoByVenue(string venue)
        {
            return this.tracks.FirstOrDefault(t => t.Name.Equals(venue, StringComparison.InvariantCultureIgnoreCase));
        }

        public LmuSettings GetSettings()
        {
            if(!File.Exists(this.pathProvider.SettingsFilePath))
            {
                return new LmuSettings();
            }

            var json = File.ReadAllText(this.pathProvider.SettingsFilePath);
            return JsonSerializer.Deserialize<LmuSettings>(json)!;
        }

        public IReadOnlyCollection<LmuTrackInfo> GetTrackInfos()
        {
            return new ReadOnlyCollection<LmuTrackInfo>(this.tracks);
        }

        public IList<string> ListResultFiles()
        {
            return Directory.GetFiles(this.pathProvider.ResultsFolder, "*.xml")
                            .ToList();
        }
    }
}