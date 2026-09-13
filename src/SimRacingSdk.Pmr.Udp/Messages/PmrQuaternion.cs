namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrQuaternion
{
    public float X { get; init; }
    public float Y { get; init; }
    public float Z { get; init; }
    public float W { get; init; }
}
