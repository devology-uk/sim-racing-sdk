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

    public static TrackLayoutEditorViewModel From(TrackLayoutInfo layout)
    {
        return new TrackLayoutEditorViewModel
        {
            Corners = layout.Corners,
            LengthM = layout.LengthM,
            Name = layout.Name
        };
    }

    public TrackLayoutInfo ToTrackLayoutInfo()
    {
        return new TrackLayoutInfo
        {
            Corners = this.Corners,
            LengthM = this.LengthM,
            Name = this.Name
        };
    }
}
