#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

// Per-car calibration data translating a setup file's raw "click" indices into real-world units.
// Loaded from SetupMaps/{carAccName}.json, falling back to SetupMaps/_default.json.
public class AccSetupMap
{
    public IndexedIntegerRange Abs { get; set; } = new();
    public IndexedIntegerRange AntiRollBarFront { get; set; } = new();
    public IndexedIntegerRange AntiRollBarRear { get; set; } = new();
    public IndexedDoubleRange BrakeBias { get; set; } = new();
    public IndexedIntegerRange BrakeCompoundFront { get; set; } = new();
    public IndexedIntegerRange BrakeCompoundRear { get; set; } = new();
    public IndexedIntegerRange BrakeDuctsFront { get; set; } = new();
    public IndexedIntegerRange BrakeDuctsRear { get; set; } = new();
    public IndexedIntegerRange BrakePower { get; set; } = new();
    public IndexedIntegerRange BumpFront { get; set; } = new();
    public IndexedIntegerRange BumpRear { get; set; } = new();
    public IndexedIntegerRange BumpStopRangeFront { get; set; } = new();
    public IndexedIntegerRange BumpStopRangeRear { get; set; } = new();
    public IndexedIntegerRange BumpStopRateFront { get; set; } = new();
    public IndexedIntegerRange BumpStopRateRear { get; set; } = new();
    public IndexedDoubleRange CamberFront { get; set; } = new();
    public IndexedDoubleRange CamberRear { get; set; } = new();
    public IndexedDoubleList Caster { get; set; } = new();
    public IndexedIntegerRange EcuMap { get; set; } = new();
    public IndexedIntegerRange FastBumpFront { get; set; } = new();
    public IndexedIntegerRange FastBumpRear { get; set; } = new();
    public IndexedIntegerRange FastReboundFront { get; set; } = new();
    public IndexedIntegerRange FastReboundRear { get; set; } = new();
    public IndexedIntegerRange Fuel { get; set; } = new();
    public IndexedIntegerRange Preload { get; set; } = new();
    public IndexedIntegerRange ReboundFront { get; set; } = new();
    public IndexedIntegerRange ReboundRear { get; set; } = new();
    public IndexedIntegerRange RideHeightFront { get; set; } = new();
    public IndexedIntegerRange RideHeightRear { get; set; } = new();
    public IndexedIntegerRange Splitter { get; set; } = new();
    public IndexedDoubleRange SteerRatio { get; set; } = new();
    public IndexedIntegerRange StrategyBrakeCompoundFront { get; set; } = new();
    public IndexedIntegerRange StrategyBrakeCompoundRear { get; set; } = new();
    public IndexedIntegerRange StrategyFuelToAdd { get; set; } = new();
    public IndexedIntegerRange StrategyPitStops { get; set; } = new();
    public IndexedStringList StrategyTyreCompound { get; set; } = new();
    public IndexedDoubleRange StrategyTyrePressureFrontLeft { get; set; } = new();
    public IndexedDoubleRange StrategyTyrePressureFrontRight { get; set; } = new();
    public IndexedDoubleRange StrategyTyrePressureRearLeft { get; set; } = new();
    public IndexedDoubleRange StrategyTyrePressureRearRight { get; set; } = new();
    public IndexedIntegerRange Tc { get; set; } = new();
    public IndexedIntegerRange Tc2 { get; set; } = new();
    public IndexedIntegerRange TelemetryLaps { get; set; } = new();
    public IndexedDoubleRange ToeFront { get; set; } = new();
    public IndexedDoubleRange ToeRear { get; set; } = new();
    public IndexedStringList TyreCompound { get; set; } = new();
    public IndexedDoubleRange TyrePressuresFront { get; set; } = new();
    public IndexedDoubleRange TyrePressuresRear { get; set; } = new();
    public IndexedIntegerRange TyreSet { get; set; } = new();
    public IndexedIntegerList WheelRateFront { get; set; } = new();
    public IndexedIntegerList WheelRateRear { get; set; } = new();
    public IndexedIntegerRange Wing { get; set; } = new();
}
