using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuGameDataProvider
{
    LmuCarInfo? GetCarInfoByDisplayName(string displayName);

    /// <summary>
    ///     Sadly in some cases the CarType field in LMU result files does not match the Display Name
    /// </summary>
    LmuCarInfo? GetCarInfoByResultCarType(string carType);

    IReadOnlyCollection<LmuCarInfo> GetCarInfos();
    LmuSettings GetSettings();
    IReadOnlyCollection<LmuTrackInfo> GetTrackInfos();
    LmuTrackInfo? GeTrackInfoByVenue(string venue);
    IList<string> ListResultFiles();
}