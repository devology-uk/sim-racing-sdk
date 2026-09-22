using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using SimRacingSdk.Pmr.DataManager.Storage;

namespace SimRacingSdk.Pmr.DataManager.Cars;

// Regenerates SimRacingSdk.Pmr.Core's PmrCarInfoProvider.cs from the cars captured here - the
// codegen step SimRacingSdk.Tools.PmrCatalogImport used to do from CSV, now driven from this app's
// own JSON catalog instead. Matches that tool's output shape (same Id convention, same method set)
// exactly, so it's a drop-in replacement for the file it used to generate.
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
        var path = Path.Combine(this.dataPathProvider.GetRepoRoot(), "src", "SimRacingSdk.Pmr.Core", "PmrCarInfoProvider.cs");
        File.WriteAllText(path, BuildSource(cars));
        return path;
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
