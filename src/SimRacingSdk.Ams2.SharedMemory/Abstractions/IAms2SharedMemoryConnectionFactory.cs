namespace SimRacingSdk.Ams2.SharedMemory.Abstractions;

public interface IAms2SharedMemoryConnectionFactory
{
    IAms2SharedMemoryConnection Create();
}