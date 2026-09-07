namespace SimRacingSdk.Acc.Setups.Models;

public class AccDecodedSetup
{
    public AccDecodedTyres Tyres { get; init; } = new();
    public AccDecodedElectronics Electronics { get; init; } = new();
    public AccDecodedFuelAndStrategy FuelAndStrategy { get; init; } = new();
    public AccDecodedMechanicalGrip MechanicalGrip { get; init; } = new();
    public AccDecodedDampers Dampers { get; init; } = new();
    public AccDecodedAero Aero { get; init; } = new();
}

public class AccDecodedTyres
{
    public string Compound { get; init; } = "";
    public double PressureFl { get; init; }
    public double PressureFr { get; init; }
    public double PressureRl { get; init; }
    public double PressureRr { get; init; }
    public double CamberFl { get; init; }
    public double CamberFr { get; init; }
    public double CamberRl { get; init; }
    public double CamberRr { get; init; }
    public double ToeMmFl { get; init; }
    public double ToeMmFr { get; init; }
    public double ToeMmRl { get; init; }
    public double ToeMmRr { get; init; }
    public double CasterLf { get; init; }
    public double CasterRf { get; init; }
    public double SteerRatio { get; init; }
}

public class AccDecodedElectronics
{
    public int TractionControl1 { get; init; }
    public int TractionControl2 { get; init; }
    public int Abs { get; init; }
    public int EcuMap { get; init; }
    public int TelemetryLaps { get; init; }

    public int FuelMix { get; init; }
}

public class AccDecodedFuelAndStrategy
{
    public int FuelLitres { get; init; }
    public double FuelPerLap { get; init; }
    public int PlannedPitStops { get; init; }
    public int TyreSet { get; init; }
    public int FrontBrakePadCompound { get; init; }
    public int RearBrakePadCompound { get; init; }
}

public class AccDecodedMechanicalGrip
{
    public int AntiRollBarFront { get; init; }
    public int AntiRollBarRear { get; init; }
    public int WheelRateFl { get; init; }
    public int WheelRateFr { get; init; }
    public int WheelRateRl { get; init; }
    public int WheelRateRr { get; init; }
    public int BumpStopRateFl { get; init; }
    public int BumpStopRateFr { get; init; }
    public int BumpStopRateRl { get; init; }
    public int BumpStopRateRr { get; init; }
    public int BumpStopRangeFl { get; init; }
    public int BumpStopRangeFr { get; init; }
    public int BumpStopRangeRl { get; init; }
    public int BumpStopRangeRr { get; init; }

    // No calibration entry for brake torque in AccSetupMap - the raw setup value, unconverted.
    public int BrakeTorque { get; init; }
    public double BrakeBiasPercent { get; init; }
    public int DifferentialPreload { get; init; }
}

public class AccDecodedDampers
{
    public int BumpSlowFl { get; init; }
    public int BumpSlowFr { get; init; }
    public int BumpSlowRl { get; init; }
    public int BumpSlowRr { get; init; }
    public int BumpFastFl { get; init; }
    public int BumpFastFr { get; init; }
    public int BumpFastRl { get; init; }
    public int BumpFastRr { get; init; }
    public int ReboundSlowFl { get; init; }
    public int ReboundSlowFr { get; init; }
    public int ReboundSlowRl { get; init; }
    public int ReboundSlowRr { get; init; }
    public int ReboundFastFl { get; init; }
    public int ReboundFastFr { get; init; }
    public int ReboundFastRl { get; init; }
    public int ReboundFastRr { get; init; }
}

public class AccDecodedAero
{
    public int RideHeightFl { get; init; }
    public int RideHeightFr { get; init; }
    public int RideHeightRl { get; init; }
    public int RideHeightRr { get; init; }
    public int Splitter { get; init; }
    public int RearWing { get; init; }
    public int BrakeDuctFront { get; init; }
    public int BrakeDuctRear { get; init; }

    public double[] RodLength { get; init; } = new double[4];
}
