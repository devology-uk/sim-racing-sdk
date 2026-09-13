namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryInput
{
    public float Steering { get; init; }
    public float Accelerator { get; init; }
    public float Brake { get; init; }
    public float Clutch { get; init; }
    public float Handbrake { get; init; }
    public int Gear { get; init; }
}
