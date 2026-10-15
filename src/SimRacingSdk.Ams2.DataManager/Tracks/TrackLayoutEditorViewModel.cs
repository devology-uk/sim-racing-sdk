using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.Ams2.DataManager.Tracks;

public partial class TrackLayoutEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private string? ams2TrackId;

    [ObservableProperty]
    private int corners;

    [ObservableProperty]
    private string? dlcPack;

    [ObservableProperty]
    private string grade = string.Empty;

    [ObservableProperty]
    private int lengthM;

    [ObservableProperty]
    private int maxGridSize;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string trackType = string.Empty;

    public static TrackLayoutEditorViewModel From(TrackLayoutInfo layout)
    {
        return new TrackLayoutEditorViewModel
        {
            Ams2TrackId = layout.Ams2TrackId,
            Corners = layout.Corners,
            DlcPack = layout.DlcPack,
            Grade = layout.Grade,
            LengthM = layout.LengthM,
            MaxGridSize = layout.MaxGridSize,
            Name = layout.Name,
            TrackType = layout.TrackType
        };
    }

    public TrackLayoutInfo ToTrackLayoutInfo()
    {
        return new TrackLayoutInfo
        {
            Ams2TrackId = NullIfBlank(this.Ams2TrackId),
            Corners = this.Corners,
            DlcPack = NullIfBlank(this.DlcPack),
            Grade = this.Grade,
            LengthM = this.LengthM,
            MaxGridSize = this.MaxGridSize,
            Name = this.Name,
            TrackType = this.TrackType
        };
    }

    private static string? NullIfBlank(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
