namespace SimRacingSdk.Lmu.Rest.Models;

// strategy/pitstop-estimate: the time each part of the planned stop would take, in seconds.
public record LmuRestPitStopEstimate
{
    public double BrakeDucts { get; init; }
    public double Brakes { get; init; }
    public double Damage { get; init; }
    public double DriverSwap { get; init; }
    public double Fuel { get; init; }
    public double Penalties { get; init; }
    public double Tires { get; init; }
    public double Total { get; init; }
    public double Ve { get; init; }
}
