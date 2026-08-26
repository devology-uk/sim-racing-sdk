#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccPitStrategy
{
    public int FuelToAdd { get; set; }
    public AccSetupTyres Tyres { get; set; } = new();
    public int TyreSet { get; set; }
    public int FrontBrakePadCompound { get; set; }
    public int RearBrakePadCompound { get; set; }
}
