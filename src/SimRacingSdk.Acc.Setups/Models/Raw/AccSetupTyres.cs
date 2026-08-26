#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccSetupTyres
{
    public int TyreCompound { get; set; }
    public int[] TyrePressure { get; set; } = new int[4];
}
