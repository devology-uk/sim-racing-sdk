using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// Letters are spaced ("A B S") so the recogniser expects them spelled out.
public static class VoiceTargets
{
    private static readonly string[] DamperNames = ["SlowBump", "SlowRebound", "FastBump", "FastRebound"];

    private static readonly R3eSetupControl[] Dampers = DamperNames
        .SelectMany(damper => new[] { Control("Front", damper), Control("Rear", damper) })
        .ToArray();

    public static IReadOnlyList<VoiceTarget> All { get; } =
    [
        ..General(),
        ..Axle("Front"),
        ..Axle("Rear"),
        ..Groups(),
        ..Electronics(),
        ..Driveline()
    ];

    public static IEnumerable<string> AllPhrases => All.SelectMany(target => target.Phrases);

    // The longest phrase wins, so "front third slow bump" isn't taken for "front third ..." something shorter.
    public static (VoiceTarget Target, int WordCount)? MatchStartOf(IReadOnlyList<string> words)
    {
        return All
            .SelectMany(target => target.Phrases.Select(phrase => (Target: target, Words: phrase.Split(' '))))
            .Where(candidate => StartsWith(words, candidate.Words))
            .OrderByDescending(candidate => candidate.Words.Length)
            .Select(candidate => ((VoiceTarget Target, int WordCount)?)(candidate.Target, candidate.Words.Length))
            .FirstOrDefault();
    }

    private static IEnumerable<VoiceTarget> General()
    {
        yield return Target("Brake bias", R3eSetupControl.BrakeBias, "brake bias");
        yield return Target("Brake pressure", R3eSetupControl.BrakePressure, "brake pressure");
        yield return Target("Front compound", R3eSetupControl.FrontTyreCompound, "front compound");
        yield return Target("Rear compound", R3eSetupControl.RearTyreCompound, "rear compound");
        yield return Group("Compounds", [R3eSetupControl.FrontTyreCompound, R3eSetupControl.RearTyreCompound], "compounds", "compound");
    }

    private static IEnumerable<VoiceTarget> Axle(string axle)
    {
        var spoken = axle.ToLowerInvariant();

        yield return Target($"{axle} anti-roll bar", Control(axle, "AntiRollBar"), $"{spoken} anti roll bar", $"{spoken} roll bar");
        yield return Target($"{axle} toe", Control(axle, "ToeIn"), $"{spoken} toe", $"{spoken} toe in");
        yield return axle == "Front"
            ? Target("Front downforce", R3eSetupControl.FrontDownforce, "front downforce")
            : Target("Rear wing", R3eSetupControl.RearWingAngle, "rear wing", "rear wing angle");
        yield return Target($"{axle} spring rate", Control(axle, "SpringRate"), $"{spoken} spring", $"{spoken} springs", $"{spoken} spring rate");
        yield return Target($"{axle} tyre pressure", Control(axle, "TyrePressure"), $"{spoken} tyre pressure", $"{spoken} pressure");
        yield return Target($"{axle} ride height", Control(axle, "RideHeight"), $"{spoken} ride height");
        yield return Target($"{axle} camber", Control(axle, "Camber"), $"{spoken} camber");
        yield return Target($"{axle} slow bump", Control(axle, "SlowBump"), $"{spoken} slow bump");
        yield return Target($"{axle} slow rebound", Control(axle, "SlowRebound"), $"{spoken} slow rebound");
        yield return Target($"{axle} fast bump", Control(axle, "FastBump"), $"{spoken} fast bump");
        yield return Target($"{axle} fast rebound", Control(axle, "FastRebound"), $"{spoken} fast rebound");
        yield return Group($"{axle} dampers", DampersOf(axle, string.Empty), $"{spoken} dampers");

        yield return Target($"{axle} third spring", Control(axle, "ThirdSpringRate"), $"{spoken} third spring");
        yield return Target($"{axle} third slow bump", Control(axle, "ThirdSpringSlowBump"), $"{spoken} third slow bump");
        yield return Target($"{axle} third slow rebound", Control(axle, "ThirdSpringSlowRebound"), $"{spoken} third slow rebound");
        yield return Target($"{axle} third fast bump", Control(axle, "ThirdSpringFastBump"), $"{spoken} third fast bump");
        yield return Target($"{axle} third fast rebound", Control(axle, "ThirdSpringFastRebound"), $"{spoken} third fast rebound");
        yield return Group($"{axle} third dampers", DampersOf(axle, "ThirdSpring"), $"{spoken} third dampers");
    }

    private static IEnumerable<VoiceTarget> Groups()
    {
        var thirdSprings = new[] { "Front", "Rear" }
            .SelectMany(axle => DampersOf(axle, "ThirdSpring").Prepend(Control(axle, "ThirdSpringRate")))
            .ToArray();

        yield return Group("Third springs", thirdSprings, "third springs");
        yield return Group("Dampers", Dampers, "dampers");
        yield return Group("Tyre pressures", [R3eSetupControl.FrontTyrePressure, R3eSetupControl.RearTyrePressure], "tyre pressures");
    }

    private static IEnumerable<VoiceTarget> Electronics()
    {
        yield return Target("ABS front", R3eSetupControl.AbsFront, "A B S front");
        yield return Target("ABS rear", R3eSetupControl.AbsRear, "A B S rear");
        yield return Target("TC cut", R3eSetupControl.TractionControlCut, "T C cut");
        yield return Target("TC slip", R3eSetupControl.TractionControlSlip, "T C slip");
        yield return Target("TC lateral", R3eSetupControl.TractionControlLateral, "T C lateral");
        yield return Group("ABS", [R3eSetupControl.AbsFront, R3eSetupControl.AbsRear], "A B S");
        yield return Group("Traction control",
            [R3eSetupControl.TractionControlCut, R3eSetupControl.TractionControlSlip, R3eSetupControl.TractionControlLateral],
            "traction control", "T C");
        yield return Group("Electronics",
            [
                R3eSetupControl.AbsFront, R3eSetupControl.AbsRear, R3eSetupControl.TractionControlCut,
                R3eSetupControl.TractionControlSlip, R3eSetupControl.TractionControlLateral
            ],
            "electronics");
    }

    private static IEnumerable<VoiceTarget> Driveline()
    {
        yield return Target("Rev limit", R3eSetupControl.RevLimit, "rev limit", "rev limiter");
        yield return Target("Engine braking", R3eSetupControl.EngineBrakingReduction, "engine braking");
        yield return Target("Engine map", R3eSetupControl.EngineMap, "engine map");
        yield return Target("Gears", R3eSetupControl.GearRatios, "gears", "gear ratios");
        yield return Target("Final gear", R3eSetupControl.FinalGear, "final gear", "final drive");
        yield return Target("Diff power", R3eSetupControl.DifferentialPower, "diff power");
        yield return Target("Diff coast", R3eSetupControl.DifferentialCoast, "diff coast");
        yield return Target("Diff preload", R3eSetupControl.DifferentialPreload, "diff preload", "preload");
        yield return Group("Diff power and coast", [R3eSetupControl.DifferentialPower, R3eSetupControl.DifferentialCoast],
            "diff power and coast", "diff ramps");
    }

    private static R3eSetupControl Control(string axle, string name)
    {
        return Enum.Parse<R3eSetupControl>($"{axle}{name}");
    }

    private static R3eSetupControl[] DampersOf(string axle, string prefix)
    {
        return DamperNames
            .Select(damper => Control(axle, $"{prefix}{damper}"))
            .ToArray();
    }

    private static VoiceTarget Group(string description, R3eSetupControl[] controls, params string[] phrases)
    {
        return new VoiceTarget(phrases, description, controls);
    }

    private static bool StartsWith(IReadOnlyList<string> words, string[] phraseWords)
    {
        return words.Count >= phraseWords.Length
               && phraseWords.Select((word, index) => string.Equals(word, words[index], StringComparison.OrdinalIgnoreCase))
                   .All(matches => matches);
    }

    private static VoiceTarget Target(string description, R3eSetupControl control, params string[] phrases)
    {
        return new VoiceTarget(phrases, description, [control]);
    }
}
