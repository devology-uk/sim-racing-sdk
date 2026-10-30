using System.IO;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Generation;
using SimRacingSdk.DataManager.Shared.Storage;
using static SimRacingSdk.DataManager.Shared.Generation.CSharpLiteral;

namespace SimRacingSdk.Lmu.DataManager.Cars;

// Regenerates SimRacingSdk.Lmu.Core's whole car-catalog surface - model, interface and provider -
// from the cars captured here, so nothing in Core has to pre-exist for a car to reach it.
public class LmuCarProviderGenerator : ICatalogGenerator<CarInfo>
{
    private readonly IDataPathProvider dataPathProvider;
    private readonly DataManagerOptions options;

    public LmuCarProviderGenerator(IDataPathProvider dataPathProvider, DataManagerOptions options)
    {
        this.dataPathProvider = dataPathProvider;
        this.options = options;
    }

    public string Generate(IReadOnlyCollection<CarInfo> entities)
    {
        var coreRoot = this.dataPathProvider.GetProjectFolder(this.options.CoreProjectName);

        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "LmuCarInfo.cs"), BuildModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Abstractions", "ILmuCarInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "Services", "LmuCarInfoProvider.cs");
        GeneratedSource.Write(providerPath, BuildProviderSource(entities));
        return providerPath;
    }

    private static string BuildEntryLine(CarInfo car)
    {
        return "        new() { "
               + $"Category = {Text(car.Category)}, "
               + $"Class = {Text(car.Class)}, "
               + $"DisplayName = {Text(car.DisplayName)}, "
               + $"Engine = {Text(car.Engine)}, "
               + $"HeightMm = {car.HeightMm}, "
               + $"LengthMm = {car.LengthMm}, "
               + $"Manufacturer = {Text(car.Manufacturer)}, "
               + $"MaxFuel = {car.MaxFuel}, "
               + $"MaxRpm = {car.MaxRpm}, "
               + $"ModelId = {NullableText(car.ModelId)}, "
               + $"PowerBhp = {car.PowerBhp}, "
               + $"PowerKw = {car.PowerKw}, "
               + $"ResultCarType = {Text(car.ResultCarType)}, "
               + $"Transmission = {Text(car.Transmission)}, "
               + $"WeightKg = {car.WeightKg}, "
               + $"WidthMm = {car.WidthMm}, "
               + $"Year = {car.Year} "
               + "},";
    }

    private static string BuildInterfaceSource()
    {
        return """
               using System.Collections.ObjectModel;
               using SimRacingSdk.Lmu.Core.Models;

               namespace SimRacingSdk.Lmu.Core.Abstractions;

               public interface ILmuCarInfoProvider
               {
                   LmuCarInfo? GetCarInfoByDisplayName(string displayName);

                   /// <summary>
                   ///     Shared memory's VehicleModel holds at most 29 characters, so a longer display name arrives cut off
                   ///     (e.g. "Aston Martin Vantage AMR LMGT" for "Aston Martin Vantage AMR LMGT3").
                   /// </summary>
                   LmuCarInfo? FindBySharedMemoryVehicleModel(string vehicleModel);

                   /// <summary>
                   ///     Sadly in some cases the CarType field in LMU result files does not match the Display Name
                   /// </summary>
                   LmuCarInfo? GetCarInfoByResultCarType(string carType);

                   /// <summary>
                   ///     ModelId is the vehicle-folder segment from a setup file's //VEH= comment (e.g. "911GT3R_2024").
                   ///     Null for cars whose setup files haven't been seen yet.
                   /// </summary>
                   LmuCarInfo? FindByModelId(string modelId);
                   IReadOnlyCollection<LmuCarInfo> GetCarInfos();
                   ReadOnlyCollection<string> GetCarClasses();
                   ReadOnlyCollection<LmuCarInfo> GetCarInfosForClass(string carClass);
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Lmu.Core.Models;

               public record LmuCarInfo
               {
                   public string Category { get; init; }
                   public string Class { get; init; }
                   public string DisplayName { get; init; }
                   public string Engine { get; init; }
                   public int HeightMm { get; init; }
                   public int LengthMm { get; init; }
                   public string Manufacturer { get; init; }
                   // Litres, whole number.
                   public int MaxFuel { get; init; }
                   // Engine rev limiter RPM.
                   public int MaxRpm { get; init; }
                   public string ModelId { get; init; }
                   public int PowerBhp { get; init; }
                   public int PowerKw { get; init; }
                   public string ResultCarType { get; init; }
                   public string Transmission { get; init; }
                   public int WeightKg { get; init; }
                   public int WidthMm { get; init; }
                   // The model year as this catalog/game represents the car, not necessarily its real-world homologation year.
                   public int Year { get; init; }
               }

               """;
    }

    private static string BuildProviderSource(IEnumerable<CarInfo> cars)
    {
        var entryLines = cars.OrderBy(car => car.Manufacturer)
            .ThenBy(car => car.DisplayName)
            .Select(BuildEntryLine);

        return $$"""
                 using System.Collections.ObjectModel;
                 using SimRacingSdk.Lmu.Core.Abstractions;
                 using SimRacingSdk.Lmu.Core.Models;

                 namespace SimRacingSdk.Lmu.Core.Services;

                 public class LmuCarInfoProvider : ILmuCarInfoProvider
                 {
                     private const int MaxSharedMemoryVehicleModelLength = 29;

                     private static LmuCarInfoProvider? singletonInstance;

                     private readonly List<LmuCarInfo> cars =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     public static LmuCarInfoProvider Instance => singletonInstance ??= new LmuCarInfoProvider();

                     public LmuCarInfo? GetCarInfoByDisplayName(string displayName)
                     {
                         return this.cars.FirstOrDefault(
                             car => car.DisplayName.Equals(displayName, StringComparison.InvariantCultureIgnoreCase));
                     }

                     public LmuCarInfo? FindBySharedMemoryVehicleModel(string vehicleModel)
                     {
                         var exactMatch = this.GetCarInfoByDisplayName(vehicleModel);
                         if(exactMatch != null || vehicleModel.Length < MaxSharedMemoryVehicleModelLength)
                         {
                             return exactMatch;
                         }

                         return this.cars.FirstOrDefault(
                             car => car.DisplayName.StartsWith(vehicleModel, StringComparison.InvariantCultureIgnoreCase));
                     }

                     public LmuCarInfo? FindByModelId(string modelId)
                     {
                         return this.cars.FirstOrDefault(
                             car => car.ModelId != null && car.ModelId.Equals(modelId, StringComparison.InvariantCultureIgnoreCase));
                     }

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

                 """;
    }
}
