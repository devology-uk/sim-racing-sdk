namespace SimRacingSdk.Acc.SharedMemory.Models;

public record AccSharedMemoryLap
{
    private GraphicsData graphicsData;
    private StaticData staticData;

    public AccSharedMemoryLap(StaticData staticData, GraphicsData graphicsData)
    {
        this.staticData = staticData;
        this.graphicsData = graphicsData;
        this.CarId = staticData.CarModel;
        this.CompletedLaps = graphicsData.CompletedLaps;
        this.DriverName = $"{staticData.PlayerFirstName[..1]}. {staticData.PlayerLastName}";
        this.FuelPerLap = graphicsData.FuelPerLap;
        this.IsOnline = staticData.IsOnline;
        this.SessionTimeLeft = graphicsData.SessionTimeLeft;
        this.SessionType = graphicsData.SessionType.ToFriendlyName();
        this.TimeStamp = graphicsData.TimeStamp;
        this.TrackId = staticData.Track;
    }

    public string CarId { get; }
    public int CompletedLaps { get; }
    public string DriverName { get; }
    public float FuelPerLap { get; }
    public bool IsOnline { get; }
    public float SessionTimeLeft { get; }
    public string SessionType { get; }
    public DateTime TimeStamp { get; }
    public string TrackId { get; }
}