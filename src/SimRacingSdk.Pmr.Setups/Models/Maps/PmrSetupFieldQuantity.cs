namespace SimRacingSdk.Pmr.Setups.Models.Maps;

// What a value measures, which decides how it's shown under the game's Units setting. Everything
// else (angles, %, N, kW, clicks, ratios, force feedback) shows the same in both unit systems.
public enum PmrSetupFieldQuantity
{
    None,
    Pressure,
    SpringRate,
    Length,
    Volume
}
