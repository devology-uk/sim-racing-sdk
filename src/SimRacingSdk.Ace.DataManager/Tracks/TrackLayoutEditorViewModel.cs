using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.Ace.DataManager.Tracks;

public partial class TrackLayoutEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private int corners;

    [ObservableProperty]
    private int lengthMeters;

    [ObservableProperty]
    private int maxPitSlot;

    [ObservableProperty]
    private string name = string.Empty;

    public static TrackLayoutEditorViewModel From(TrackLayoutInfo layout)
    {
        return new TrackLayoutEditorViewModel
        {
            Corners = layout.Corners,
            LengthMeters = layout.LengthMeters,
            MaxPitSlot = layout.MaxPitSlot,
            Name = layout.Name
        };
    }

    public TrackLayoutInfo ToTrackLayoutInfo()
    {
        return new TrackLayoutInfo
        {
            Corners = this.Corners,
            LengthMeters = this.LengthMeters,
            MaxPitSlot = this.MaxPitSlot,
            Name = this.Name
        };
    }
}
