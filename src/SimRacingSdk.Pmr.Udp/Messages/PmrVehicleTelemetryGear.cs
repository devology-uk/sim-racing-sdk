namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryGear
{
    public float UpshiftRpm { get; init; }
    public float DownshiftRpm { get; init; }
}
