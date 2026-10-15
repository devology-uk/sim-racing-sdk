using System.Collections.ObjectModel;
using SimRacingSdk.Ams2.Core.Models;

namespace SimRacingSdk.Ams2.Core.Abstractions;

public interface IAms2TrackInfoProvider
{
    // Takes the track location AMS2's shared memory reports, as well as a ShortName.
    Ams2TrackInfo? FindTrackByShortName(string shortName);
    ReadOnlyCollection<Ams2TrackInfo> GetTrackInfos();
    ReadOnlyCollection<string> GetTrackNames();
}
