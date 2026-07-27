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
        this.SessionId = Guid.NewGuid();
        this.SessionType = staticData.Session.ToString();
        this.TrackName = staticData.Track;
    }

    public float DurationMs { get; }
    public bool IsRunning { get; internal set; }
    public Guid SessionId { get; }
    public string SessionType { get; }
    public string TrackName { get; }
    public bool IsOnline { get; set; }
}
