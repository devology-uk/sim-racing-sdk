namespace SimRacingSdk.Acc.Udp.Enums;

public enum InboundMessageType : byte
{
    RegistrationResult = 1,
    RealtimeUpdate = 2,
    RealtimeCarUpdate = 3,
    EntryList = 4,
    EntryListCar = 6,
    TrackData = 5,
    BroadcastingEvent = 7
}