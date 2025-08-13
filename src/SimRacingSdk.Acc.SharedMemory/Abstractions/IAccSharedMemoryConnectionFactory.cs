namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccSharedMemoryConnectionFactory
{
    IAccSharedMemoryConnection Create();
}