using System.IO;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Generation;
using SimRacingSdk.DataManager.Shared.Storage;
using static SimRacingSdk.DataManager.Shared.Generation.CSharpLiteral;

namespace SimRacingSdk.R3e.DataManager.Cars;

// Regenerates SimRacingSdk.R3e.Core's whole car-catalog surface - model, interface and provider -
// from the cars captured here, so nothing in Core has to pre-exist for a car to reach it.
public class R3eCarProviderGenerator : ICatalogGenerator<CarInfo>
{
    private readonly IDataPathProvider dataPathProvider;
    private readonly DataManagerOptions options;

    public R3eCarProviderGenerator(IDataPathProvider dataPathProvider, DataManagerOptions options)
    {
        this.dataPathProvider = dataPathProvider;
        this.options = options;
    }

    public string Generate(IReadOnlyCollection<CarInfo> entities)
    {
        var coreRoot = this.dataPathProvider.GetProjectFolder(this.options.CoreProjectName);

        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "R3eCarInfo.cs"), BuildModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Abstractions", "IR3eCarInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "R3eCarInfoProvider.cs");
        GeneratedSource.Write(providerPath, BuildProviderSource(entities));
        return providerPath;
    }

    private static string BuildEntryLine(CarInfo car)
    {
        return "        new() { "
               + $"Class = {Text(car.Class)}, "
               + $"Drivetrain = {Text(car.Drivetrain)}, "
               + $"EngineCapacityLitres = {NullableNumber(car.EngineCapacityLitres)}, "
               + $"EngineLayout = {Text(car.EngineLayout)}, "
               + $"GameId = {car.GameId}, "
               + $"Manufacturer = {Text(car.Manufacturer)}, "
               + $"Name = {Text(car.Name)}, "
               + $"NotInGameSelector = {(car.NotInGameSelector ? "true" : "false")}, "
               + $"PowerBhp = {NullableNumber(car.PowerBhp)}, "
               + $"WeightWithDriverKg = {NullableNumber(car.WeightWithDriverKg)} "
               + "},";
    }

    private static string BuildInterfaceSource()
    {
        return """
               using SimRacingSdk.R3e.Core.Models;

               namespace SimRacingSdk.R3e.Core.Abstractions;

               public interface IR3eCarInfoProvider
               {
                   R3eCarInfo? FindByGameId(int gameId);

                   /// <summary>
                   ///     Several cars share a name across classes and seasons, so a name alone (e.g. from a setup
                   ///     file's name) can match more than one.
                   /// </summary>
                   IReadOnlyCollection<R3eCarInfo> FindByName(string name);

                   IReadOnlyCollection<string> GetCarClasses();
                   IReadOnlyCollection<R3eCarInfo> GetCarInfos();
                   IReadOnlyCollection<R3eCarInfo> GetCarInfosForClass(string carClass);
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.R3e.Core.Models;

               public record R3eCarInfo
               {
                   public string Class { get; init; }
                   // As the game shows it, e.g. "RWD".
                   public string Drivetrain { get; init; }
                   // The game shows the engine as "1.8L Inline-4": capacity 1.8, layout "Inline-4".
                   public double? EngineCapacityLitres { get; init; }
                   public string EngineLayout { get; init; }
                   // The game's own model id, as shared memory and results files report it.
                   public int GameId { get; init; }
                   public string Manufacturer { get; init; }
                   public string Name { get; init; }
                   // Known to the game but not offered in its car selector (esports, safety cars, retired copies).
                   public bool NotInGameSelector { get; init; }
                   public int? PowerBhp { get; init; }
                   public int? WeightWithDriverKg { get; init; }
               }

               """;
    }

    private static string BuildProviderSource(IEnumerable<CarInfo> cars)
    {
        var entryLines = cars.OrderBy(car => car.Manufacturer)
            .ThenBy(car => car.Name)
            .ThenBy(car => car.GameId)
            .Select(BuildEntryLine);

        return $$"""
                 using SimRacingSdk.R3e.Core.Abstractions;
                 using SimRacingSdk.R3e.Core.Models;

                 namespace SimRacingSdk.R3e.Core;

                 public class R3eCarInfoProvider : IR3eCarInfoProvider
                 {
                     private static R3eCarInfoProvider? singletonInstance;

                     private readonly List<R3eCarInfo> cars =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     private readonly Dictionary<int, R3eCarInfo> carsByGameId;

                     public R3eCarInfoProvider()
                     {
                         this.carsByGameId = this.cars.ToDictionary(car => car.GameId);
                     }

                     public static R3eCarInfoProvider Instance => singletonInstance ??= new R3eCarInfoProvider();

                     public R3eCarInfo? FindByGameId(int gameId)
                     {
                         return this.carsByGameId.GetValueOrDefault(gameId);
                     }

                     public IReadOnlyCollection<R3eCarInfo> FindByName(string name)
                     {
                         return this.cars.Where(car => car.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                    .ToList()
                                    .AsReadOnly();
                     }

                     public IReadOnlyCollection<string> GetCarClasses()
                     {
                         return this.cars.Select(car => car.Class)
                                    .Distinct()
                                    .Order()
                                    .ToList()
                                    .AsReadOnly();
                     }

                     public IReadOnlyCollection<R3eCarInfo> GetCarInfos()
                     {
                         return this.cars.AsReadOnly();
                     }

                     public IReadOnlyCollection<R3eCarInfo> GetCarInfosForClass(string carClass)
                     {
                         return this.cars.Where(car => car.Class == carClass)
                                    .ToList()
                                    .AsReadOnly();
                     }
                 }

                 """;
    }
}
