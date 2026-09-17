using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Core.Abstractions;

public interface IPmrCarInfoProvider
{
    PmrCarInfo? FindById(string id);
    IReadOnlyCollection<PmrCarInfo> GetCarInfos();
    IReadOnlyCollection<PmrCarInfo> GetCarInfosForManufacturer(string manufacturer);
    IReadOnlyCollection<string> GetManufacturers();
}
