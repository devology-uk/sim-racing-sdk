namespace SimRacingSdk.Lmu.SharedMemory.Enums;

// Matches SharedMemoryEvent (SharedMemoryInterface.hpp) - indexes into LmuSharedMemoryGeneric.Events, where each
// slot is a flag (non-zero = signalled since the last read) rather than a single active value.
public enum LmuSharedMemoryEventType
{
    Enter = 0,
    Exit = 1,
    Startup = 2,
    Shutdown = 3,
    Load = 4,
    Unload = 5,
    StartSession = 6,
    EndSession = 7,
    EnterRealtime = 8,
    ExitRealtime = 9,
    UpdateScoring = 10,
    UpdateTelemetry = 11,
    InitApplication = 12,
    UninitApplication = 13,
    SetEnvironment = 14,
    Ffb = 15
}
