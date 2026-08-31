using System.IO.MemoryMappedFiles;

namespace SimRacingSdk.Lmu.SharedMemory;

// Cooperates with the game's own SharedMemoryLock (SharedMemoryInterface.hpp) via the same named kernel objects and
// the same InterlockedCompareExchange spinlock protocol on LMU_SharedMemoryLockData's "busy" field (the second of
// its two 4-byte fields - {waiters, busy}), so a read here can't race with the game's own write. Read-only: never
// touches "waiters", since the game doesn't need to know a reader is blocked - only needs correct interop on "busy".
internal sealed unsafe class LmuSharedMemoryLock : IDisposable
{
    private const string LockEventName = "LMU_SharedMemoryLockEvent";
    private const string LockMapName = "LMU_SharedMemoryLockData";
    private const int LockDataSize = 8;
    private const int MaxSpins = 4000;

    private readonly int* busyPointer;
    private readonly EventWaitHandle lockEvent;
    private readonly MemoryMappedFile lockMap;
    private readonly MemoryMappedViewAccessor lockView;

    private LmuSharedMemoryLock(MemoryMappedFile lockMap, MemoryMappedViewAccessor lockView, EventWaitHandle lockEvent)
    {
        this.lockMap = lockMap;
        this.lockView = lockView;
        this.lockEvent = lockEvent;

        byte* viewPointer = null;
        this.lockView.SafeMemoryMappedViewHandle.AcquirePointer(ref viewPointer);
        this.busyPointer = (int*)(viewPointer + sizeof(int));
    }

    public void Dispose()
    {
        this.lockView.SafeMemoryMappedViewHandle.ReleasePointer();
        this.lockView.Dispose();
        this.lockMap.Dispose();
        this.lockEvent.Dispose();
    }

    public static LmuSharedMemoryLock? TryOpen(out string? failureReason)
    {
        try
        {
            var lockMap = MemoryMappedFile.OpenExisting(LockMapName, MemoryMappedFileRights.ReadWrite);
            var lockView = lockMap.CreateViewAccessor(0, LockDataSize, MemoryMappedFileAccess.ReadWrite);
            var lockEvent = EventWaitHandle.OpenExisting(LockEventName);
            failureReason = null;
            return new LmuSharedMemoryLock(lockMap, lockView, lockEvent);
        }
        catch(Exception exception)
        {
            failureReason = exception.Message;
            return null;
        }
    }

    public void Release()
    {
        Interlocked.Exchange(ref *this.busyPointer, 0);
        this.lockEvent.Set();
    }

    public bool TryAcquire(TimeSpan timeout)
    {
        var deadline = DateTime.UtcNow + timeout;

        for(var spin = 0; spin < MaxSpins; spin++)
        {
            if(this.TryClaimBusy())
            {
                return true;
            }
        }

        while(DateTime.UtcNow < deadline)
        {
            if(this.TryClaimBusy())
            {
                return true;
            }

            this.lockEvent.WaitOne(TimeSpan.FromMilliseconds(5));
        }

        return false;
    }

    private bool TryClaimBusy()
    {
        return Interlocked.CompareExchange(ref *this.busyPointer, 1, 0) == 0;
    }
}
