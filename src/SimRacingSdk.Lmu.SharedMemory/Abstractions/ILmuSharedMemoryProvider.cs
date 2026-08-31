using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Abstractions;

public interface ILmuSharedMemoryProvider : IDisposable
{
    IObservable<LogMessage> LogMessages { get; }

    LmuSharedMemoryObjectOut? Read();
}
