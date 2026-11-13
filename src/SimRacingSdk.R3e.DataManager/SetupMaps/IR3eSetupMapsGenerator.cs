namespace SimRacingSdk.R3e.DataManager.SetupMaps;

public interface IR3eSetupMapsGenerator
{
    R3eSetupMapsGenerationResult Generate();
}

public record R3eSetupMapsGenerationResult(string OutputPath, int MapsWritten);
