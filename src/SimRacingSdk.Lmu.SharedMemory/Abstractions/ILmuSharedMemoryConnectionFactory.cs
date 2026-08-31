namespace SimRacingSdk.Lmu.SharedMemory.Abstractions;

public interface ILmuSharedMemoryConnectionFactory
{
    ILmuSharedMemoryConnection Create();
}
