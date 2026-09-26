using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using SimRacingSdk.Pmr.DataManager.Cars;
using SimRacingSdk.Pmr.DataManager.Storage;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// Writes every car's setup map into SimRacingSdk.Pmr.Setups as one embedded JSON file, keyed by car
// Id and the game's vehicle id (the prefix of a saved .vset's name). Setup maps are data plus SDK
// logic rather than a catalog, so unlike Cars/Tracks they're shipped as data, not generated code.
public class PmrSetupMapsGenerator : IPmrSetupMapsGenerator
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    private readonly IDataPathProvider dataPathProvider;
    private readonly ISetupMapRepository setupMapRepository;

    public PmrSetupMapsGenerator(IDataPathProvider dataPathProvider, ISetupMapRepository setupMapRepository)
    {
        this.dataPathProvider = dataPathProvider;
        this.setupMapRepository = setupMapRepository;
    }

    public string Generate(IEnumerable<CarInfo> cars)
    {
        var catalog = new GeneratedSetupMapCatalog
        {
            Maps = cars.OrderBy(car => car.Id)
                       .Select(this.BuildEntry)
                       .OfType<GeneratedSetupMap>()
                       .ToList()
        };

        var outputPath = Path.Combine(this.dataPathProvider.GetRepoRoot(), "src", "SimRacingSdk.Pmr.Setups", "Data", "pmr-setup-maps.json");
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        File.WriteAllText(outputPath, JsonSerializer.Serialize(catalog, SerializerOptions));
        return outputPath;
    }

    private GeneratedSetupMap? BuildEntry(CarInfo car)
    {
        var map = this.setupMapRepository.FindByCarId(car.Id);
        if(map is null)
        {
            return null;
        }

        return new GeneratedSetupMap
        {
            CarId = car.Id,
            EngineAndDrivetrain = map.EngineAndDrivetrain,
            SteeringWheel = map.SteeringWheel,
            Suspension = map.Suspension,
            TyresAndChassis = map.TyresAndChassis,
            VehicleGameId = car.GameId
        };
    }

    // Shaped to match SimRacingSdk.Pmr.Setups' PmrSetupMapCatalog/PmrSetupMap.
    private record GeneratedSetupMapCatalog
    {
        public required List<GeneratedSetupMap> Maps { get; init; }
    }

    private record GeneratedSetupMap
    {
        public required string CarId { get; init; }
        public required List<SetupFieldInfo> EngineAndDrivetrain { get; init; }
        public required List<SetupFieldInfo> SteeringWheel { get; init; }
        public required List<SetupFieldInfo> Suspension { get; init; }
        public required List<SetupFieldInfo> TyresAndChassis { get; init; }
        public string? VehicleGameId { get; init; }
    }
}
