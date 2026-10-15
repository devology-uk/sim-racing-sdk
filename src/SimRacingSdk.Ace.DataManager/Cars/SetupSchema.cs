using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimRacingSdk.Ace.DataManager.Cars;

// Which setup controls a car exposes in ACE's own setup screens - genuinely per car, and recorded
// from those screens. Mutable so the car editor's checkboxes can bind straight to it; the editor
// works on a Clone, so a saved CarInfo never changes underneath the catalog.
public class SetupSchema
{
    private static readonly JsonSerializerOptions CloneOptions = new() { Converters = { new JsonStringEnumConverter() } };

    public AeroSchema Aero { get; set; } = new();
    public DampersSchema Dampers { get; set; } = new();
    public ElectronicsSchema Electronics { get; set; } = new();
    public FuelAndStrategySchema FuelAndStrategy { get; set; } = new();
    public SuspensionSchema Suspension { get; set; } = new();
    public TyresSchema Tyres { get; set; } = new();

    public SetupSchema Clone()
    {
        return JsonSerializer.Deserialize<SetupSchema>(JsonSerializer.Serialize(this, CloneOptions), CloneOptions)!;
    }
}

public class TyresSchema
{
    public bool Enabled { get; set; }
    public TyreAxleSchema Front { get; set; } = new();
    public TyreAxleSchema Rear { get; set; } = new();
}

public class TyreAxleSchema
{
    public bool HasCamber { get; set; }
    public bool HasCaster { get; set; }
    public bool HasPressure { get; set; }
    public bool HasToe { get; set; }
}

public class ElectronicsSchema
{
    public bool Enabled { get; set; }
    public bool HasAbs { get; set; }
    public bool HasEngineMap { get; set; }
    public bool HasErsDeploymentMap { get; set; }
    public bool HasErsHeatCharging { get; set; }
    public bool HasErsRechargeLevel { get; set; }
    public bool HasEsc { get; set; }
    public bool HasTelemetryLaps { get; set; }
    public bool HasTractionControl { get; set; }
    public bool HasTractionControl2 { get; set; }
    public bool HasTurboBoost { get; set; }
}

public class FuelAndStrategySchema
{
    public bool Enabled { get; set; }
    public bool HasFuel { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TyreCompoundMode TyreCompoundMode { get; set; } = TyreCompoundMode.None;
}

// None: no compound control at all. Unified: one control sets all four corners together (Audi).
// FrontOnly: one control, front axle only (some road-derived cars). FrontAndRear: independent front
// and rear controls (most racing-class cars).
public enum TyreCompoundMode
{
    None,
    Unified,
    FrontOnly,
    FrontAndRear
}

public class SuspensionSchema
{
    public bool Enabled { get; set; }
    public bool HasBrakeBias { get; set; }
    public bool HasBrakeTorqueMultiplier { get; set; }
    public bool HasBumpstopRange { get; set; }
    public bool HasBumpstopRate { get; set; }
    public bool HasDifferentialCoast { get; set; }
    public bool HasDifferentialPower { get; set; }
    public bool HasDifferentialPreload { get; set; }
    public bool HasFrontAntiRollBar { get; set; }
    public bool HasRearAntiRollBar { get; set; }
    public bool HasSteerRatio { get; set; }
    public bool HasWheelRate { get; set; }
}

public class DampersSchema
{
    public bool Enabled { get; set; }
    public bool HasFastBump { get; set; }
    public bool HasFastRebound { get; set; }
}

public class AeroSchema
{
    public bool Enabled { get; set; }
    public bool HasFrontRideHeight { get; set; }
    public bool HasFrontWingAngle { get; set; }
    public bool HasRearRideHeight { get; set; }
    public bool HasRearWingAngle { get; set; }
}
