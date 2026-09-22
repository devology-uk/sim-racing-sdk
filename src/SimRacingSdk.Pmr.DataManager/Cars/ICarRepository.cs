namespace SimRacingSdk.Pmr.DataManager.Cars;

public interface ICarRepository
{
    void Delete(string id);
    IReadOnlyList<CarInfo> GetAll();
    void Save(CarInfo car);
}
