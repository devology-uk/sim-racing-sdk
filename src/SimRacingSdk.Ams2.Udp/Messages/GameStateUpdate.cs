using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp.Messages;

public record GameStateUpdate : MessageBase
{
    public GameStateUpdate(MessageHeader header)
        : base(header) { }

    public byte AmbientTemperature { get; internal set; }
    public short BuildVersionNumber { get; init; }
    public int GameState { get; internal set; }
    public byte RainDensity { get; internal set; }
    public int SessionState { get; internal set; }
    public byte SnowDensity { get; internal set; }
    public byte TrackTemperature { get; internal set; }
    public byte WindDirectionX { get; internal set; }
    public byte WindDirectionY { get; internal set; }
    public byte WindSpeed { get; internal set; }
}