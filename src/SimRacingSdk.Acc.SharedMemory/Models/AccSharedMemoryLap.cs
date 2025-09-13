namespace SimRacingSdk.Acc.SharedMemory.Models;

public record AccSharedMemoryLap(StaticData staticData, GraphicsData graphicsData)
{
    public string CarId { get; } = staticData.CarModel;
    public int CompletedLaps { get; } = graphicsData.CompletedLaps;
    public string DriverName { get; } = $"{staticData.PlayerFirstName[..1]}. {staticData.PlayerLastName}";
    public float FuelPerLap { get; } = graphicsData.FuelPerLap;
    public bool IsOnline { get; } = staticData.IsOnline;
    public float SessionTimeLeft { get; } = graphicsData.SessionTimeLeft;
    public string SessionType { get; } = graphicsData.SessionType.ToFriendlyName();
    public DateTime TimeStamp { get; } = graphicsData.TimeStamp;
    public string TrackId { get; } = staticData.Track;
}