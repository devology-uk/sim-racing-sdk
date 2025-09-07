namespace SimRacingSdk.Acc.SharedMemory.Models;

public record AccSharedMemoryEvent
{
    public AccSharedMemoryEvent(StaticData staticData)
    {
        this.AccVersion = staticData.AccVersion;
        this.CarId = staticData.CarModel;
        this.IsOnline = staticData.IsOnline;
        this.MaxRpm = staticData.MaxRpm;
        this.PlayerName = staticData.PlayerName;
        this.PlayerNickname = staticData.PlayerNickname;
        this.PlayerSurname = staticData.PlayerSurname;
        this.SharedMemoryVersion = staticData.SharedMemoryVersion;
        this.TrackId = staticData.Track;
    }

    public string AccVersion { get; }
    public string CarId { get; }
    public bool IsOnline { get; }
    public int MaxRpm { get; }
    public string PlayerName { get; }
    public string PlayerNickname { get; }
    public string PlayerSurname { get; }
    public string SharedMemoryVersion { get; }
    public string TrackId { get; }
}