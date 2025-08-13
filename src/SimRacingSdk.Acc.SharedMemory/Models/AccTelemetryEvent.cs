namespace SimRacingSdk.Acc.SharedMemory.Models;

public class AccTelemetryEvent
{
    public AccTelemetryEvent(StaticData staticData)
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

    public override string ToString()
    {
        return
            $"ACC Telemetry Event: ACC Version: {this.AccVersion} Shared Memory Version: {this.SharedMemoryVersion} Track: {this.TrackId} Car: {this.CarId} Player: {this.PlayerName[..1]}. {this.PlayerSurname} Is Online: {this.IsOnline}";
    }
}