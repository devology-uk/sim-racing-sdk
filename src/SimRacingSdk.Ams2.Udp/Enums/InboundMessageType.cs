namespace SimRacingSdk.Ams2.Udp.Enums;

public enum InboundMessageType: byte
{
    Telemetry,
    RaceInfo,
    Participants,
    Timings,
    GameState,
    TimeStats,
    VehicleInfo
}