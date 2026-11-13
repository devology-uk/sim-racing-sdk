namespace SimRacingSdk.R3e.Setups.Models;

// Every control the game's setup screen can show. Every car uses this same set; cars differ only in which
// controls they have and which of those can be adjusted. Per-wheel controls are one control per axle,
// because the game links left and right (Shift + adjust changes one side only).
public enum R3eSetupControl
{
    BrakeBias,
    BrakePressure,
    FrontTyreCompound,
    RearTyreCompound,

    FrontAntiRollBar,
    FrontToeIn,
    FrontDownforce,
    FrontSpringRate,
    FrontTyrePressure,
    FrontRideHeight,
    FrontCamber,
    FrontSlowBump,
    FrontSlowRebound,
    FrontFastBump,
    FrontFastRebound,
    FrontThirdSpringRate,
    FrontThirdSpringSlowBump,
    FrontThirdSpringSlowRebound,
    FrontThirdSpringFastBump,
    FrontThirdSpringFastRebound,

    RearAntiRollBar,
    RearToeIn,
    RearWingAngle,
    RearSpringRate,
    RearTyrePressure,
    RearRideHeight,
    RearCamber,
    RearSlowBump,
    RearSlowRebound,
    RearFastBump,
    RearFastRebound,
    RearThirdSpringRate,
    RearThirdSpringSlowBump,
    RearThirdSpringSlowRebound,
    RearThirdSpringFastBump,
    RearThirdSpringFastRebound,

    AbsFront,
    AbsRear,
    TractionControlCut,
    TractionControlSlip,
    TractionControlLateral,

    RevLimit,
    EngineBrakingReduction,
    EngineMap,
    GearRatios,
    FinalGear,
    DifferentialPower,
    DifferentialCoast,
    DifferentialPreload
}
