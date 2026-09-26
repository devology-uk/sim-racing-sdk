namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// The known universe of fields across PMR's 5 real setup tabs, gathered from Mike's own in-game
// screenshots plus what the .hadron importer and a min/step/max .vset diff (AMC Javelin, Chevrolet
// Camaro 1969, Ford Mustang Boss 302 - the first three cars worked through by hand, 2026-09-23)
// confirmed - used to seed a car's SetupMap with the right Name/Section/Scope/Kind/Unit/RawKey so
// Mike only has to fill in Min/Max/Step/RawMin/RawMax/RawStep (or EnumValues) per car rather than
// retyping the same ~30 rows by hand for every one.
//
// Individual gear ratios are deliberately NOT here as named entries, even though they're a real,
// legitimate row to capture - Boss 302's 1st-4th Ratio show real, fixed numbers on screen despite
// having no backing raw key at all (same category as Camber - see the field-by-field note below),
// and the actual gear count varies per car (4-speed here, more on others), so there's no fixed set
// of names a shared template could sensibly offer. Add "1st Ratio"/"2nd Ratio"/etc. by hand per car
// as needed, each with Min == Max, Decimals 3 and no RawKey.
//
// RawKey confidence varies - see each field's own comment. Where a field wasn't adjustable on the
// cars checked so far (so its raw key never appeared in any of their saved files), the key here is
// inferred from the naming pattern of fields that were confirmed (e.g. "{Corner}-camber" from
// "{Corner}-toe") or from a .hadron channel:name (which the AMC diff confirmed IS the literal raw
// key wherever both were available - spring-platform, antirollbar and rack-steering-rate all
// matched exactly). Treat an inferred RawKey as a strong guess to verify, not a confirmed fact.
//
// Two things NOT modelled here, both found on Boss 302: (1) a raw key being present in a saved
// file doesn't mean the field is real - `abs`/`tractionControl` are written with a constant `0` on
// every save for this car, yet the screen shows "-" for both, not a number, so what the screen
// shows always wins over what the file contains; (2) not every raw value maps to its display value
// by a simple multiply - Final Drive displays a gear ratio computed as `fixedTeeth / raw` (a
// per-car constant, confirmed against three exact data points), so it's an Enum listing the exact
// on-screen ratios over its raw range rather than a Numeric with an approximate display Step.
public static class DefaultSetupFields
{
    public static IReadOnlyList<SetupFieldInfo> EngineAndDrivetrain { get; } =
    [
        Numeric("Fuel Level", "Fuel", SetupFieldScope.Single, "L", "fuelLevel", 0, SetupFieldQuantity.Volume, 1),
        // Shown as "56.0F / 44.0R" from a raw 0-1 front fraction.
        Numeric("Brake Bias", "Brakes", SetupFieldScope.Single, "%", "brake-bias", 1),
        Numeric("Brake Pressure", "Brakes", SetupFieldScope.Single, "%", "brake-pressure", 0),
        Numeric("Preload", "Differential", SetupFieldScope.Single, "N", "diff-preload", 0),
        Numeric("Clutches", "Differential", SetupFieldScope.Single, null, "diff-clutches", 0),
        Numeric("Power Ramp Angle", "Differential", SetupFieldScope.Single, "deg", "diff-ramp-power", 0),
        Numeric("Coast Ramp Angle", "Differential", SetupFieldScope.Single, "deg", "diff-ramp-coast", 0),
        Numeric("Engine Braking", "Engine/Electronics", SetupFieldScope.Single, null, null, 2),
        Numeric("Traction Control", "Engine/Electronics", SetupFieldScope.Single, null, "tractionControl", 0),
        Numeric("ABS", "Engine/Electronics", SetupFieldScope.Single, null, "abs", 0),
        Enum("Regen Mode", "Engine/Electronics", SetupFieldScope.Single, hasAutoOption: false, rawKey: null),
        Numeric("Regen Limit", "Engine/Electronics", SetupFieldScope.Single, "kW", null, 0),
        Numeric("Deploy Limit", "Engine/Electronics", SetupFieldScope.Single, "kW", null, 0),
        // Displays as a ratio ("3.500:1") computed from the raw integer via a per-car
        // fixedTeeth/raw formula, not the raw value itself - see this file's header comment.
        Enum("Final Drive", "Transmission", SetupFieldScope.Single, false, "final-drive")
    ];

    // Force Feedback is identical on every car (confirmed against the Barracuda, Javelin and Camaro
    // 1969 .vset trios), so these rows are complete and DefaultSetupFieldApplier overwrites this
    // section on every car rather than only adding what's missing. Display values equal raw values.
    // Strength/Alignment Boost/Load Boost share one raw range, so their keys were pinned by a
    // separate save clicking each a different number of times (Barracuda "-ffb.vset", 2026-09-26).
    public const string UniversalSection = "Force Feedback";

    public static IReadOnlyList<SetupFieldInfo> SteeringWheel { get; } =
    [
        ForceFeedback("Strength", "ffb-deep-gain", 0, 2, 0.04, 2),
        ForceFeedback("Rack Feel", "ffb-base-rack", 0, 1, 0.02, 2),
        ForceFeedback("Alignment Boost", "ffb-Mz-boost", 0, 2, 0.04, 2),
        ForceFeedback("Load Boost", "ffb-load-boost", 0, 2, 0.04, 2),
        ForceFeedback("Friction", "ffb-friction-boost", -0.1, 0.2, 0.01, 3),
        ForceFeedback("EQ Low", "ffb-eq-level-lo", 0, 2, 0.1, 1),
        ForceFeedback("EQ Mid", "ffb-eq-level-md", 0, 2, 0.1, 1),
        ForceFeedback("EQ High", "ffb-eq-level-hi", 0, 4, 0.1, 1),
        Numeric("Steering Rack Rate", "Steering", SetupFieldScope.Single, null, "rack-steering-rate", 2),
        // Displayed as raw / 1e7 and raw / 1e5 respectively.
        Numeric("Steering Stiffness", "Steering", SetupFieldScope.Single, null, "ffb-tierod-stiffness", 1),
        Numeric("Steering Damping", "Steering", SetupFieldScope.Single, null, "ffb-tierod-damping", 0)
    ];

    public static IReadOnlyList<SetupFieldInfo> Suspension { get; } =
    [
        // Not adjustable on AMC Javelin (absent from all 3 of its .vset files) - RawKey inferred
        // from the "{Corner}-toe" pattern, not directly confirmed yet.
        Numeric("Camber", "Springs & Dampers", SetupFieldScope.PerCorner, "deg", "{Corner}-camber", 2),
        Numeric("Toe-in", "Springs & Dampers", SetupFieldScope.PerCorner, "deg", "{Corner}-toe", 2),
        Numeric("Caster Offset", "Springs & Dampers", SetupFieldScope.PerCorner, "deg", null, 1),
        Numeric("Spring Rate", "Springs & Dampers", SetupFieldScope.PerCorner, "N/mm", "{Corner}-spring-rate", 0, SetupFieldQuantity.SpringRate, 0),
        Numeric("Slow Bump", "Springs & Dampers", SetupFieldScope.PerCorner, null, "{Corner}-slow-bump", 0),
        Numeric("Slow Rebound", "Springs & Dampers", SetupFieldScope.PerCorner, null, "{Corner}-slow-rebound", 0),
        // Not adjustable on AMC Javelin - RawKey inferred from the Slow Bump/Rebound pattern.
        Numeric("Fast Bump", "Springs & Dampers", SetupFieldScope.PerCorner, null, "{Corner}-fast-bump", 0),
        Numeric("Fast Rebound", "Springs & Dampers", SetupFieldScope.PerCorner, null, "{Corner}-fast-rebound", 0),
        Numeric("Bump Stop Length", "Springs & Dampers", SetupFieldScope.PerCorner, "mm", "{Corner}-bumpstop-size", 0, SetupFieldQuantity.Length, 1),
        Numeric("Bump Stop Spring Rate", "Springs & Dampers", SetupFieldScope.PerCorner, "N/mm", "{Corner}-bumpstop-stiffness", 0, SetupFieldQuantity.SpringRate, 0),
        Numeric("Ride Height Adjust", "Springs & Dampers", SetupFieldScope.PerCorner, "mm", "{Corner}-spring-platform", 0, SetupFieldQuantity.Length, 2),
        Numeric("Anti-Roll Bar", "Anti-Roll Bar & 3rd Spring", SetupFieldScope.FrontRear, "N/mm", "{Axle}-antirollbar", 0, SetupFieldQuantity.SpringRate, 0),
        // Not on AMC Javelin at all (no 3rd spring); RawKey taken from the .hadron channel:name
        // seen on LMDh-class cars, which the AMC diff confirmed is how a raw key is literally named.
        Numeric("3rd Spring Rate", "Anti-Roll Bar & 3rd Spring", SetupFieldScope.FrontRear, "N/mm", "{Axle}-third_spring-rate", 0, SetupFieldQuantity.SpringRate, 0),
        Numeric("3rd Spring Damping", "Anti-Roll Bar & 3rd Spring", SetupFieldScope.FrontRear, null, "{Axle}-third_spring-damping", 0)
    ];

    public static IReadOnlyList<SetupFieldInfo> TyresAndChassis { get; } =
    [
        Enum("Tyre Compound", "Wheels", SetupFieldScope.PerCorner, hasAutoOption: true, rawKey: "{Corner}-tire-choice"),
        Numeric("Tyre Pressure", "Wheels", SetupFieldScope.PerCorner, "bar", "{Corner}-tire-pressure", 2, SetupFieldQuantity.Pressure, 1),
        Numeric("Brake Duct", "Wheels", SetupFieldScope.PerCorner, null, null, 0),
        // Not seen directly - RawKey guessed from the .hadron channel:name convention ("R-wing-angle").
        Numeric("Front Wing Angle", "Aero", SetupFieldScope.Single, "deg", "F-wing-angle", 1),
        Numeric("Rear Wing Angle", "Aero", SetupFieldScope.Single, "deg", "R-wing-angle", 1)
    ];

    private static SetupFieldInfo Enum(string name, string section, SetupFieldScope scope, bool hasAutoOption, string? rawKey)
    {
        return new SetupFieldInfo
        {
            EnumValues = null,
            HasAutoOption = hasAutoOption,
            Kind = SetupFieldKind.Enum,
            Max = null,
            Min = null,
            Name = name,
            RawKey = rawKey,
            RawMax = null,
            RawMin = null,
            RawStep = null,
            Scope = scope,
            Section = section,
            Step = null,
            Unit = null
        };
    }

    private static SetupFieldInfo ForceFeedback(string name, string rawKey, double min, double max, double step, int decimals)
    {
        return Numeric(name, UniversalSection, SetupFieldScope.Single, null, rawKey, decimals) with
        {
            Max = max,
            Min = min,
            RawMax = max,
            RawMin = min,
            RawStep = step,
            Step = step
        };
    }

    private static SetupFieldInfo Numeric(
        string name,
        string section,
        SetupFieldScope scope,
        string? unit,
        string? rawKey,
        int decimals,
        SetupFieldQuantity quantity = SetupFieldQuantity.None,
        int? imperialDecimals = null)
    {
        return new SetupFieldInfo
        {
            Decimals = decimals,
            EnumValues = null,
            HasAutoOption = false,
            ImperialDecimals = imperialDecimals,
            Kind = SetupFieldKind.Numeric,
            Max = null,
            Min = null,
            Name = name,
            Quantity = quantity,
            RawKey = rawKey,
            RawMax = null,
            RawMin = null,
            RawStep = null,
            Scope = scope,
            Section = section,
            Step = null,
            Unit = unit
        };
    }
}
