using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory;

// Both the mapped file and its lock are per-machine, per-game-instance OS resources - not something that makes
// sense to open once per Connection - so this is shared as a singleton and lazily (re)opens each on first use, or
// after either fails to open, since the game may not be running yet when a consumer starts polling.
public class LmuSharedMemoryProvider : ILmuSharedMemoryProvider
{
    private static LmuSharedMemoryProvider? singletonInstance;

    private LmuSharedMemoryLock? sharedMemoryLock;
    private LmuSharedMemoryReader? sharedMemoryReader;

    public static LmuSharedMemoryProvider Instance => singletonInstance ??= new LmuSharedMemoryProvider();

    public void Dispose()
    {
        this.sharedMemoryReader?.Dispose();
        this.sharedMemoryReader = null;
        this.sharedMemoryLock?.Dispose();
        this.sharedMemoryLock = null;
        GC.SuppressFinalize(this);
    }

    public LmuSharedMemoryObjectOut? Read()
    {
        this.sharedMemoryReader ??= LmuSharedMemoryReader.TryOpen();
        this.sharedMemoryLock ??= LmuSharedMemoryLock.TryOpen();

        if(this.sharedMemoryReader is null || this.sharedMemoryLock is null)
        {
            return null;
        }

        return this.sharedMemoryReader.Read(this.sharedMemoryLock);
    }
}
