using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Core.Abstractions;

public interface IPmrCarInfoProvider
{
    PmrCarInfo? FindById(string id);
    PmrCarInfo? FindByVehiclePath(string vehiclePath);
    IReadOnlyCollection<PmrCarInfo> GetCarInfos();
    IReadOnlyCollection<PmrCarInfo> GetCarInfosForManufacturer(string manufacturer);
    IReadOnlyCollection<string> GetManufacturers();
}
