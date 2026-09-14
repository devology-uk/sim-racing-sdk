namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryInput
{
    public float SteeringFraction { get; init; }
    public float AcceleratorFraction { get; init; }
    public float BrakeFraction { get; init; }
    public float ClutchFraction { get; init; }
    public float HandbrakeFraction { get; init; }
    public int Gear { get; init; }
}
