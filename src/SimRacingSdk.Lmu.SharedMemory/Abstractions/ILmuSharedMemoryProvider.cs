using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Abstractions;

public interface ILmuSharedMemoryProvider
{
    LmuExtendedBuffer? ReadExtended();
    Rf2ScoringBuffer? ReadScoring();
    Rf2TelemetryBuffer? ReadTelemetry();
}
