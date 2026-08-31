using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Abstractions;

public interface ILmuSharedMemoryProvider : IDisposable
{
    LmuSharedMemoryObjectOut? Read();
}
