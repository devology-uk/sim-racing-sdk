namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetrySuspension
{
    public IReadOnlyList<float> AverageLoads { get; init; } = [];
    public float LoadBias { get; init; }
}
