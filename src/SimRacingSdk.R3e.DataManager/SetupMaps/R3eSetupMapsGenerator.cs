using System.IO;
using SimRacingSdk.DataManager.Shared.Generation;
using SimRacingSdk.DataManager.Shared.Storage;
using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// Writes every validated map into SimRacingSdk.R3e.Setups as one embedded JSON file. Setup maps ship as data,
// not generated code, as PMR's do.
public class R3eSetupMapsGenerator : IR3eSetupMapsGenerator
{
    private const string SetupsProjectName = "SimRacingSdk.R3e.Setups";

    private readonly IDataPathProvider dataPathProvider;
    private readonly ISetupMapRepository setupMapRepository;

    public R3eSetupMapsGenerator(IDataPathProvider dataPathProvider, ISetupMapRepository setupMapRepository)
    {
        this.dataPathProvider = dataPathProvider;
        this.setupMapRepository = setupMapRepository;
    }

    public R3eSetupMapsGenerationResult Generate()
    {
        var catalog = new R3eSetupMapCatalog
        {
            Maps = this.setupMapRepository.GetAll()
                .Where(map => map.Validated)
                .OrderBy(map => map.CarGameId)
                .Select(ToSdkMap)
                .ToList()
        };

        var outputPath = Path.Combine(this.dataPathProvider.GetProjectFolder(SetupsProjectName), "Data", "r3e-setup-maps.json");
        GeneratedSource.Write(outputPath, SetupMapJson.Serialize(catalog));
        return new R3eSetupMapsGenerationResult(outputPath, catalog.Maps.Count);
    }

    private static R3eSetupMap ToSdkMap(CapturedSetupMap capturedMap)
    {
        return new R3eSetupMap
        {
            CarGameId = capturedMap.CarGameId,
            Controls = capturedMap.Controls
        };
    }
}
