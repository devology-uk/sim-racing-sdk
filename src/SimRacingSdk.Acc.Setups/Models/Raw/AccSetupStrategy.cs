#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccSetupStrategy
{
    public int Fuel { get; set; }
    public int NPitStops { get; set; }
    public int TyreSet { get; set; }
    public int FrontBrakePadCompound { get; set; }
    public int RearBrakePadCompound { get; set; }
    public List<AccPitStrategy> PitStrategy { get; set; } = [];
    public double FuelPerLap { get; set; }
}
