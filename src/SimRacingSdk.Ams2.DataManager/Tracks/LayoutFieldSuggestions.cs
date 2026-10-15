using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ams2.DataManager.Tracks;

// The values already in use for the layout fields that repeat across the catalog.
public class LayoutFieldSuggestions
{
    public LayoutFieldSuggestions(IReadOnlyCollection<TrackInfo> tracks)
    {
        var layouts = tracks.SelectMany(track => track.Layouts).ToList();
        this.DlcPacks = DistinctValues.Of(layouts, layout => layout.DlcPack);
        this.Grades = DistinctValues.Of(layouts, layout => layout.Grade);
        this.TrackTypes = DistinctValues.Of(layouts, layout => layout.TrackType);
    }

    public IReadOnlyList<string> DlcPacks { get; }
    public IReadOnlyList<string> Grades { get; }
    public IReadOnlyList<string> TrackTypes { get; }
}
