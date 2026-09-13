namespace SimRacingSdk.Pmr.Udp.Enums;

public enum PmrPacketType : byte
{
    RaceInfo = 0,
    ParticipantRaceState = 1,
    ParticipantVehicleTelemetry = 2,
    SessionStopped = 3
}
