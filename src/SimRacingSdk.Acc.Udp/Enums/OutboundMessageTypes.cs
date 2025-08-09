namespace SimRacingSdk.Acc.Udp.Enums;

public enum OutboundMessageTypes : byte
{
    RegisterCommandApplication = 1,
    UnregisterCommandApplication = 9,
    RequestEntryList = 10,
    RequestTrackData = 11,
    ChangeHudPage = 49,
    ChangeFocus = 50,
    InstantReplayRequest = 51,
    PlayManualReplayHighlight = 52,
    SaveManualReplayHighlight = 60
}