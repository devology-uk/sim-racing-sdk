namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccTelemetryConnectionFactory
{
    IAccTelemetryConnection Create();
}