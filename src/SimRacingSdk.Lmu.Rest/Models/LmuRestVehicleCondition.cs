namespace SimRacingSdk.Lmu.Rest.Models;

// garage/getVehicleCondition: the player's car only. Fuel is litres; conditions are 1 = new, per wheel FL, FR, RL, RR.
public record LmuRestVehicleCondition
{
    public IReadOnlyList<double> BrakeCondition { get; init; } = [];
    public double Fuel { get; init; }
    public double FuelCapacity { get; init; }
    public IReadOnlyList<double> SuspensionDamage { get; init; } = [];
    public IReadOnlyList<double> TireCondition { get; init; } = [];
    public double VehicleDamage { get; init; }
}
