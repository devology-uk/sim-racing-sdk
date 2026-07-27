namespace SimRacingSdk.Ace.SharedMemory.Models;

// Mirrors focused_car_id_a/b and player_car_id_a/b: Evo appears to represent a car UID as
// two combined uint64 halves. car_ids[60][2] in the PDF is treated as 60 of these pairs.
public record struct AceCarIdPair
{
    public ulong A;
    public ulong B;
}
