namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public class DefaultSetupFieldApplier : IDefaultSetupFieldApplier
{
    public int AddMissingFields(PmrSetupMap map)
    {
        return AddMissing(map.EngineAndDrivetrain, DefaultSetupFields.EngineAndDrivetrain)
               + AddMissing(map.SteeringWheel, DefaultSetupFields.SteeringWheel)
               + AddMissing(map.Suspension, DefaultSetupFields.Suspension)
               + AddMissing(map.TyresAndChassis, DefaultSetupFields.TyresAndChassis);
    }

    public int ApplyForceFeedback(PmrSetupMap map)
    {
        var forceFeedbackFields = DefaultSetupFields.SteeringWheel
            .Where(field => field.Section == DefaultSetupFields.UniversalSection)
            .ToList();

        return OverwriteExisting(map.SteeringWheel, forceFeedbackFields) + AddMissing(map.SteeringWheel, forceFeedbackFields);
    }

    private static int AddMissing(List<SetupFieldInfo> existing, IReadOnlyList<SetupFieldInfo> defaults)
    {
        var existingNames = existing.Select(field => field.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missing = defaults.Where(field => !existingNames.Contains(field.Name)).ToList();
        existing.AddRange(missing);
        return missing.Count;
    }

    private static int OverwriteExisting(List<SetupFieldInfo> existing, IReadOnlyList<SetupFieldInfo> replacements)
    {
        var changed = 0;
        foreach(var replacement in replacements)
        {
            var index = existing.FindIndex(field => string.Equals(field.Name, replacement.Name, StringComparison.OrdinalIgnoreCase));
            if(index < 0 || IsSameField(existing[index], replacement))
            {
                continue;
            }

            existing[index] = replacement;
            changed++;
        }

        return changed;
    }

    // Records compare List<string> by reference, so EnumValues is compared by content.
    private static bool IsSameField(SetupFieldInfo first, SetupFieldInfo second)
    {
        return first with { EnumValues = null } == second with { EnumValues = null }
               && (first.EnumValues ?? []).SequenceEqual(second.EnumValues ?? []);
    }
}
