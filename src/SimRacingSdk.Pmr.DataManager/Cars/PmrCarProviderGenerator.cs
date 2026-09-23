using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using SimRacingSdk.Pmr.DataManager.Storage;

namespace SimRacingSdk.Pmr.DataManager.Cars;

// Regenerates SimRacingSdk.Pmr.Core's whole car-catalog surface from the cars captured here -
// the model and interface as well as the provider, not just the provider's data, so nothing in
// Core has to pre-exist for a car to reach it. Matches the shape that
// SimRacingSdk.Tools.PmrCatalogImport used to hand-generate from CSV (same Id convention, same
// method set), so it's a drop-in replacement for the files that tool used to write.
//
// GameId writes empty for any car where it's still null here (FindByGameId returns null for those
// until it's looked up in stats.xml and filled in).
public class PmrCarProviderGenerator : IPmrCarProviderGenerator
{
    private readonly IDataPathProvider dataPathProvider;

    public PmrCarProviderGenerator(IDataPathProvider dataPathProvider)
    {
        this.dataPathProvider = dataPathProvider;
    }

    public string Generate(IEnumerable<CarInfo> cars)
    {
        var coreRoot = Path.Combine(this.dataPathProvider.GetRepoRoot(), "src", "SimRacingSdk.Pmr.Core");
        var carList = cars.ToList();

        File.WriteAllText(Path.Combine(coreRoot, "Models", "PmrCarInfo.cs"), BuildModelSource());
        File.WriteAllText(Path.Combine(coreRoot, "Abstractions", "IPmrCarInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "PmrCarInfoProvider.cs");
        File.WriteAllText(providerPath, BuildSource(carList));
        return providerPath;
    }

    private static string BuildEntryLine(CarInfo car)
    {
        var id = Regex.Replace($"{car.Manufacturer} {car.Name} {car.Year}".Trim(), @"\s+", "_");

        return "        new() { "
               + $"EngineDisplacementLitres = {CsNumber(car.EngineDisplacementLitres)}, "
               + $"EngineLayout = {CsString(car.EngineLayout)}, "
               + $"EngineType = {CsString(car.EngineType)}, "
               + $"FuelCapacityLitres = {CsNullableNumber(car.FuelCapacityLitres)}, "
               + $"GameId = {CsString(car.GameId ?? string.Empty)}, "
               + $"IconFileName = {CsString(car.IconFileName)}, "
               + $"Id = {CsString(id)}, "
               + $"Manufacturer = {CsString(car.Manufacturer)}, "
               + $"Name = {CsString(car.Name)}, "
               + $"PowerKw = {CsNumber(car.PowerKw)}, "
               + $"PowertrainLayout = {CsString(car.PowertrainLayout)}, "
               + $"TorqueNm = {CsNumber(car.TorqueNm)}, "
               + $"Transmission = {CsString(car.Transmission)}, "
               + $"VehicleClass = {CsString(car.VehicleClass)}, "
               + $"WeightKg = {CsNumber(car.WeightKg)}, "
               + $"Year = {car.Year} "
               + "},";
    }

    private static string BuildInterfaceSource()
    {
        return """
               using SimRacingSdk.Pmr.Core.Models;

               namespace SimRacingSdk.Pmr.Core.Abstractions;

               public interface IPmrCarInfoProvider
               {
                   PmrCarInfo? FindByGameId(string gameId);
                   PmrCarInfo? FindById(string id);
                   IReadOnlyCollection<PmrCarInfo> GetCarInfos();
                   IReadOnlyCollection<PmrCarInfo> GetCarInfosForManufacturer(string manufacturer);
                   IReadOnlyCollection<string> GetManufacturers();
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Pmr.Core.Models;

               public record PmrCarInfo
               {
                   public double EngineDisplacementLitres { get; init; }

                   // Front/Mid/Rear Engine - separate from PowertrainLayout (the drivetrain) since the game's
                   // own car-select screen shows them as two distinct facts, not one combined string.
                   public string EngineLayout { get; init; }

                   // Cylinder configuration as the game names it - "V8", "Straight 6", "Flat-6", "4-Rotor" etc.
                   public string EngineType { get; init; }

                   // Null when the game's own vdef and vset files carry no fuel-tank spec at all - seen only
                   // on the Hypercar/LMDh class, which is regulated by an energy allocation rather than a
                   // simple litres figure.
                   public double? FuelCapacityLitres { get; init; }

                   // The game's own vehicle id (e.g. "ID_BMW_M_Hybrid_V8") - the prefix of a saved setup's
                   // .vset file name. Its casing varies between cars in the game's own data, so compare it
                   // case-insensitively.
                   public string GameId { get; init; }

                   // Usually equal to Manufacturer - overridden where a manufacturer trades under more than
                   // one name for logo purposes (e.g. "Mercedes-AMG"/"Mercedes-Benz" both use the one generic
                   // Mercedes logo), so consumers can bind straight to this instead of Manufacturer for the
                   // CarManufacturerImageSourceConverter lookup.
                   public string IconFileName { get; init; }

                   // Derived from Manufacturer + Name + Year - a stable slug, not the game's own id (see GameId).
                   public string Id { get; init; }

                   public string Manufacturer { get; init; }
                   public string Name { get; init; }
                   public double PowerKw { get; init; }

                   // FWD/RWD/4WD - the drivetrain, as distinct from EngineLayout above.
                   public string PowertrainLayout { get; init; }

                   public double TorqueNm { get; init; }
                   public string Transmission { get; init; }
                   public string VehicleClass { get; init; }
                   public double WeightKg { get; init; }
                   public int Year { get; init; }

                   public string DisplayName => $"{this.Manufacturer} {this.Name}";
               }

               """;
    }

    private static string BuildSource(IEnumerable<CarInfo> cars)
    {
        var entryLines = cars.OrderBy(car => car.Manufacturer)
            .ThenBy(car => car.Name)
            .Select(BuildEntryLine);

        return $$"""
                 using SimRacingSdk.Pmr.Core.Abstractions;
                 using SimRacingSdk.Pmr.Core.Models;

                 namespace SimRacingSdk.Pmr.Core;

                 public class PmrCarInfoProvider : IPmrCarInfoProvider
                 {
                     private static PmrCarInfoProvider? singletonInstance;

                     private readonly List<PmrCarInfo> cars =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     public static PmrCarInfoProvider Instance => singletonInstance ??= new PmrCarInfoProvider();

                     public PmrCarInfo? FindByGameId(string gameId)
                     {
                         return this.cars.FirstOrDefault(
                             c => string.Equals(c.GameId, gameId, StringComparison.OrdinalIgnoreCase));
                     }

                     public PmrCarInfo? FindById(string id)
                     {
                         return this.cars.FirstOrDefault(c => c.Id == id);
                     }

                     public IReadOnlyCollection<PmrCarInfo> GetCarInfos()
                     {
                         return this.cars.AsReadOnly();
                     }

                     public IReadOnlyCollection<PmrCarInfo> GetCarInfosForManufacturer(string manufacturer)
                     {
                         return this.cars.Where(c => c.Manufacturer == manufacturer)
                                          .ToList()
                                          .AsReadOnly();
                     }

                     public IReadOnlyCollection<string> GetManufacturers()
                     {
                         return this.cars.Select(c => c.Manufacturer)
                                          .Distinct()
                                          .OrderBy(m => m)
                                          .ToList()
                                          .AsReadOnly();
                     }
                 }

                 """;
    }

    private static string CsNullableNumber(double? value)
    {
        return value.HasValue ? CsNumber(value.Value) : "null";
    }

    private static string CsNumber(double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    private static string CsString(string value)
    {
        return $"\"{value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
    }
}
