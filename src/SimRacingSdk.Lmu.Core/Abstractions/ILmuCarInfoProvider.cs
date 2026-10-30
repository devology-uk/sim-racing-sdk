using System.Collections.ObjectModel;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuCarInfoProvider
{
    LmuCarInfo? GetCarInfoByDisplayName(string displayName);

    /// <summary>
    ///     Shared memory's VehicleModel holds at most 29 characters, so a longer display name arrives cut off
    ///     (e.g. "Aston Martin Vantage AMR LMGT" for "Aston Martin Vantage AMR LMGT3").
    /// </summary>
    LmuCarInfo? FindBySharedMemoryVehicleModel(string vehicleModel);

    /// <summary>
    ///     Sadly in some cases the CarType field in LMU result files does not match the Display Name
    /// </summary>
    LmuCarInfo? GetCarInfoByResultCarType(string carType);

    /// <summary>
    ///     ModelId is the vehicle-folder segment from a setup file's //VEH= comment (e.g. "911GT3R_2024").
    ///     Null for cars whose setup files haven't been seen yet.
    /// </summary>
    LmuCarInfo? FindByModelId(string modelId);
    IReadOnlyCollection<LmuCarInfo> GetCarInfos();
    ReadOnlyCollection<string> GetCarClasses();
    ReadOnlyCollection<LmuCarInfo> GetCarInfosForClass(string carClass);
}
