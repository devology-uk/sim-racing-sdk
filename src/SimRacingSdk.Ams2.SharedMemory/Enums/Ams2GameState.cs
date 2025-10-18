namespace SimRacingSdk.Ams2.SharedMemory.Enums;

public enum Ams2GameState: uint
{
    Exited = 0,
    FrontEnd,
    InGamePlaying,
    InGamePaused,
    InGameInMenuTimeTicking,
    InGameRestarting,
    InGameReplay,
    FrontEndReplay
}