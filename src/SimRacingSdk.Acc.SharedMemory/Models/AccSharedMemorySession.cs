namespace SimRacingSdk.Acc.SharedMemory.Models;

public record AccSharedMemorySession
{
    private readonly StaticData staticData;
    private readonly GraphicsData graphicsData;

    public AccSharedMemorySession(StaticData staticData,
        GraphicsData graphicsData)
    {
        this.staticData = staticData;
        this.graphicsData = graphicsData;
        this.DurationMs = graphicsData.SessionTimeLeft;
        this.IsOnline = staticData.IsOnline;
        this.IsRunning = true;
        this.NumberOfCars = staticData.NumberOfCars;
        this.SessionId = Guid.NewGuid();
        this.SessionType = graphicsData.SessionType.ToFriendlyName();
        this.TrackName = staticData.Track;
    }

    public float DurationMs { get; }
    public bool IsRunning { get; internal set; }
    public int NumberOfCars { get; }
    public Guid SessionId { get; }
    public string SessionType { get; }
    public string TrackName { get; }
    public bool IsOnline { get; set; }
}