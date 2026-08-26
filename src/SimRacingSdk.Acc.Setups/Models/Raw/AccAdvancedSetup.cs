#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccAdvancedSetup
{
    public AccMechanicalBalance MechanicalBalance { get; set; } = new();
    public AccDampers Dampers { get; set; } = new();
    public AccAeroBalance AeroBalance { get; set; } = new();
    public AccDriveTrain DriveTrain { get; set; } = new();
}
