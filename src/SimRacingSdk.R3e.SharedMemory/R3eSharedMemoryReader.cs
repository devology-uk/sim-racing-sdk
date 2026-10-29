using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using SimRacingSdk.R3e.SharedMemory.Messages;

namespace SimRacingSdk.R3e.SharedMemory;

// Opens the game's "$R3E" mapped file once and reuses the view across reads. The game has no lock or update
// counter, so a read is taken as-is and an occasional torn frame is accepted.
internal sealed class R3eSharedMemoryReader : IDisposable
{
    private const string SharedMemoryFileName = "$R3E";

    private readonly byte[] buffer;
    private readonly int bufferSize;
    private readonly MemoryMappedFile mappedFile;
    private readonly MemoryMappedViewStream viewStream;

    private R3eSharedMemoryReader(MemoryMappedFile mappedFile, MemoryMappedViewStream viewStream, int bufferSize)
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

    public static R3eSharedMemoryReader? TryOpen(out string? failureReason)
    {
        try
        {
            var bufferSize = Marshal.SizeOf<R3eShared>();
            var mappedFile = MemoryMappedFile.OpenExisting(SharedMemoryFileName, MemoryMappedFileRights.Read);
            var viewStream = mappedFile.CreateViewStream(0, bufferSize, MemoryMappedFileAccess.Read);
            failureReason = null;
            return new R3eSharedMemoryReader(mappedFile, viewStream, bufferSize);
        }
        catch(Exception exception)
        {
            failureReason = exception.Message;
            return null;
        }
    }

    public R3eShared Read()
    {
        this.viewStream.Position = 0;
        this.viewStream.ReadExactly(this.buffer, 0, this.bufferSize);

        var handle = GCHandle.Alloc(this.buffer, GCHandleType.Pinned);
        try
        {
            return Marshal.PtrToStructure<R3eShared>(handle.AddrOfPinnedObject());
        }
        finally
        {
            handle.Free();
        }
    }
}
