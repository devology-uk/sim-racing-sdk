namespace SimRacingSdk.Ace.SharedMemory.Models;

public record AceSharedMemorySession
{
    private readonly AceStaticData staticData;
    private readonly AceGraphicsData graphicsData;

    public AceSharedMemorySession(AceStaticData staticData, AceGraphicsData graphicsData)
    {
        this.staticData = staticData;
        this.graphicsData = graphicsData;
        this.DurationMs = graphicsData.SessionState.TimeLeftMs;
        this.IsOnline = staticData.IsOnline;
        this.IsRunning = true;
        // Evo has no per-session car count on the static page like Acc does - TotalDrivers
        // is a per-frame graphics field, so this reflects the count at session-start time.
        this.NumberOfCars = graphicsData.TotalDrivers;
        this.SessionId = Guid.NewGuid();
        this.SessionType = staticData.Session.ToString();
        this.TrackName = staticData.Track;
    }

    public float DurationMs { get; }
    public bool IsRunning { get; internal set; }
    public uint NumberOfCars { get; }
    public Guid SessionId { get; }
    public string SessionType { get; }
    public string TrackName { get; }
    public bool IsOnline { get; set; }
}
