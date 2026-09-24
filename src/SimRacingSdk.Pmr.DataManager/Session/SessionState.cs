namespace SimRacingSdk.Pmr.DataManager.Session;

// Where Mike was working when the app last ran, so a rebuild/restart lands him back in the same
// place. Mutable on purpose - each page updates just its own part as the selection changes.
public class SessionState
{
    public string? CarsBrowseMode { get; set; }
    public string? CarsClass { get; set; }
    public string? CarsCarId { get; set; }
    public string? Page { get; set; }
    public string? SetupMapsCarId { get; set; }
    public int SetupMapsTabIndex { get; set; }
    public string? TracksBrowseMode { get; set; }
    public string? TracksContinent { get; set; }
    public string? TracksTrackId { get; set; }
    public double? WindowHeight { get; set; }
    public bool WindowIsMaximized { get; set; }
    public double? WindowLeft { get; set; }
    public double? WindowTop { get; set; }
    public double? WindowWidth { get; set; }
}
