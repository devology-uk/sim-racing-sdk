using SimRacingSdk.Pmr.Setups.Models.Maps;

namespace SimRacingSdk.Pmr.Setups.Abstractions;

public interface IPmrSetupMapProvider
{
    PmrSetupMap? FindByCarId(string carId);
    PmrSetupMap? FindByVehicleGameId(string vehicleGameId);
    IReadOnlyList<PmrSetupMap> GetSetupMaps();
}
