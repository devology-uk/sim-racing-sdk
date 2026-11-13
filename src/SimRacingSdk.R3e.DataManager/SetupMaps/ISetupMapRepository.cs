namespace SimRacingSdk.R3e.DataManager.SetupMaps;

public interface ISetupMapRepository
{
    CapturedSetupMap? FindByCarGameId(int carGameId);
    IReadOnlyList<CapturedSetupMap> GetAll();
    void Save(CapturedSetupMap setupMap);
}
