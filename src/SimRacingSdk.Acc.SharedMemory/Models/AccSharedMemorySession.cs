namespace SimRacingSdk.Acc.SharedMemory.Models;

public record AccSharedMemorySession
{
    public AccSharedMemorySession(StaticData staticData,
        GraphicsData graphicsData,
        Guid eventId,
        bool isRunning = true)
    {
        this.DurationMs = graphicsData.SessionTimeLeft;
        this.EventId = eventId;
        this.IsOnline = staticData.IsOnline;
        this.IsRunning = isRunning;
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
    public Guid EventId { get; set; }
    public bool IsOnline { get; set; }
}