using System.Collections.ObjectModel;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Abstractions;

public interface ILmuTrackInfoProvider
{
    LmuTrackInfo? FindTrackByShortName(string shortName);
    LmuTrackLayoutInfo? FindLayoutByRawTrackName(string rawTrackName);
    LmuTrackInfo? GeTrackInfoByVenue(string venue);
    IReadOnlyCollection<LmuTrackInfo> GetTrackInfos();
    ReadOnlyCollection<string> GetTrackNames();
}