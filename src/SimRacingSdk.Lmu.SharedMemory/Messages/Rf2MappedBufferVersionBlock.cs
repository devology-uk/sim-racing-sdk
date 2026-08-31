#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Precedes every buffer written by both plugins (rF2MappedBufferVersionBlock / LMU_MappedBufferVersionBlock -
// identical shape in both). Incremented before and after each write; Begin != End means a write is in progress.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Rf2MappedBufferVersionBlock
{
    public uint VersionUpdateBegin;
    public uint VersionUpdateEnd;
}
