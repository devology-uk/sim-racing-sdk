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
    IReadOnlyCollection<LmuCarInfo> GetCarInfos();
    ReadOnlyCollection<string> GetCarClasses();
    ReadOnlyCollection<LmuCarInfo> GetCarInfosForClass(string carClass);
}