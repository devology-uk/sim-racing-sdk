namespace SimRacingSdk.Pmr.DataManager.Tracks;

public interface IPmrTrackProviderGenerator
{
    // Writes SimRacingSdk.Pmr.Core's PmrTrackInfoProvider.cs, Models/PmrTrackInfo.cs,
    // Models/PmrTrackLayoutInfo.cs and Abstractions/IPmrTrackInfoProvider.cs from the given tracks -
    // the DataManager owns Core's whole track-catalog surface, not just the provider's data, so
    // nothing there has to pre-exist. Returns the path written to for PmrTrackInfoProvider.cs.
    string Generate(IEnumerable<TrackInfo> tracks);
}
