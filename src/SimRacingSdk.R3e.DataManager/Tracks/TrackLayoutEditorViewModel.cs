using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.R3e.DataManager.Tracks;

public partial class TrackLayoutEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private int gameId;

    [ObservableProperty]
    private int gridSize;

    [ObservableProperty]
    private double? lengthKm;

    [ObservableProperty]
    private string name = string.Empty;

    public static TrackLayoutEditorViewModel From(TrackLayoutInfo layout)
    {
        return new TrackLayoutEditorViewModel
        {
            GameId = layout.GameId,
            GridSize = layout.GridSize,
            LengthKm = layout.LengthKm,
            Name = layout.Name
        };
    }

    public TrackLayoutInfo ToTrackLayoutInfo()
    {
        return new TrackLayoutInfo
        {
            GameId = this.GameId,
            GridSize = this.GridSize,
            LengthKm = this.LengthKm,
            Name = this.Name
        };
    }
}
