using System.Text.Json;
using System.Text.Json.Serialization;
using SimRacingSdk.Pmr.Setups.Abstractions;
using SimRacingSdk.Pmr.Setups.Models.Maps;

namespace SimRacingSdk.Pmr.Setups.Services;

public class PmrSetupMapProvider : IPmrSetupMapProvider
{
    private const string ResourceName = "SimRacingSdk.Pmr.Setups.pmr-setup-maps.json";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private static PmrSetupMapProvider? singletonInstance;

    private readonly Lazy<IReadOnlyList<PmrSetupMap>> setupMaps = new(LoadSetupMaps);

    public static PmrSetupMapProvider Instance => singletonInstance ??= new PmrSetupMapProvider();

    public PmrSetupMap? FindByCarId(string carId)
    {
        return this.setupMaps.Value.FirstOrDefault(map => string.Equals(map.CarId, carId, StringComparison.OrdinalIgnoreCase));
    }

    public PmrSetupMap? FindByVehicleGameId(string vehicleGameId)
    {
        return this.setupMaps.Value.FirstOrDefault(map => string.Equals(map.VehicleGameId, vehicleGameId, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<PmrSetupMap> GetSetupMaps()
    {
        return this.setupMaps.Value;
    }

    private static IReadOnlyList<PmrSetupMap> LoadSetupMaps()
    {
        using var stream = typeof(PmrSetupMapProvider).Assembly.GetManifestResourceStream(ResourceName)
                           ?? throw new InvalidOperationException($"Embedded resource {ResourceName} is missing.");
        return JsonSerializer.Deserialize<PmrSetupMapCatalog>(stream, SerializerOptions)?.Maps ?? [];
    }
}
