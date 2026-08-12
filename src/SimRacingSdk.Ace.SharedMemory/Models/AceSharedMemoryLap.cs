namespace SimRacingSdk.Ace.SharedMemory.Models;

public record AceSharedMemoryLap
{
    private readonly AceStaticData staticData;
    private readonly AceGraphicsData graphicsData;

    public AceSharedMemoryLap(AceStaticData staticData,
        AceGraphicsData graphicsData,
        int? sector1Ms,
        int? sector2Ms)
    {
        this.staticData = staticData;
        this.graphicsData = graphicsData;
        this.CarModel = graphicsData.CarModel;
        this.CompletedLaps = graphicsData.TotalLapCount;
        this.DriverName = string.IsNullOrEmpty(graphicsData.DriverName)
            ? graphicsData.DriverSurname
            : $"{graphicsData.DriverName[..1]}. {graphicsData.DriverSurname}";
        this.IsOnline = staticData.IsOnline;
        this.LastLapTimeMs = graphicsData.LastLapTimeMs;
        this.Sector1Ms = sector1Ms;
        this.Sector2Ms = sector2Ms - sector1Ms;
        this.Sector3Ms = graphicsData.LastLapTimeMs - sector2Ms;
        this.SessionType = staticData.Session.ToString();
        this.TimeStamp = graphicsData.TimeStamp;
        this.TrackId = staticData.Track;
    }

    public string CarModel { get; }
    public int CompletedLaps { get; }
    public string DriverName { get; }
    public bool IsOnline { get; }
    public int LastLapTimeMs { get; }

    // Evo's shared memory has no official sector data - these are synthetic splits at 1/3 and
    // 2/3 of NormalizedPosition, not sectors the game itself reports.
    public int? Sector1Ms { get; }
    public int? Sector2Ms { get; }
    public int? Sector3Ms { get; }
    public string SessionType { get; }
    public DateTime TimeStamp { get; }
    public string TrackId { get; }
}
