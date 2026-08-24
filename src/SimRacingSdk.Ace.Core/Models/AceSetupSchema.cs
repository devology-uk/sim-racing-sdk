namespace SimRacingSdk.Ace.Core.Models;

// Describes which setup tabs and settings a car actually exposes in ACE's UI - Early Access means
// this genuinely varies per car (not just BOP-disabled fields, some tabs are entirely absent, some
// underlying values exist in the setup file but aren't surfaced as editable controls at all yet).
public record AceSetupSchema
{
    public AceTyresSchema Tyres { get; init; } = new();
    public AceElectronicsSchema Electronics { get; init; } = new();
    public AceFuelAndStrategySchema FuelAndStrategy { get; init; } = new();
    public AceSuspensionSchema Suspension { get; init; } = new();
    public AceDampersSchema Dampers { get; init; } = new();
    public AceAeroSchema Aero { get; init; } = new();
}

public record AceTyresSchema
{
    public bool Enabled { get; init; }
    public AceTyreAxleSchema Front { get; init; } = new();
    public AceTyreAxleSchema Rear { get; init; } = new();
}

// Front-left/front-right (and rear-left/rear-right) have always shown the same set of available
// fields as each other in every car checked so far, only the values differ - so axle-level
// granularity is enough, no need to model all four corners independently.
public record AceTyreAxleSchema
{
    public bool HasPressure { get; init; }
    public bool HasCamber { get; init; }
    public bool HasToe { get; init; }
    public bool HasCaster { get; init; }
}

public record AceElectronicsSchema
{
    public bool Enabled { get; init; }
    public bool HasTractionControl { get; init; }
    public bool HasTractionControl2 { get; init; }
    public bool HasAbs { get; init; }
    public bool HasEsc { get; init; }
    public bool HasTurboBoost { get; init; }
    public bool HasEngineMap { get; init; }
    public bool HasErsDeploymentMap { get; init; }
    public bool HasErsRechargeLevel { get; init; }
    public bool HasErsHeatCharging { get; init; }
    public bool HasTelemetryLaps { get; init; }
}

public record AceFuelAndStrategySchema
{
    public bool Enabled { get; init; }
    public bool HasFuel { get; init; }
    public AceTyreCompoundMode TyreCompoundMode { get; init; } = AceTyreCompoundMode.None;
}

// None: no compound control at all. Unified: one control sets all four corners together (Audi).
// FrontOnly: one control, front axle only, no rear control (some road-derived cars). FrontAndRear:
// independent front and rear controls (most racing-class cars).
public enum AceTyreCompoundMode
{
    None,
    Unified,
    FrontOnly,
    FrontAndRear
}

public record AceSuspensionSchema
{
    public bool Enabled { get; init; }
    public bool HasWheelRate { get; init; }
    public bool HasFrontAntiRollBar { get; init; }
    public bool HasRearAntiRollBar { get; init; }
    public bool HasBrakeBias { get; init; }
    public bool HasBrakeTorqueMultiplier { get; init; }
    public bool HasSteerRatio { get; init; }
    public bool HasBumpstopRate { get; init; }
    public bool HasBumpstopRange { get; init; }
    public bool HasDifferentialPreload { get; init; }
    public bool HasDifferentialCoast { get; init; }
    public bool HasDifferentialPower { get; init; }
}

public record AceDampersSchema
{
    public bool Enabled { get; init; }
    public bool HasFastBump { get; init; }
    public bool HasFastRebound { get; init; }
}

public record AceAeroSchema
{
    public bool Enabled { get; init; }
    public bool HasFrontRideHeight { get; init; }
    public bool HasRearRideHeight { get; init; }
    public bool HasFrontWingAngle { get; init; }
    public bool HasRearWingAngle { get; init; }
}
