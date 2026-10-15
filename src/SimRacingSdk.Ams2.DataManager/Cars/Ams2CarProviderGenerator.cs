using System.IO;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Storage;
using static SimRacingSdk.DataManager.Shared.Generation.CSharpLiteral;

namespace SimRacingSdk.Ams2.DataManager.Cars;

// Regenerates SimRacingSdk.Ams2.Core's whole car-catalog surface - model, interface and provider -
// from the cars captured here.
public class Ams2CarProviderGenerator : ICatalogGenerator<CarInfo>
{
    private readonly IDataPathProvider dataPathProvider;
    private readonly DataManagerOptions options;

    public Ams2CarProviderGenerator(IDataPathProvider dataPathProvider, DataManagerOptions options)
    {
        this.dataPathProvider = dataPathProvider;
        this.options = options;
    }

    public string Generate(IReadOnlyCollection<CarInfo> entities)
    {
        var coreRoot = this.dataPathProvider.GetProjectFolder(this.options.CoreProjectName);

        File.WriteAllText(Path.Combine(coreRoot, "Models", "Ams2CarInfo.cs"), BuildModelSource());
        File.WriteAllText(Path.Combine(coreRoot, "Abstractions", "IAms2CarInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "Ams2CarInfoProvider.cs");
        File.WriteAllText(providerPath, BuildProviderSource(entities));
        return providerPath;
    }

    private static string BuildEntryLine(CarInfo car)
    {
        var assignments = new List<string>();
        if(car.Ams2CarId is not null)
        {
            assignments.Add($"Ams2CarId = {Text(car.Ams2CarId)}");
        }

        assignments.Add($"Class = {Text(car.Class)}");
        assignments.Add($"Differential = {Text(car.Differential)}");
        assignments.Add($"DisplacementL = {Number(car.DisplacementL)}");
        if(car.DlcPack is not null)
        {
            assignments.Add($"DlcPack = {Text(car.DlcPack)}");
        }

        assignments.Add($"Driveline = {Text(car.Driveline)}");
        assignments.Add($"Electronics = {Text(car.Electronics)}");
        assignments.Add($"Engine = {Text(car.Engine)}");
        assignments.Add($"EnginePlacement = {Text(car.EnginePlacement)}");
        assignments.Add($"HasAdjustableTurbo = {Bool(car.HasAdjustableTurbo)}");
        assignments.Add($"HasBoostButton = {Bool(car.HasBoostButton)}");
        assignments.Add($"HasDrs = {Bool(car.HasDrs)}");
        assignments.Add($"HasHeadlights = {Bool(car.HasHeadlights)}");
        assignments.Add($"HasOnBoardBrakeBias = {Bool(car.HasOnBoardBrakeBias)}");
        assignments.Add($"HasOnBoardRollBars = {Bool(car.HasOnBoardRollBars)}");
        assignments.Add($"HasPitSpeedLimiter = {Bool(car.HasPitSpeedLimiter)}");
        assignments.Add($"Manufacturer = {Text(car.Manufacturer)}");
        assignments.Add($"Model = {Text(car.Model)}");
        assignments.Add($"PowerHp = {Number(car.PowerHp)}");
        assignments.Add($"TopSpeedKph = {car.TopSpeedKph}");
        assignments.Add($"TorqueNm = {Number(car.TorqueNm)}");
        assignments.Add($"Transmission = {Text(car.Transmission)}");
        assignments.Add($"WeightDistribution = {Text(car.WeightDistribution)}");
        assignments.Add($"WeightKg = {Number(car.WeightKg)}");
        assignments.Add($"WheelBaseM = {Number(car.WheelBaseM)}");

        return $"        new() {{ {string.Join(", ", assignments)} }},";
    }

    private static string BuildInterfaceSource()
    {
        return """
               using System.Collections.ObjectModel;
               using SimRacingSdk.Ams2.Core.Models;

               namespace SimRacingSdk.Ams2.Core.Abstractions;

               public interface IAms2CarInfoProvider
               {
                   Ams2CarInfo? FindByModel(string model);
                   Ams2CarInfo? FindByRawVehicleName(string rawVehicleName);
                   ReadOnlyCollection<string> GetCarClasses();
                   ReadOnlyCollection<Ams2CarInfo> GetCarInfos();
                   ReadOnlyCollection<Ams2CarInfo> GetCarInfosForClass(string carClass);
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Ams2.Core.Models;

               public record Ams2CarInfo
               {
                   private string ams2CarId;

                   public string Ams2CarId
                   {
                       get => this.ams2CarId ?? this.Model.Replace(" ", "_");
                       set => this.ams2CarId = value;
                   }

                   public string Class { get; set; }
                   public string Differential { get; set; }
                   public double DisplacementL { get; set; }
                   public string DlcPack { get; set; }
                   public string Driveline { get; set; }
                   public string Electronics { get; set; }
                   public string Engine { get; set; }
                   public string EnginePlacement { get; set; }
                   public bool HasAdjustableTurbo { get; set; }
                   public bool HasBoostButton { get; set; }
                   public bool HasDrs { get; set; }
                   public bool HasHeadlights { get; set; }
                   public bool HasOnBoardBrakeBias { get; set; }
                   public bool HasOnBoardRollBars { get; set; }
                   public bool HasPitSpeedLimiter { get; set; }
                   public string Manufacturer { get; set; }
                   public string Model { get; set; }
                   public double PowerHp { get; set; }
                   public int TopSpeedKph { get; set; }
                   public double TorqueNm { get; set; }
                   public string Transmission { get; set; }
                   public string WeightDistribution { get; set; }
                   public double WeightKg { get; set; }
                   public double WheelBaseM { get; set; }
               }

               """;
    }

    // Sorted by Class first: FindByModel returns the first match, and a car in two classes (e.g. Oreca 07
    // in LMP2 Gen 1 and Gen 2) has always resolved to the earlier class.
    private static string BuildProviderSource(IEnumerable<CarInfo> cars)
    {
        var entryLines = cars.OrderBy(car => car.Class)
            .ThenBy(car => car.Model)
            .Select(BuildEntryLine);

        return $$"""
                 using System.Collections.ObjectModel;
                 using SimRacingSdk.Ams2.Core.Abstractions;
                 using SimRacingSdk.Ams2.Core.Models;

                 namespace SimRacingSdk.Ams2.Core;

                 public class Ams2CarInfoProvider : IAms2CarInfoProvider
                 {
                     private static Ams2CarInfoProvider? singletonInstance;

                     private readonly List<Ams2CarInfo> cars =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     public static Ams2CarInfoProvider Instance => singletonInstance ??= new Ams2CarInfoProvider();

                     public Ams2CarInfo? FindByModel(string model)
                     {
                         return this.cars.FirstOrDefault(c => c.Model == model);
                     }

                     // AMS2's shared memory appends a BOP-variant suffix to some GT3 cars' names (e.g. "Porsche 992 GT3 R -
                     // Low Downforce"), which the catalog has no separate entry for - falls back to matching the catalog's base
                     // Model as a prefix once an exact match fails.
                     public Ams2CarInfo? FindByRawVehicleName(string rawVehicleName)
                     {
                         var exactMatch = this.cars.FirstOrDefault(c => c.Model == rawVehicleName);
                         if(exactMatch != null)
                         {
                             return exactMatch;
                         }

                         return this.cars.FirstOrDefault(c =>
                             rawVehicleName.StartsWith(c.Model + " - ", StringComparison.OrdinalIgnoreCase));
                     }

                     public ReadOnlyCollection<Ams2CarInfo> GetCarInfos()
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

                     public ReadOnlyCollection<Ams2CarInfo> GetCarInfosForClass(string carClass)
                     {
                         return this.cars.Where(c => c.Class == carClass)
                                    .ToList()
                                    .AsReadOnly();
                     }
                 }

                 """;
    }
}
