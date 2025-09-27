using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuTrackInfoProvider
{
    LmuTrackInfo? GeTrackInfoByVenue(string venue);
    IReadOnlyCollection<LmuTrackInfo> GetTrackInfos();
}