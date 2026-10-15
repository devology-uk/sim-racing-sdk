using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.Lmu.DataManager.Tracks;

public partial class TrackLayoutEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private int corners;

    [ObservableProperty]
    private int lengthM;

    [ObservableProperty]
    private string name = string.Empty;

    // One raw track name per line.
    [ObservableProperty]
    private string rawTrackNames = string.Empty;

    public static TrackLayoutEditorViewModel From(TrackLayoutInfo layout)
    {
        return new TrackLayoutEditorViewModel
        {
            Corners = layout.Corners,
            LengthM = layout.LengthM,
            Name = layout.Name,
            RawTrackNames = string.Join(Environment.NewLine, layout.RawTrackNames)
        };
    }

    public TrackLayoutInfo ToTrackLayoutInfo()
    {
        return new TrackLayoutInfo
        {
            Corners = this.Corners,
            LengthM = this.LengthM,
            Name = this.Name,
            RawTrackNames = this.RawTrackNames.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToList()
        };
    }
}
