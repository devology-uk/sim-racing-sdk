using SimRacingSdk.R3e.DataManager.Cars;
using SimRacingSdk.R3e.Setups;
using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// What a car nobody has mapped yet starts with: the controls every car seen so far (NSU TTS, Tatuus F4,
// BMW M4 GT3 DTM, DMD P20) could adjust are Adjustable, the rest of the common screen is Fixed, and the
// sections only some cars have (third spring, electronics, engine map, power/coast diff) are Not Fitted.
// Mapping a car is then mostly flipping the few rows that differ.
public static class NewCarDefaults
{
    private static readonly HashSet<R3eSetupControl> AdjustableControls =
    [
        R3eSetupControl.BrakeBias,
        R3eSetupControl.BrakePressure,
        R3eSetupControl.FrontAntiRollBar,
        R3eSetupControl.FrontToeIn,
        R3eSetupControl.FrontSpringRate,
        R3eSetupControl.FrontTyrePressure,
        R3eSetupControl.FrontRideHeight,
        R3eSetupControl.FrontCamber,
        R3eSetupControl.FrontSlowBump,
        R3eSetupControl.FrontSlowRebound,
        R3eSetupControl.RearAntiRollBar,
        R3eSetupControl.RearToeIn,
        R3eSetupControl.RearSpringRate,
        R3eSetupControl.RearTyrePressure,
        R3eSetupControl.RearRideHeight,
        R3eSetupControl.RearCamber,
        R3eSetupControl.RearSlowBump,
        R3eSetupControl.RearSlowRebound,
        R3eSetupControl.EngineBrakingReduction,
        R3eSetupControl.FinalGear,
        R3eSetupControl.DifferentialPreload
    ];

    private static readonly HashSet<R3eSetupControl> FixedControls =
    [
        R3eSetupControl.FrontTyreCompound,
        R3eSetupControl.RearTyreCompound,
        R3eSetupControl.FrontDownforce,
        R3eSetupControl.FrontFastBump,
        R3eSetupControl.FrontFastRebound,
        R3eSetupControl.RearWingAngle,
        R3eSetupControl.RearFastBump,
        R3eSetupControl.RearFastRebound,
        R3eSetupControl.RevLimit,
        R3eSetupControl.GearRatios
    ];

    public static CapturedSetupMap For(CarInfo car)
    {
        return new CapturedSetupMap
        {
            CarGameId = car.GameId,
            CarName = car.Name,
            Controls = R3eSetupControls.All
                .Select(definition => new R3eSetupControlSetting
                {
                    Control = definition.Control,
                    Availability = AvailabilityOf(definition.Control)
                })
                .ToList()
        };
    }

    private static R3eSetupControlAvailability AvailabilityOf(R3eSetupControl control)
    {
        if(AdjustableControls.Contains(control))
        {
            return R3eSetupControlAvailability.Adjustable;
        }

        return FixedControls.Contains(control) ? R3eSetupControlAvailability.Fixed : R3eSetupControlAvailability.NotFitted;
    }
}
