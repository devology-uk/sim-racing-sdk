using SimRacingSdk.Core.Messages;
using SimRacingSdk.R3e.SharedMemory.Messages;

namespace SimRacingSdk.R3e.SharedMemory.Abstractions;

public interface IR3eSharedMemoryProvider : IDisposable
{
    IObservable<LogMessage> LogMessages { get; }

    R3eShared? Read();
}
