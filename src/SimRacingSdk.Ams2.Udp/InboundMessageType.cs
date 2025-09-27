namespace SimRacingSdk.Ams2.Udp;

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