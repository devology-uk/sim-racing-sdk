namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public interface ISetupMapRepository
{
    PmrSetupMap? FindByCarId(string carId);
    void ReassignCar(string oldCarId, string newCarId);
    void Save(PmrSetupMap setupMap);
}
