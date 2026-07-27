namespace SimRacingSdk.Ace.SharedMemory.Models;

public record AceSharedMemoryLap
{
    private readonly AceStaticData staticData;
    private readonly AceGraphicsData graphicsData;

    public AceSharedMemoryLap(AceStaticData staticData, AceGraphicsData graphicsData)
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
        this.SessionType = staticData.Session.ToString();
        this.TimeStamp = graphicsData.TimeStamp;
        this.TrackId = staticData.Track;
    }

    public string CarModel { get; }
    public int CompletedLaps { get; }
    public string DriverName { get; }
    public bool IsOnline { get; }
    public int LastLapTimeMs { get; }
    public string SessionType { get; }
    public DateTime TimeStamp { get; }
    public string TrackId { get; }
}
