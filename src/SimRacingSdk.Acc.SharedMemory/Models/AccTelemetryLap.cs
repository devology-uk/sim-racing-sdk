namespace SimRacingSdk.Acc.SharedMemory.Models;

public class AccTelemetryLap(StaticData staticData, GraphicsData graphicsData)
{
    public string CarId { get; } = staticData.CarModel;

    public int CompletedLaps { get; } = graphicsData.CompletedLaps;

    public string DriverName { get; } = $"{staticData.PlayerName[..1]}. {staticData.PlayerSurname}";

    public float FuelPerLap { get; } = graphicsData.FuelPerLap;

    public bool IsOnline { get; } = staticData.IsOnline;

    public float SessionTimeLeft { get; } = graphicsData.SessionTimeLeft;

    public string SessionType { get; } = graphicsData.SessionType.ToFriendlyName();

    public DateTime TimeStamp { get; } = graphicsData.TimeStamp;

    public string TrackId { get; } = staticData.Track;

    public override string ToString()
    {
        return
            $"Telemetry Lap: Track ID: {this.TrackId} Car ID: {this.CarId} Driver: {this.DriverName} Is Online: {this.IsOnline} Completed Laps: {this.CompletedLaps}";
    }
}