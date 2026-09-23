namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public interface ISetupMapRepository
{
    PmrSetupMap? FindByCarId(string carId);
    void Save(PmrSetupMap setupMap);
}
