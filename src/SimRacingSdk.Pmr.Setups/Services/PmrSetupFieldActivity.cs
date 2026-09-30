using System.Globalization;
using SimRacingSdk.Pmr.Setups.Models.Files;
using SimRacingSdk.Pmr.Setups.Models.Maps;
using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Services;

// Marks the values the game greys out because another setting disables them, e.g. Regen Limit
// while Regen Mode isn't Full.
internal class PmrSetupFieldActivity
{
    private const string AlternativesSeparator = " or ";
    private const double RawValueTolerance = 1e-6;

    private readonly IReadOnlyList<PmrSetupFieldMap> allFields;
    private readonly PmrSetupFile setup;

    public PmrSetupFieldActivity(PmrSetupMap map, PmrSetupFile setup)
    {
        this.allFields = [..map.TyresAndChassis, ..map.Suspension, ..map.EngineAndDrivetrain, ..map.SteeringWheel];
        this.setup = setup;
    }

    public PmrSetupValueView Apply(PmrSetupFieldMap field, PmrSetupValueView value)
    {
        var condition = field.EnabledWhen;
        if(condition is null || this.IsMet(condition, value.Position))
        {
            return value;
        }

        return value with
        {
            InactiveReason = this.ReasonFor(condition),
            IsActive = false
        };
    }

    private static string DescribeRawValue(PmrSetupFieldMap? controllingField, double rawValue)
    {
        if(controllingField is { Kind: PmrSetupFieldKind.Enum, EnumValues: { } enumValues })
        {
            var index = (int)Math.Round((rawValue - (controllingField.RawMin ?? 0)) / (controllingField.RawStep ?? 1));
            if(index >= 0 && index < enumValues.Count)
            {
                return PmrSetupValueFormatter.FormatEnumValue(enumValues[index], controllingField);
            }
        }

        return rawValue.ToString(CultureInfo.InvariantCulture);
    }

    // Nothing says the setting is disabled when the setup doesn't record the controlling one.
    private bool IsMet(PmrSetupFieldCondition condition, PmrSetupPosition position)
    {
        var controllingValue = this.setup.FindValue(PmrSetupPositions.ExpandRawKey(condition.RawKey, position));
        return controllingValue is null
               || condition.RawValues.Any(rawValue => Math.Abs(rawValue - controllingValue.Value) < RawValueTolerance);
    }

    private string ReasonFor(PmrSetupFieldCondition condition)
    {
        var controllingField = this.allFields.FirstOrDefault(field => field.RawKey == condition.RawKey);
        var activeValues = string.Join(AlternativesSeparator, condition.RawValues.Select(rawValue => DescribeRawValue(controllingField, rawValue)));
        return PmrSetupExplanations.NotActiveUnless(controllingField?.Name ?? condition.RawKey, activeValues);
    }
}
