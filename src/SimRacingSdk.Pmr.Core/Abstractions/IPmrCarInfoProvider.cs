using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Core.Abstractions;

public interface IPmrCarInfoProvider
{
    PmrCarInfo? FindByGameId(string gameId);
    PmrCarInfo? FindById(string id);
    IReadOnlyCollection<PmrCarInfo> GetCarInfos();
    IReadOnlyCollection<PmrCarInfo> GetCarInfosForManufacturer(string manufacturer);
    IReadOnlyCollection<string> GetManufacturers();
}
