using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

public partial class TrackLayoutEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private int gridSlots;

    [ObservableProperty]
    private double lengthMeters;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private int turns;

    public static TrackLayoutEditorViewModel From(TrackLayoutInfo layout)
    {
        return new TrackLayoutEditorViewModel
        {
            GridSlots = layout.GridSlots,
            LengthMeters = layout.LengthMeters,
            Name = layout.Name,
            Turns = layout.Turns
        };
    }

    public TrackLayoutInfo ToTrackLayoutInfo()
    {
        return new TrackLayoutInfo
        {
            GridSlots = this.GridSlots,
            LengthMeters = this.LengthMeters,
            Name = this.Name,
            Turns = this.Turns
        };
    }
}
