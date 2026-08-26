#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccBasicSetup
{
    public AccSetupTyres Tyres { get; set; } = new();
    public AccSetupAlignment Alignment { get; set; } = new();
    public AccSetupElectronics Electronics { get; set; } = new();
    public AccSetupStrategy Strategy { get; set; } = new();
}
