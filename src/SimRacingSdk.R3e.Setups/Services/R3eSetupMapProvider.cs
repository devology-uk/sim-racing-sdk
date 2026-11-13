using System.Text.Json;
using System.Text.Json.Serialization;
using SimRacingSdk.R3e.Setups.Abstractions;
using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.Setups.Services;

public class R3eSetupMapProvider : IR3eSetupMapProvider
{
    private const string ResourceName = "SimRacingSdk.R3e.Setups.r3e-setup-maps.json";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private static R3eSetupMapProvider? singletonInstance;

    private readonly Lazy<IReadOnlyList<R3eSetupMap>> setupMaps = new(LoadSetupMaps);

    public static R3eSetupMapProvider Instance => singletonInstance ??= new R3eSetupMapProvider();

    public R3eSetupMap? FindByCarGameId(int carGameId)
    {
        return this.setupMaps.Value.FirstOrDefault(map => map.CarGameId == carGameId);
    }

    public IReadOnlyList<R3eSetupMap> GetSetupMaps()
    {
        return this.setupMaps.Value;
    }

    private static IReadOnlyList<R3eSetupMap> LoadSetupMaps()
    {
        using var stream = typeof(R3eSetupMapProvider).Assembly.GetManifestResourceStream(ResourceName)
                           ?? throw new InvalidOperationException($"Embedded resource {ResourceName} is missing.");
        return JsonSerializer.Deserialize<R3eSetupMapCatalog>(stream, SerializerOptions)?.Maps ?? [];
    }
}
