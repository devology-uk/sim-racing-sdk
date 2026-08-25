using System.Collections.ObjectModel;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuCarInfoProvider
{
    LmuCarInfo? GetCarInfoByDisplayName(string displayName);

    /// <summary>
    ///     Sadly in some cases the CarType field in LMU result files does not match the Display Name
    /// </summary>
    LmuCarInfo? GetCarInfoByResultCarType(string carType);

    /// <summary>
    ///     ModelId is the vehicle-folder segment from a setup file's //VEH= comment (e.g. "911GT3R_2024").
    ///     Only populated for cars whose setup files have actually been seen - most rows have no ModelId yet.
    /// </summary>
    LmuCarInfo? FindByModelId(string modelId);
    IReadOnlyCollection<LmuCarInfo> GetCarInfos();
    ReadOnlyCollection<string> GetCarClasses();
    ReadOnlyCollection<LmuCarInfo> GetCarInfosForClass(string carClass);
}