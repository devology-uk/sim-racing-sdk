using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.Setups;

// The one fixed list of setup controls, in the order the game's setup screen shows them.
public static class R3eSetupControls
{
    private const string Clicks = "clicks";
    private const string Degrees = "deg";
    private const string Multiplier = "x";
    private const string NewtonsPerMillimetre = "N/mm";
    private const string Percent = "%";

    private static readonly Dictionary<R3eSetupControl, R3eSetupControlDefinition> DefinitionsByControl;

    static R3eSetupControls()
    {
        All = BuildAll();
        DefinitionsByControl = All.ToDictionary(definition => definition.Control);
    }

    public static IReadOnlyList<R3eSetupControlDefinition> All { get; }

    public static R3eSetupControlDefinition DefinitionOf(R3eSetupControl control)
    {
        return DefinitionsByControl[control];
    }

    private static List<R3eSetupControlDefinition> BuildAll()
    {
        return
        [
            ..General(),
            ..Axle(R3eSetupTab.Front),
            ..Axle(R3eSetupTab.Rear),
            ..Electronics(),
            ..Driveline()
        ];
    }

    private static IEnumerable<R3eSetupControlDefinition> General()
    {
        const string section = "General";
        yield return Number(R3eSetupControl.BrakeBias, R3eSetupTab.General, section, "Brake Bias", Percent);
        yield return Number(R3eSetupControl.BrakePressure, R3eSetupTab.General, section, "Brake Pressure", Percent);
        yield return Choice(R3eSetupControl.FrontTyreCompound, section, "Front Tyre Compound");
        yield return Choice(R3eSetupControl.RearTyreCompound, section, "Rear Tyre Compound");
    }

    private static IEnumerable<R3eSetupControlDefinition> Axle(R3eSetupTab tab)
    {
        var isFront = tab == R3eSetupTab.Front;

        yield return Number(isFront ? R3eSetupControl.FrontAntiRollBar : R3eSetupControl.RearAntiRollBar,
            tab, "Shared", "Anti-Roll Bar", NewtonsPerMillimetre);
        yield return Number(isFront ? R3eSetupControl.FrontToeIn : R3eSetupControl.RearToeIn,
            tab, "Shared", "Toe In", Degrees);
        yield return isFront
            ? Number(R3eSetupControl.FrontDownforce, tab, "Shared", "Downforce", string.Empty)
            : Number(R3eSetupControl.RearWingAngle, tab, "Shared", "Wing Angle", Degrees);

        yield return Number(isFront ? R3eSetupControl.FrontSpringRate : R3eSetupControl.RearSpringRate,
            tab, "Wheels", "Spring Rate", NewtonsPerMillimetre, R3eSetupQuantity.SpringRate);
        yield return Number(isFront ? R3eSetupControl.FrontTyrePressure : R3eSetupControl.RearTyrePressure,
            tab, "Wheels", "Tyre Pressure", "kPa", R3eSetupQuantity.Pressure);
        yield return Number(isFront ? R3eSetupControl.FrontRideHeight : R3eSetupControl.RearRideHeight,
            tab, "Wheels", "Ride Height", "cm", R3eSetupQuantity.Length);
        yield return Number(isFront ? R3eSetupControl.FrontCamber : R3eSetupControl.RearCamber,
            tab, "Wheels", "Camber", Degrees);
        yield return Number(isFront ? R3eSetupControl.FrontSlowBump : R3eSetupControl.RearSlowBump,
            tab, "Wheels", "Slow Bump", Clicks);
        yield return Number(isFront ? R3eSetupControl.FrontSlowRebound : R3eSetupControl.RearSlowRebound,
            tab, "Wheels", "Slow Rebound", Clicks);
        yield return Number(isFront ? R3eSetupControl.FrontFastBump : R3eSetupControl.RearFastBump,
            tab, "Wheels", "Fast Bump", Clicks);
        yield return Number(isFront ? R3eSetupControl.FrontFastRebound : R3eSetupControl.RearFastRebound,
            tab, "Wheels", "Fast Rebound", Clicks);

        // The game shows the rear third spring in N/mm even in Imperial, unlike the front.
        var thirdSpringQuantity = isFront ? R3eSetupQuantity.SpringRate : R3eSetupQuantity.None;
        yield return Number(isFront ? R3eSetupControl.FrontThirdSpringRate : R3eSetupControl.RearThirdSpringRate,
            tab, "Third Spring", "Spring Rate", NewtonsPerMillimetre, thirdSpringQuantity);
        yield return Number(isFront ? R3eSetupControl.FrontThirdSpringSlowBump : R3eSetupControl.RearThirdSpringSlowBump,
            tab, "Third Spring", "Slow Bump", Clicks);
        yield return Number(isFront ? R3eSetupControl.FrontThirdSpringSlowRebound : R3eSetupControl.RearThirdSpringSlowRebound,
            tab, "Third Spring", "Slow Rebound", Clicks);
        yield return Number(isFront ? R3eSetupControl.FrontThirdSpringFastBump : R3eSetupControl.RearThirdSpringFastBump,
            tab, "Third Spring", "Fast Bump", Clicks);
        yield return Number(isFront ? R3eSetupControl.FrontThirdSpringFastRebound : R3eSetupControl.RearThirdSpringFastRebound,
            tab, "Third Spring", "Fast Rebound", Clicks);
    }

    private static IEnumerable<R3eSetupControlDefinition> Electronics()
    {
        yield return LevelTable(R3eSetupControl.AbsFront, "ABS", "ABS Front", Multiplier);
        yield return LevelTable(R3eSetupControl.AbsRear, "ABS", "ABS Rear", Multiplier);
        yield return LevelTable(R3eSetupControl.TractionControlCut, "Traction Control", "TC Cut", Percent);
        yield return LevelTable(R3eSetupControl.TractionControlSlip, "Traction Control", "TC Slip", Multiplier);
        yield return LevelTable(R3eSetupControl.TractionControlLateral, "Traction Control", "TC Lateral", Multiplier);
    }

    private static IEnumerable<R3eSetupControlDefinition> Driveline()
    {
        const R3eSetupTab tab = R3eSetupTab.Driveline;
        yield return Number(R3eSetupControl.RevLimit, tab, "Engine", "Rev Limit", "rpm");
        yield return Number(R3eSetupControl.EngineBrakingReduction, tab, "Engine", "Engine Braking Reduction", Clicks);
        yield return Number(R3eSetupControl.EngineMap, tab, "Engine Map", "Map #", string.Empty);
        yield return new R3eSetupControlDefinition
        {
            Control = R3eSetupControl.GearRatios,
            Kind = R3eSetupValueKind.GearSet,
            MetricUnit = string.Empty,
            Name = "Gear Ratios",
            Section = "Gearbox Ratios",
            Tab = tab
        };
        yield return Number(R3eSetupControl.FinalGear, tab, "Gearbox Ratios", "Final Gear", string.Empty);
        yield return Number(R3eSetupControl.DifferentialPower, tab, "Differential", "Power", Percent);
        yield return Number(R3eSetupControl.DifferentialCoast, tab, "Differential", "Coast", Percent);
        yield return Number(R3eSetupControl.DifferentialPreload, tab, "Differential", "Preload", "Nm");
    }

    private static R3eSetupControlDefinition Choice(R3eSetupControl control, string section, string name)
    {
        return new R3eSetupControlDefinition
        {
            Control = control,
            Kind = R3eSetupValueKind.Choice,
            MetricUnit = string.Empty,
            Name = name,
            Section = section,
            Tab = R3eSetupTab.General
        };
    }

    private static R3eSetupControlDefinition LevelTable(R3eSetupControl control, string section, string name, string unit)
    {
        return new R3eSetupControlDefinition
        {
            Control = control,
            Kind = R3eSetupValueKind.LevelTable,
            MetricUnit = unit,
            Name = name,
            Section = section,
            Tab = R3eSetupTab.Electronics
        };
    }

    private static R3eSetupControlDefinition Number(
        R3eSetupControl control,
        R3eSetupTab tab,
        string section,
        string name,
        string unit,
        R3eSetupQuantity quantity = R3eSetupQuantity.None)
    {
        return new R3eSetupControlDefinition
        {
            Control = control,
            Kind = R3eSetupValueKind.Number,
            MetricUnit = unit,
            Name = name,
            Quantity = quantity,
            Section = section,
            Tab = tab
        };
    }
}
