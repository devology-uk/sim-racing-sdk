#nullable disable

using SimRacingSdk.Acc.Udp.Enums;

namespace SimRacingSdk.Acc.Udp.Messages;

public record RealtimeCarUpdate
{
    public int CarIndex { get; init; }
    public int DriverIndex { get; init; }
    public int Gear { get; init; }
    public float WorldPosX { get; init; }
    public float WorldPosY { get; init; }
    public float Yaw { get; init; }
    public CarLocation CarLocation { get; init; }
    public int Kmh { get; init; }
    public int Position { get; init; }
    public int TrackPosition { get; init; }
    public float SplinePosition { get; init; }
    public int Delta { get; init; }
    public LapInfo BestSessionLap { get; init; }
    public LapInfo LastLap { get; init; }
    public LapInfo CurrentLap { get; init; }
    public int Laps { get; init; }
    public ushort CupPosition { get; init; }
    public byte DriverCount { get; init; }
}