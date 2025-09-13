using SimRacingSdk.Acc.SharedMemory.Enums;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public record AccSharedMemorySession
{
    public AccSharedMemorySession(StaticData staticData, GraphicsData graphicsData)
    {
        this.SessionType = graphicsData.SessionType.ToFriendlyName();
        this.DurationMs = graphicsData.SessionTimeLeft;
    }

    public float DurationMs { get; }
    public string SessionType { get; }
}