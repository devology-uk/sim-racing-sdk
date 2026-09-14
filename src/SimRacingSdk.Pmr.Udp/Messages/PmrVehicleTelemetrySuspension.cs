namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetrySuspension
{
    public IReadOnlyList<float> AverageLoadsNewtons { get; init; } = [];
    public float LoadBiasFraction { get; init; }
}
