namespace SimRacingSdk.Acc.SharedMemory.Models;

public record AccSharedMemoryEvent
{
    public AccSharedMemoryEvent(StaticData staticData)
    {
        this.EventId = Guid.NewGuid();
        this.AccVersion = staticData.AccVersion;
        this.IsOnline = staticData.IsOnline;
        this.NumberOfCars = staticData.NumberOfCars;
        this.NumberOfSessions = staticData.NumberOfSessions;
        this.PlayerCarModel = staticData.CarModel;
        this.PlayerName = staticData.PlayerFirstName;
        this.PlayerNickname = staticData.PlayerNickname;
        this.PlayerSurname = staticData.PlayerLastName;
        this.SharedMemoryVersion = staticData.SharedMemoryVersion;
        this.TrackId = staticData.Track;
    }

    public Guid EventId { get; }
    public string AccVersion { get; }
    public bool IsOnline { get; }
    public int NumberOfCars { get; }
    public int NumberOfSessions { get; }
    public string PlayerCarModel { get; }
    public string PlayerName { get; }
    public string PlayerNickname { get; }
    public string PlayerSurname { get; }
    public string SharedMemoryVersion { get; }
    public string TrackId { get; }
}