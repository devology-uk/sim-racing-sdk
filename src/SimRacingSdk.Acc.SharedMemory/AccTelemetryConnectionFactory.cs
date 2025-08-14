using SimRacingSdk.Acc.SharedMemory.Abstractions;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccTelemetryConnectionFactory : IAccTelemetryConnectionFactory
{
    private static AccTelemetryConnectionFactory? singletonInstance;
    public static AccTelemetryConnectionFactory Instance =>
        singletonInstance ??= new AccTelemetryConnectionFactory();

    public IAccTelemetryConnection Create()
    {
        return new AccTelemetryConnection(new AccSharedMemoryProvider());
    }
}