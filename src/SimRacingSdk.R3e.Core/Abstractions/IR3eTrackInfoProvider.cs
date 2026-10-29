using SimRacingSdk.R3e.Core.Models;

namespace SimRacingSdk.R3e.Core.Abstractions;

public interface IR3eTrackInfoProvider
{
    R3eTrackInfo? FindByGameId(int trackGameId);

    /// <summary>
    ///     Layout ids are unique across every track, so a layout id alone (shared memory's LayoutId)
    ///     identifies both.
    /// </summary>
    (R3eTrackInfo Track, R3eTrackLayoutInfo Layout)? FindByLayoutGameId(int layoutGameId);

    R3eTrackInfo? FindByName(string name);
    IReadOnlyCollection<R3eTrackInfo> GetTrackInfos();
}
