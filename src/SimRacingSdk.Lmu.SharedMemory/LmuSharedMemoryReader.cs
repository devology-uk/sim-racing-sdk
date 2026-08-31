using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory;

// Opens the game's native "LMU_Data" mapped file (SharedMemoryInterface.hpp) once and reuses it across reads,
// taking LmuSharedMemoryLock around each copy so a read can't race with the game's own write.
internal sealed class LmuSharedMemoryReader : IDisposable
{
    private const string SharedMemoryFileName = "LMU_Data";

    private readonly byte[] buffer;
    private readonly int bufferSize;
    private readonly MemoryMappedFile mappedFile;
    private readonly MemoryMappedViewStream viewStream;

    private LmuSharedMemoryReader(MemoryMappedFile mappedFile, MemoryMappedViewStream viewStream, int bufferSize)
    {
        this.mappedFile = mappedFile;
        this.viewStream = viewStream;
        this.bufferSize = bufferSize;
        this.buffer = new byte[bufferSize];
    }

    public void Dispose()
    {
        this.viewStream.Dispose();
        this.mappedFile.Dispose();
    }

    public static LmuSharedMemoryReader? TryOpen(out string? failureReason)
    {
        try
        {
            var bufferSize = Marshal.SizeOf<LmuSharedMemoryObjectOut>();
            var mappedFile = MemoryMappedFile.OpenExisting(SharedMemoryFileName, MemoryMappedFileRights.Read);
            var viewStream = mappedFile.CreateViewStream(0, bufferSize, MemoryMappedFileAccess.Read);
            failureReason = null;
            return new LmuSharedMemoryReader(mappedFile, viewStream, bufferSize);
        }
        catch(Exception exception)
        {
            failureReason = exception.Message;
            return null;
        }
    }

    public LmuSharedMemoryObjectOut? Read(LmuSharedMemoryLock sharedMemoryLock)
    {
        if(!sharedMemoryLock.TryAcquire(TimeSpan.FromMilliseconds(100)))
        {
            throw new TimeoutException("Timed out waiting to acquire the LMU shared memory lock.");
        }

        try
        {
            this.viewStream.Position = 0;
            this.viewStream.ReadExactly(this.buffer, 0, this.bufferSize);
        }
        finally
        {
            sharedMemoryLock.Release();
        }

        var handle = GCHandle.Alloc(this.buffer, GCHandleType.Pinned);
        try
        {
            return Marshal.PtrToStructure<LmuSharedMemoryObjectOut>(handle.AddrOfPinnedObject());
        }
        finally
        {
            handle.Free();
        }
    }
}
