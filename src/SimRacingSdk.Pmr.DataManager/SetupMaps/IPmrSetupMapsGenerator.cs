using SimRacingSdk.Pmr.DataManager.Cars;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public interface IPmrSetupMapsGenerator
{
    // Returns the path of the file written.
    string Generate(IEnumerable<CarInfo> cars);
}
