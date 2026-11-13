namespace SimRacingSdk.R3e.Setups.Models;

// What a value converts as when the game's Measurement Units setting is Imperial. The game works in
// Metric. None covers values that don't convert, including two the game leaves in N/mm in Imperial:
// the anti-roll bars and the rear third spring.
public enum R3eSetupQuantity
{
    None,
    SpringRate,
    Pressure,
    Length
}
