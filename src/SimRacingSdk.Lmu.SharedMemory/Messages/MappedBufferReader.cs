using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Both plugins precede their buffer with a Rf2MappedBufferVersionBlock and increment VersionUpdateBegin/End around
// each write (see MappedBuffer.h in either plugin repo) - reading the version block before and after copying the
// buffer, and retrying if it changed mid-copy, avoids marshalling a frame that was torn by a concurrent write.
internal static class MappedBufferReader
{
    private const int MaxAttempts = 5;

    public static T? Read<T>(string mappedFileName)
        where T : struct
    {
        try
        {
            using var mappedFile = MemoryMappedFile.OpenExisting(mappedFileName, MemoryMappedFileRights.Read);

            var versionBlockSize = Marshal.SizeOf<Rf2MappedBufferVersionBlock>();
            var bufferSize = Marshal.SizeOf<T>();
            using var stream =
                mappedFile.CreateViewStream(0, versionBlockSize + bufferSize, MemoryMappedFileAccess.Read);

            var versionBlockBytes = new byte[versionBlockSize];
            var bufferBytes = new byte[bufferSize];

            for(var attempt = 0; attempt < MaxAttempts; attempt++)
            {
                stream.Position = 0;
                stream.ReadExactly(versionBlockBytes, 0, versionBlockSize);
                var versionBegin = ToStruct<Rf2MappedBufferVersionBlock>(versionBlockBytes);

                if(versionBegin.VersionUpdateBegin != versionBegin.VersionUpdateEnd)
                {
                    continue;
                }

                stream.ReadExactly(bufferBytes, 0, bufferSize);

                stream.Position = 0;
                stream.ReadExactly(versionBlockBytes, 0, versionBlockSize);
                var versionEnd = ToStruct<Rf2MappedBufferVersionBlock>(versionBlockBytes);

                if(versionEnd.VersionUpdateBegin == versionBegin.VersionUpdateBegin &&
                   versionEnd.VersionUpdateEnd == versionBegin.VersionUpdateBegin)
                {
                    return ToStruct<T>(bufferBytes);
                }
            }

            return null;
        }
        catch(Exception)
        {
            return null;
        }
    }

    private static TStruct ToStruct<TStruct>(byte[] bytes)
        where TStruct : struct
    {
        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        try
        {
            return Marshal.PtrToStructure<TStruct>(handle.AddrOfPinnedObject());
        }
        finally
        {
            handle.Free();
        }
    }
}
