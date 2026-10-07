using SimRacingSdk.Pmr.DataManager.Cars;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public interface IPmrSetupMapsGenerator
{
    PmrSetupMapsGenerationResult Generate(IEnumerable<CarInfo> cars);
}

public record PmrSetupMapsGenerationResult(string OutputPath, int MapsWritten);
