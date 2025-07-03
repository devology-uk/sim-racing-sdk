namespace SimRacingSdk.Lmu.Core.Models;

public record LmuDriverLap
{
    public double EventTiming { get; set; }
    public double Fuel { get; set; }
    public double FuelUsed { get; set; }
    public double LapTime { get; set; }
    public int Number { get; set; }
    public int Position { get; set; }
    public double Sector1Time { get; set; }
    public double Sector2Time { get; set; }
    public double Sector3Time { get; set; }
    public double TopSpeed { get; set; }
    public string? TyreFrontCompound { get; set; }
    public string? TyreFrontLeftCompound { get; set; }
    public string? TyreFrontRightCompound { get; set; }
    public string? TyreRearCompound { get; set; }
    public string? TyreRearLeftCompound { get; set; }
    public string? TyreRearRightCompound { get; set; }
    public double TyreWearFrontLeft { get; set; }
    public double TyreWearFrontRight { get; set; }
    public double TyreWearRearLeft { get; set; }
    public double TyreWearRearRight { get; set; }
    public double VirtualEnergy { get; set; }
    public double VirtualEnergyUsed { get; set; }
}