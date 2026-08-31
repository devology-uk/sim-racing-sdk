using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory;

public class LmuSharedMemoryProvider : ILmuSharedMemoryProvider
{
    private const string ExtendedMapName = "$LMU_SMMP_Extended$";
    private const string ScoringMapName = "$rFactor2SMMP_Scoring$";
    private const string TelemetryMapName = "$rFactor2SMMP_Telemetry$";

    private static LmuSharedMemoryProvider? singletonInstance;

    public static LmuSharedMemoryProvider Instance => singletonInstance ??= new LmuSharedMemoryProvider();

    public LmuExtendedBuffer? ReadExtended()
    {
        return MappedBufferReader.Read<LmuExtendedBuffer>(ExtendedMapName);
    }

    public Rf2ScoringBuffer? ReadScoring()
    {
        return MappedBufferReader.Read<Rf2ScoringBuffer>(ScoringMapName);
    }

    public Rf2TelemetryBuffer? ReadTelemetry()
    {
        return MappedBufferReader.Read<Rf2TelemetryBuffer>(TelemetryMapName);
    }
}
