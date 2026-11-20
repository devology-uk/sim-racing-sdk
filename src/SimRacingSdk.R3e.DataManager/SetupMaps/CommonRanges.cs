using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// Ranges that every car mapped so far shares, filled into an adjustable row whose range is still blank so that
// mapping a car only means checking them. Metric display values, brake bias as the front share.
public static class CommonRanges
{
    private static readonly SetupRange DamperClicks = new(1, 1, 18);

    private static readonly Dictionary<R3eSetupControl, SetupRange> Ranges = new()
    {
        [R3eSetupControl.BrakeBias] = new SetupRange(0, 0.5, 100),
        [R3eSetupControl.BrakePressure] = new SetupRange(80, 1, 100),
        [R3eSetupControl.FrontToeIn] = new SetupRange(-5, 1, 5),
        [R3eSetupControl.RearToeIn] = new SetupRange(-5, 1, 5),
        [R3eSetupControl.FrontSlowBump] = DamperClicks,
        [R3eSetupControl.FrontSlowRebound] = DamperClicks,
        [R3eSetupControl.FrontFastBump] = DamperClicks,
        [R3eSetupControl.FrontFastRebound] = DamperClicks,
        [R3eSetupControl.RearSlowBump] = DamperClicks,
        [R3eSetupControl.RearSlowRebound] = DamperClicks,
        [R3eSetupControl.RearFastBump] = DamperClicks,
        [R3eSetupControl.RearFastRebound] = DamperClicks,
        [R3eSetupControl.EngineBrakingReduction] = new SetupRange(1, 1, 20),
        [R3eSetupControl.FrontTyrePressure] = new SetupRange(140, 1, 250),
        [R3eSetupControl.RearTyrePressure] = new SetupRange(140, 1, 250),
        [R3eSetupControl.FrontCamber] = new SetupRange(-5, 0.1, 5),
        [R3eSetupControl.RearCamber] = new SetupRange(-5, 0.1, 5)
    };

    public static SetupRange? For(R3eSetupControl control)
    {
        return Ranges.GetValueOrDefault(control);
    }
}
