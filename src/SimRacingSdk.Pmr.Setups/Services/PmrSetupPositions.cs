using SimRacingSdk.Pmr.Setups.Models.Maps;
using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Services;

internal static class PmrSetupPositions
{
    private const string AxlePlaceholder = "{Axle}";
    private const string CornerPlaceholder = "{Corner}";

    public static string ExpandRawKey(string rawKeyTemplate, PmrSetupPosition position)
    {
        return rawKeyTemplate.Replace(CornerPlaceholder, CornerCode(position))
                             .Replace(AxlePlaceholder, AxleCode(position));
    }

    public static IReadOnlyList<PmrSetupPosition> For(PmrSetupFieldScope scope)
    {
        return scope switch
        {
            PmrSetupFieldScope.FrontRear => [PmrSetupPosition.Front, PmrSetupPosition.Rear],
            PmrSetupFieldScope.PerCorner =>
            [
                PmrSetupPosition.FrontLeft, PmrSetupPosition.FrontRight, PmrSetupPosition.RearLeft, PmrSetupPosition.RearRight
            ],
            PmrSetupFieldScope.Front => [PmrSetupPosition.FrontLeft, PmrSetupPosition.FrontRight],
            PmrSetupFieldScope.Rear => [PmrSetupPosition.RearLeft, PmrSetupPosition.RearRight],
            _ => [PmrSetupPosition.Car]
        };
    }

    private static string AxleCode(PmrSetupPosition position)
    {
        return position switch
        {
            PmrSetupPosition.Front or PmrSetupPosition.FrontLeft or PmrSetupPosition.FrontRight => "F",
            PmrSetupPosition.Rear or PmrSetupPosition.RearLeft or PmrSetupPosition.RearRight => "R",
            _ => string.Empty
        };
    }

    private static string CornerCode(PmrSetupPosition position)
    {
        return position switch
        {
            PmrSetupPosition.FrontLeft => "FL",
            PmrSetupPosition.FrontRight => "FR",
            PmrSetupPosition.RearLeft => "RL",
            PmrSetupPosition.RearRight => "RR",
            _ => string.Empty
        };
    }
}
